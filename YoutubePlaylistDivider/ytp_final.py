import os
import sys
import time
import argparse
import logging
import google_auth_oauthlib.flow
import googleapiclient.discovery
import googleapiclient.errors
import json

# Set your OAuth 2.0 client secrets file, API service name, and API version
CLIENT_SECRETS_FILE = 'client_secret.json'
API_SERVICE_NAME = 'youtube'
API_VERSION = 'v3'

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Set sleep time between video insertions
SLEEP_TIME = 0.2

def get_authenticated_service():
    flow = google_auth_oauthlib.flow.InstalledAppFlow.from_client_secrets_file(
        CLIENT_SECRETS_FILE, ['https://www.googleapis.com/auth/youtube.force-ssl'])
    credentials = flow.run_local_server(port=8080)
    youtube = googleapiclient.discovery.build(
        API_SERVICE_NAME, API_VERSION, credentials=credentials)
    return youtube

def add_videos_to_playlist(youtube, playlist_id, cache_file, videos, start_index, end_index):
    # Set the sleep time between video insertions
    sleep_time = 0.3
    videos_processed = 0

    for current_idx in range(start_index, end_index):
        if current_idx >= len(videos):
            break
            
        video = videos[current_idx]
        if video.get('faulty', 0) == 1:
            continue
            
        video_url = video['link']
        video_id = video_url.split('=')[-1]

        # Create snippet for each video
        snippet = {
            'playlistId': playlist_id,
            'resourceId': {
                'kind': 'youtube#video',
                'videoId': video_id
            }
        }

        # Insert video
        request = youtube.playlistItems().insert(part='snippet', body={'snippet': snippet})

        try:
            # Execute the request and add a delay
            request.execute()
            time.sleep(sleep_time)
            logger.info(f'Video with ID {video_id} (Entry ID {video["id"]}) inserted into the playlist.')
            videos_processed += 1
        except googleapiclient.errors.HttpError as e:
            logger.error(f'Error inserting video with ID {video_id} (Entry ID {video["id"]}): {e}')
            state = {
                "dest_playlist_id": playlist_id,
                "cache_file": cache_file,
                "resume_index": current_idx,
                "original_end": end_index,
                "interruption_reason": str(e)
            }
            with open("resume_state.json", "w") as f:
                json.dump(state, f, indent=4)
            print("\n[!] INTERRUPTED! Smart Resume state saved.")
            print(f"[!] You can seamlessly resume from index {current_idx} tomorrow when your quota resets.\n")
            sys.exit(1)

    logger.info(f'Inserted {videos_processed} videos into playlist {playlist_id}.')
    
    if os.path.exists("resume_state.json"):
        open("resume_state.json", "w").close()


    
def load_json_file(file_path):
    try:
        with open(file_path, 'r') as file:
            return json.load(file)
    except FileNotFoundError:
        logger.error(f'Error: File not found - {file_path}')
        return None
    except json.JSONDecodeError as e:
        logger.error(f'Error decoding JSON file: {e}')
        return None


def main():
    # Set the OAuth 2.0 client secrets file path
    CLIENT_SECRETS_FILE = 'client_secret.json'

    # Check if client_secret.json exists
    if not os.path.exists(CLIENT_SECRETS_FILE):
        logger.error(f"Missing {CLIENT_SECRETS_FILE}!")
        print("\n[!] You need a Google Cloud OAuth 2.0 Client Secret to use this tool.")
        print("1. Go to Google Cloud Console (https://console.cloud.google.com/)")
        print("2. Create a project and enable 'YouTube Data API v3'")
        print("3. Create an OAuth 2.0 Client ID (Desktop App)")
        print(f"4. Download the JSON file, rename it to '{CLIENT_SECRETS_FILE}', and place it in this folder.\n")
        sys.exit(1)

    # Setup argparse for command line support
    parser = argparse.ArgumentParser(description="Insert cached YouTube videos into a playlist.")
    parser.add_argument('-d', '--dest', type=str, help="Destination YouTube Playlist ID")
    parser.add_argument('-c', '--cache', type=str, help="Path to the JSON cache file")
    parser.add_argument('-s', '--start', type=int, help="Start index for insertion (e.g. 0)")
    parser.add_argument('-e', '--end', type=int, help="End index for insertion (e.g. 190)")
    args = parser.parse_args()

    RESUME_FILE = "resume_state.json"
    if os.path.exists(RESUME_FILE) and os.path.getsize(RESUME_FILE) > 0 and not (args.dest or args.cache or args.start or args.end):
        try:
            with open(RESUME_FILE, "r") as f:
                state = json.load(f)
            
            print("\n*** SMART RESUME STATE FOUND ***")
            print(f"Failed playing list ID: {state.get('dest_playlist_id')}")
            print(f"It stopped at index : {state.get('resume_index')}")
            
            ans = input("\nDo you want to RESUME from this exact spot? [Y/n]: ").strip().lower()
            if ans == 'n':
                open(RESUME_FILE, "w").close()
            else:
                youtube_service = get_authenticated_service()
                if not youtube_service: sys.exit(1)
                
                videos = load_json_file(state['cache_file'])
                if not videos: sys.exit("Error loading cached videos")
                
                print(f"Resuming insertion from index {state['resume_index']} to {state['original_end']}...\n")
                add_videos_to_playlist(youtube_service, state['dest_playlist_id'], state['cache_file'], videos, state['resume_index'], state['original_end'])
                sys.exit(0)
        except Exception as e:
            logger.warning(f"Could not read resume state. Ignoring. ({e})")

    # Destination Playlist ID
    dest_playlist_id = args.dest
    if not dest_playlist_id:
        print("\n--- Youtube Playlist Divider (Inserter) ---")
        dest_playlist_id = input("Enter the Destination Playlist ID: ").strip()
    
    if not dest_playlist_id:
        logger.error("No destination playlist provided.")
        sys.exit(1)

    # JSON Cache Path
    json_file_path = args.cache
    if not json_file_path:
        json_file_path = input("Enter the path to the JSON cache file [e.g. playlist_cache_xyz.json]: ").strip()
    
    if not json_file_path or not os.path.exists(json_file_path):
        logger.error("Invalid or missing JSON cache file.")
        sys.exit(1)

    # Get authenticated YouTube API service
    youtube_service = get_authenticated_service()
    if not youtube_service:
        sys.exit(1)

    # Load video details early so we can know the total amount
    videos = load_json_file(json_file_path)
    if not videos:
        logger.error("Could not load videos from cache file.")
        sys.exit(1)
        
    total_videos = len(videos)

    # Start Index
    start_index = args.start
    if start_index is None:
        try:
            start_input = input(f"Enter the start index [0 to {total_videos}]: ").strip()
            start_index = int(start_input) if start_input else 0
        except ValueError:
            logger.error("Start index must be an integer.")
            sys.exit(1)

    # End Index
    end_index = args.end
    if end_index is None:
        try:
            end_input = input(f"Enter the end index (e.g. {min(start_index + 190, total_videos)}): ").strip()
            end_index = int(end_input) if end_input else min(start_index + 190, total_videos)
        except ValueError:
            logger.error("End index must be an integer.")
            sys.exit(1)

    # Add videos to the playlist within the specified interval
    add_videos_to_playlist(youtube_service, dest_playlist_id, json_file_path, videos, start_index, end_index)


if __name__ == '__main__':
    main()
