using Android.Content;
using Android.Views;
using FiveMin.Droid.Activities;
using FiveMin.Portable.Data;
using FiveMin.Portable.Entities;
using FiveMin.Portable.Helpers;

namespace FiveMin.Droid.Helpers
{
    public static class VideoHelper
    {
        public static void StartVideo (FiveMinVideo video, View view)
        {
            FirebaseManager.Instance.UpdateWatchCount (video);

            // Put the name of the selected category into the intent
            var intent = new Intent (view.Context, typeof (VideoActivity));
            intent.PutExtra ("VideoId", video.VideoId);
            intent.PutExtra ("VideoName", video.Name);
            intent.PutExtra ("VideoDescription", video.Description);
            intent.PutExtra ("WatchCount", video.WatchCount.ToString ("N0"));
            intent.PutExtra ("LikesDiff", (video.Likes - video.Dislikes).ToString ("N0"));
            intent.PutExtra ("Length", StringHelper.TimeSpanFormatter (video.Length));
            view.Context.StartActivity (intent);
        }
    }
}

