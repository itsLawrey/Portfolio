# Youtube playlist maker
>One of my side projects, there are multiple files here which make it possible via the YouTube API, to fetch an existing public playlist from youtube into a json file.

>You can then specify your own playlist to insert the songs into, keeping the original order.

>You can also specify the index range of the videos you need. This way you can insert only the songs in the middle of the playlist, for example

## Language
>These files are written in python.

## Issues
>I ran into several issues while developing the program. Mainly it is not very efficient with the number of API calls. I tried to fix it by inserting batches of videos at the same time, but quickly discovered that if a single video is not available from the batch, then the insertion of the entire batch fails.

>I also tried using more threads to speed up the insertion process, but then I wouldn't have been able to keep the original order of the videos, which was a must for me.

>I also have to wait after every insertion for at least half a second, because otherwise the insertion would be too fast for the youtube api to handle.

## Use
>You need to have a valid youtube API key, which you can simply get from Google. You also need a client-secret.json file, which is used for authentication purposes by google. I have not uploaded those for security reasons.

>In the ytp_final.py file, you can replace the placeholder strings with your own json file, and then run the program. Use json maker to get the videos from a playlist, then json inserter to insert into an existing one.
