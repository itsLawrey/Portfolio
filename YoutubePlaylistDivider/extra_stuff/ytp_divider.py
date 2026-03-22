import os
import time
import google_auth_oauthlib.flow
import googleapiclient.discovery
import googleapiclient.errors

# Set your OAuth 2.0 client secrets file, API service name, and API version
CLIENT_SECRETS_FILE = 'client_secret.json'
API_SERVICE_NAME = 'youtube'
API_VERSION = 'v3'

def get_authenticated_service():
    flow = google_auth_oauthlib.flow.InstalledAppFlow.from_client_secrets_file(
        CLIENT_SECRETS_FILE, ['https://www.googleapis.com/auth/youtube.force-ssl'])
    credentials = flow.run_local_server(port=8080)
    youtube = googleapiclient.discovery.build(API_SERVICE_NAME, API_VERSION, credentials=credentials)
    return youtube

import json

def get_all_playlist_videos(youtube, playlist_id):
    cache_file = f'playlist_cache_{playlist_id}.json'

    try:
        # Check if cache file exists and load cached data
        if os.path.exists(cache_file):
            with open(cache_file, 'r') as file:
                videos = json.load(file)
            print('Using cached data.')
        else:
            # Initial request to get the first page of videos
            playlist_items = youtube.playlistItems().list(
                playlistId=playlist_id,
                part='snippet',
                maxResults=50
            ).execute()

            # Extract video details from the initial response
            videos = extract_video_details(playlist_items)

            # Continue making requests until there are no more pages
            while 'nextPageToken' in playlist_items:
                next_page_token = playlist_items['nextPageToken']
                playlist_items = youtube.playlistItems().list(
                    playlistId=playlist_id,
                    part='snippet',
                    maxResults=50,
                    pageToken=next_page_token
                ).execute()
                videos.extend(extract_video_details(playlist_items))

            # Cache the fetched data
            with open(cache_file, 'w') as file:
                json.dump(videos, file)
            print('Data cached.')

        return videos

    except googleapiclient.errors.HttpError as e:
        print(f'An error occurred: {e}')
        return None


def extract_video_details(playlist_items):
    # Extract video details from playlist items
    videos = []
    for item in playlist_items.get('items', []):
        video_title = item['snippet']['title']
        video_id = item['snippet']['resourceId']['videoId']
        video_link = f'https://www.youtube.com/watch?v={video_id}'
        videos.append({'title': video_title, 'link': video_link})
    return videos

def create_playlist(youtube, title, description):
    try:
        # Call the playlists.insert method to create a new playlist
        playlist = youtube.playlists().insert(
            part='snippet',
            body={
                'snippet': {
                    'title': title,
                    'description': description
                }
            }
        ).execute()

        playlist_id = playlist['id']
        print(f'Playlist created successfully!\nPlaylist Title: {title}\nPlaylist ID: {playlist_id}')

        return playlist_id

    except googleapiclient.errors.HttpError as e:
        print(f'An error occurred: {e}')
        return None

def add_videos_to_playlist(youtube, playlist_id, videos):
    # Check if the playlist is not empty
    existing_items = youtube.playlistItems().list(
        playlistId=playlist_id,
        part='snippet',
        maxResults=1
    ).execute().get('items', [])

    if existing_items:
        print(f'Playlist is not empty. Skipping video insertion.')
        return

    # Insert videos into the playlist one by one with a delay
    for video in videos:
        try:
            youtube.playlistItems().insert(
                part='snippet',
                body={
                    'snippet': {
                        'playlistId': playlist_id,
                        'resourceId': {
                            'kind': 'youtube#video',
                            'videoId': video['link'].split('=')[-1]
                        }
                    }
                }
            ).execute()
            print(f'Inserted video: {video["title"]}. Waiting 0.1s...\n')
            time.sleep(0.1)
        except googleapiclient.errors.HttpError as e:
            print(f'An error occurred while inserting videos: {e}\n')

def countdown_timer(seconds):
    for i in range(seconds, 0, -1):
        print(f'Waiting... {i}s', end='\r')
        time.sleep(1)
    print('')

if __name__ == '__main__':
    # Set the OAuth 2.0 client secrets file path
    CLIENT_SECRETS_FILE = 'client_secret.json'

    # Set the existing playlist ID
    existing_playlist_id = input('Enter the existing YouTube playlist ID: ')

    # Get authenticated YouTube API service
    youtube_service = get_authenticated_service()

    # Countdown before fetching playlist videos
    countdown_timer(5)

    # Call the get_all_playlist_videos function to get all videos from the existing playlist
    existing_playlist_videos = get_all_playlist_videos(youtube_service, existing_playlist_id)

    if existing_playlist_videos:
        # Calculate the midpoint to split the playlist into two halves
        midpoint = len(existing_playlist_videos) // 2

        # Get the name of the existing playlist
        existing_playlist_info = youtube_service.playlists().list(
            part='snippet',
            id=existing_playlist_id
        ).execute()

        existing_playlist_title = existing_playlist_info['items'][0]['snippet']['title']

        # Create the first new playlist
        first_half_playlist_id = create_playlist(youtube_service, f'{existing_playlist_title} - First Half', 'First Half Playlist Description')

        if first_half_playlist_id:
            # Countdown before adding videos to the first playlist
            countdown_timer(10)

            # Add videos to the first half playlist maintaining original order
            add_videos_to_playlist(youtube_service, first_half_playlist_id, existing_playlist_videos[:midpoint])

            # Create the second new playlist
            second_half_playlist_id = create_playlist(youtube_service, f'{existing_playlist_title} - Second Half', 'Second Half Playlist Description')

            if second_half_playlist_id:
                # Countdown before adding videos to the second playlist
                countdown_timer(10)

                # Add videos to the second half playlist maintaining original order
                add_videos_to_playlist(youtube_service, second_half_playlist_id, existing_playlist_videos[midpoint:])

                print(f'Two new playlists created successfully!\nFirst Half Playlist ID: {first_half_playlist_id}\nSecond Half Playlist ID: {second_half_playlist_id}')
            else:
                print('Failed to create the second playlist.')
        else:
            print('Failed to create the first playlist.')
    else:
        print("Unable to fetch existing playlist videos.")
