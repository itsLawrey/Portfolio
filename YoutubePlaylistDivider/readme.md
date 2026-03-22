# Youtube Playlist Divider

A Python toolset designed to manage, download, and divide massive YouTube playlists using the official YouTube Data API v3. 

I made this because I had a huge playlist of songs and wanted to better compartmentalize them, but doing so by hand was far too annoying and time-consuming. This tool ensures that videos keep their original order during transfers and gracefully skips over "faulty" (deleted/private) videos.

## ✨ Features
- **Fetch & Cache**: Backup any public playlist to a local JSON file, automatically flagging unavailable/deleted videos.
- **Chunk Insertion**: Insert a specific range (index) of videos from a JSON cache into a brand new playlist.
- **Playlist Splitting**: Automatically split a very large playlist directly into two halves.
- **Smart Resume**: Built-in state saving! If the YouTube API kicks you out (e.g. daily quota reached), the tool intelligently saves its progress. Next time you run it, it natively detects the interruption and asks to seamlessly resume.
- **Fully Interactive CLI**: The scripts feature smart terminal prompts. You can also bypass prompts completely by passing variables directly via command-line arguments.

---

## ⚙️ Setup & Prerequisites

1. **Python Environment**:
   Ensure you have the required Google API libraries installed:
   ```bash
   pip install google-auth-oauthlib google-api-python-client
   ```

2. **Google Cloud Credentials**:
   - Go to the [Google Cloud Console](https://console.cloud.google.com/) and enable the **YouTube Data API v3**.
   - Create an **OAuth 2.0 Client ID** (Desktop App).
   - Download the JSON credentials file, rename it to EXACTLY `client_secret.json`, and place it in this project folder.

---

## 🚀 Usage Guide

### 1. Caching a Playlist (`ytp_json_maker.py`)
Use this to extract all video data from a source playlist into a JSON file.
- **Interactive**: Run `python ytp_json_maker.py` and it will ask you for a Playlist ID.
- **CLI Mode**: `python ytp_json_maker.py -p <PLAYLIST_ID>`

### 2. Inserting a Chunk of Videos (`ytp_final.py`)
Use this to push a specific range of cached videos into a target playlist.
- **Interactive**: Just run `python ytp_final.py`. The setup wizard will sequentially ask for your destination Playlist ID, the JSON Cache file path, and exactly what start/end indices you want to insert.
- **CLI Mode**: `python ytp_final.py -d <DEST_ID> -c cache.json -s 0 -e 190`
- **Smart Resume**: If you hit an API limit, simply don't do anything! It creates `resume_state.json`. The next day, just run `python ytp_final.py` and press `Y` to automatically load all settings and resume inserting exactly where it crashed.

### 3. Splitting an Entire Playlist in Half (`ytp_divider.py`)
An interactive terminal script that prompts you for a playlist ID, automatically creates two new playlists ("First Half" and "Second Half"), and populates them.
- Run `python ytp_divider.py` and follow the terminal prompts.

---

## ⚠️ Important Limitations & API Quotas

- **Rate Limiting**: The scripts feature intentional `time.sleep()` delays between insertions (around 0.2s - 0.3s). This is necessary because the YouTube API cannot handle rapid-fire requests and will return errors if you go too fast.
- **Multithreading**: Multi-threading was attempted to speed up the process, but keeping the exact original order of videos is a priority, which multithreading breaks.
- **Daily Quota Limits**: The YouTube Data API provides a default limit of **10,000 units per day**.
  - Inserting a single video costs **50 units**.
  - **This means you can only insert ~200 videos per day** (closer to ~190 because fetching playlists and listing items also costs a few units).
  - *Mitigation*: Just let the **Smart Resume** feature handle it! If you get a `403 Quota Exceeded` error, look at the saved state message, and wait until midnight Pacific Time. When you run the script tomorrow, hit `Y` to resume!