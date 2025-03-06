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
    # Get existing video IDs in the playlist
    existing_video_ids = set()
    existing_items = youtube.playlistItems().list(
        playlistId=playlist_id,
        part='snippet',
        maxResults=50
    ).execute().get('items', [])

    existing_video_ids.update(
        item['snippet']['resourceId']['videoId'] for item in existing_items
    )

    # Filter videos based on the specified interval
    videos_to_insert = videos[start_index:end_index]

    for video in videos_to_insert:
        video_url = video['link']
        video_id = video_url.split('=')[-1]

        # Check if the video is already in the playlist
        if video_id in existing_video_ids:
            logger.info(f'Video with ID {video_id} already exists in the playlist. Skipping...')
            continue

        snippet = {
            'playlistId': playlist_id,
            'resourceId': {
                'kind': 'youtube#video',
                'videoId': video_id
            }
        }

        request = youtube.playlistItems().insert(part='snippet', body={'snippet': snippet})

        try:
            # Execute the request and add a delay
            request.execute()
            time.sleep(SLEEP_TIME)
            logger.info(f'Video with ID {video_id} inserted into the playlist.')
        except googleapiclient.errors.HttpError as e:
            if 'video not found' in str(e).lower() or 'forbidden' in str(e).lower():
                # Handle the error for faulty videos
                logger.warning(f'Faulty video with ID {video_id}. Skipping...')
            else:
                logger.error(f'Error inserting video with ID {video_id}: {e}')

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
    playlist_id = 'PL4hoapFeS8lxgep-Dz6mG1u-hjWxSatcW'  # Replace with your actual playlist ID

    # Set the path to the JSON file containing video details
    json_file_path = 'playlist_cache_PLP_LcnuF3YFk7i4h58rHdzuveAmCHW3fC.json'  # Replace with your actual JSON file path

    # Set the interval of videos to insert 
    # (1st element is 0)
    # (start included, end not included)
    start_index = 0
    end_index = 4

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
