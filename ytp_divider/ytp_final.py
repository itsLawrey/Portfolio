import os
import time
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

def add_videos_to_playlist(youtube, playlist_id, videos, start_index, end_index):
    # Set the sleep time between video insertions
    sleep_time = 0.3
    
    # Filter videos based on the specified interval
    videos_to_insert = [video for video in videos[start_index:end_index] if video.get('faulty', 0) == 0]

    for video in videos_to_insert:
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
        except googleapiclient.errors.HttpError as e:
            logger.error(f'Error inserting video with ID {video_id} (Entry ID {video["id"]}): {e}')

    logger.info(f'Inserted {len(videos_to_insert)} videos into playlist {playlist_id}.')


    
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

    # Set the YouTube playlist ID
    # Replace with your actual playlist ID
    playlist_id = 'PL4hoapFeS8lxgep-Dz6mG1u-hjWxSatcW'  

    # Set the path to the JSON file containing video details
    # Replace with your actual JSON file path
    json_file_path = 'botb.json'  

    # Set the interval of videos to insert 
    # (1st element is 0)
    # (start included, end not included)
    start_index = 1167
    end_index = 1276

    # Get authenticated YouTube API service
    youtube_service = get_authenticated_service()

    if youtube_service:
        # Load video details from the JSON file
        videos = load_json_file(json_file_path)

        if videos:
            # Add videos to the playlist within the specified interval
            add_videos_to_playlist(youtube_service, playlist_id, videos, start_index, end_index)


if __name__ == '__main__':
    main()
    #1273 az utso