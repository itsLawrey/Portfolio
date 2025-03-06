import os
import time
import google_auth_oauthlib.flow
import googleapiclient.discovery
import googleapiclient.errors
import json

# Set your OAuth 2.0 client secrets file, API service name, and API version
CLIENT_SECRETS_FILE = 'client_secret.json'
API_SERVICE_NAME = 'youtube'
API_VERSION = 'v3'


def batch_callback(request_id, response, exception):
    if exception is not None:
        error_message = f'Error inserting video: {exception}'
        print(error_message)

        # You can add more sophisticated error handling here if needed
        # For now, let's print the error message and move on


def get_authenticated_service():
    flow = google_auth_oauthlib.flow.InstalledAppFlow.from_client_secrets_file(
        CLIENT_SECRETS_FILE, ['https://www.googleapis.com/auth/youtube.force-ssl'])
    credentials = flow.run_local_server(port=8080)
    youtube = googleapiclient.discovery.build(
        API_SERVICE_NAME, API_VERSION, credentials=credentials)
    return youtube


# Modify get_all_playlist_videos function
def get_all_playlist_videos(youtube, playlist_id):
    cache_file = f'playlist_cache_{playlist_id}.json'

    try:
        # Check if cache file exists and load cached data
        if os.path.exists(cache_file):
            with open(cache_file, 'r') as file:
                videos = json.load(file)
            print('Using cached data.')
        else:
            # Initial request to get the first page of videos with limited fields
            playlist_items = youtube.playlistItems().list(
                playlistId=playlist_id,
                part='snippet',
                maxResults=50,
                fields='items(snippet(title,resourceId(videoId)))'
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
                    pageToken=next_page_token,
                    fields='items(snippet(title,resourceId(videoId)))'
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
        print(
            f'Playlist created successfully!\nPlaylist Title: {title}\nPlaylist ID: {playlist_id}')

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

    # Prepare batch request list for inserting videos
    request_list = []

    for video in videos:
        video_id = video['link'].split('=')[-1]
        request = youtube.playlistItems().insert(
            part='snippet',
            body={
                'snippet': {
                    'playlistId': playlist_id,
                    'resourceId': {
                        'kind': 'youtube#video',
                        'videoId': video_id
                    }
                }
            }
        )
        request_list.append(request)

    # Execute requests with a small delay
    for request in request_list:
        request.execute()
        time.sleep(0.1)  # Adjust sleep time based on the number of videos

    print(f'Inserted {len(videos)} videos into playlist {playlist_id}.')


# Rest of your code remains unchanged




def countdown_timer(seconds):
    for i in range(seconds, 0, -1):
        print(f'Waiting... {i}s', end='\r')
        time.sleep(1)
    print('')


if __name__ == '__main__':
    # Set the OAuth 2.0 client secrets file path
    CLIENT_SECRETS_FILE = 'client_secret.json'

    # Set the existing playlist ID
    existing_playlist_id = input(
        'Enter the existing YouTube playlist ID: ')

    # Get authenticated YouTube API service
    youtube_service = get_authenticated_service()

    # Countdown before fetching playlist videos
    countdown_timer(5)

    # Call the get_all_playlist_videos function to get all videos from the existing playlist
    existing_playlist_videos = get_all_playlist_videos(
        youtube_service, existing_playlist_id)

    if existing_playlist_videos:
        # Ask the user how many new playlists they want to generate
        num_new_playlists = int(input(
            'Enter the number of new playlists to generate: '))

        # Calculate the number of videos in each new playlist
        videos_per_playlist = len(existing_playlist_videos) // num_new_playlists

        # Get the name of the existing playlist
        existing_playlist_info = youtube_service.playlists().list(
            part='snippet',
            id=existing_playlist_id
        ).execute()

        existing_playlist_title = existing_playlist_info['items'][0]['snippet']['title']

        for i in range(num_new_playlists):
            # Calculate the start and end indices for videos in the current new playlist
            start_index = i * videos_per_playlist
            end_index = start_index + videos_per_playlist

            # Create a new playlist
            new_playlist_id = create_playlist(
                youtube_service, f'{existing_playlist_title} - Playlist {i + 1}', f'Generated Playlist {i + 1}')

            if new_playlist_id:
                # Countdown before adding videos to the new playlist
                countdown_timer(10)

                # Add videos to the new playlist maintaining original order
                add_videos_to_playlist(
                    youtube_service, new_playlist_id, existing_playlist_videos[start_index:end_index])

                print(
                    f'New playlist {i + 1} created successfully!\nPlaylist ID: {new_playlist_id}')
            else:
                print(f'Failed to create new playlist {i + 1}.')
    else:
        print("Unable to fetch existing playlist videos.")
