﻿using Android.Content;
using Android.Views;
using Kwikee.Droid.Activities;
using Kwikee.Portable.Data;
using Kwikee.Portable.Entities;
using Kwikee.Portable.Helpers;

namespace Kwikee.Droid.Helpers
{
    public static class VideoHelper
    {
        /// <summary>
        /// Given a video object, starts an activity with the video ready to be played
        /// </summary>
        /// <param name="video">The video to play.</param>
        /// <param name="view">The view from which this is called.</param>
        public static void StartVideo (FiveMinVideo video, View view)
        {
            FirebaseManager.Instance.UpdateWatchCount (video);

            // Put the name of the selected category into the intent
            var intent = new Intent (view.Context, typeof (VideoActivity));
            intent.PutExtra ("VideoId", video.VideoId);
            intent.PutExtra ("VideoKey", video.Key);
            intent.PutExtra ("VideoName", video.Name);
            intent.PutExtra ("VideoDescription", video.Description);
            intent.PutExtra ("WatchCount", video.WatchCount.ToString ("N0"));
            intent.PutExtra ("LikesDiff", (video.Likes - video.Dislikes).ToString ("N0"));
            intent.PutExtra ("Length", StringHelper.TimeSpanFormatter (video.Length));
            view.Context.StartActivity (intent);
        }
    }
}

