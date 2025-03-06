import os
import json
import logging
import google_auth_oauthlib.flow
import googleapiclient.discovery
import googleapiclient.errors

# Set your OAuth 2.0 client secrets file, API service name, and API version
CLIENT_SECRETS_FILE = 'client_secret.json'
API_SERVICE_NAME = 'youtube'
API_VERSION = 'v3'

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


def get_authenticated_service():
    flow = google_auth_oauthlib.flow.InstalledAppFlow.from_client_secrets_file(
        CLIENT_SECRETS_FILE, ['https://www.googleapis.com/auth/youtube.force-ssl'])
    credentials = flow.run_local_server(port=8080)
    youtube = googleapiclient.discovery.build(
        API_SERVICE_NAME, API_VERSION, credentials=credentials)
    return youtube


def is_video_embeddable(youtube, video_id):
    try:
        video_response = youtube.videos().list(
            part='status',
            id=video_id
        ).execute()

        items = video_response.get('items', [])
        if not items:
            logger.warning(f'No video information found for video {video_id}. Assuming the video is faulty.')
            return False

        status = items[0].get('status', {})
        embeddable = status.get('embeddable', True)

        return embeddable

    except googleapiclient.errors.HttpError as e:
        logger.error(f'An error occurred while checking embeddable status for video {video_id}: {e}')
        return False


def extract_video_details(playlist_items, youtube):
    # Extract video details from playlist items
    videos = []
    for item in playlist_items.get('items', []):
        video_title = item['snippet']['title']
        video_id = item['snippet']['resourceId']['videoId']

        # Check if the video is faulty (private, unlisted, or taken down)
        embeddable = is_video_embeddable(youtube, video_id)
        privacy_status = item['snippet'].get('privacyStatus', 'public')

        faulty = 1 if privacy_status != 'public' or not embeddable else 0

        video_link = f'https://www.youtube.com/watch?v={video_id}'
        videos.append({'title': video_title, 'link': video_link, 'faulty': faulty})

    return videos


def fetch_and_cache_playlist(youtube, playlist_id):
    cache_file = f'playlist_cache_{playlist_id}.json'

    try:
        # Check if the cache file exists and load cached data
        if os.path.exists(cache_file):
            with open(cache_file, 'r') as file:
                videos = json.load(file)
            logger.info('Using cached data.')
        else:
            videos = []

            # Continue making requests until there are no more pages
            next_page_token = None
            while True:
                # Request to get videos with limited fields
                playlist_items = youtube.playlistItems().list(
                    playlistId=playlist_id,
                    part='snippet',
                    maxResults=50,
                    pageToken=next_page_token,
                    fields='items(snippet(title,resourceId(videoId))),nextPageToken'
                ).execute()

                # Extract video details from the response
                videos.extend(extract_video_details(playlist_items, youtube))

                # Break the loop if no more pages
                if 'nextPageToken' not in playlist_items:
                    break

                next_page_token = playlist_items['nextPageToken']

            # Add 'id' tag to each video
            for i, video in enumerate(videos, start=1):
                video['id'] = i

            # Cache the fetched data with each entry in a new line
            with open(cache_file, 'w') as file:
                json.dump(videos, file, indent=2)

            logger.info('Data cached.')

        return videos

    except googleapiclient.errors.HttpError as e:
        logger.error(f'An error occurred: {e}')
        return None



def main():
    # Set the OAuth 2.0 client secrets file path
    CLIENT_SECRETS_FILE = 'client_secret.json'

    # Hardcoded YouTube playlist ID
    playlist_id = 'PL4hoapFeS8lxPEzoF_MatO5FopX61FGxb'  # Replace with your actual playlist ID

    # Get authenticated YouTube API service
    youtube_service = get_authenticated_service()

    # Fetch and cache all videos from the playlist
    cached_playlist = fetch_and_cache_playlist(youtube_service, playlist_id)

    if cached_playlist:
        logger.info('Playlist cached successfully!')
    else:
        logger.error('Unable to cache the playlist.')


if __name__ == '__main__':
    main()
