
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FiveMin.Droid.Helpers;
using Google.YouTube.Player;

namespace FiveMin.Droid.Activities
{
    [Activity (Label = "Video")]
    public class VideoActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.Main;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Create your application here

            SetContentView (Resource.Layout.fragment_videoPage);

            savedInstanceState.GetCharSequence ("VideoId");
            savedInstanceState.GetCharSequence ("VideoName");
            savedInstanceState.GetCharSequence ("VideoDescription");
            savedInstanceState.GetCharSequence ("WatchCount");
            savedInstanceState.GetCharSequence ("LikesDiff");
            savedInstanceState.GetCharSequence ("Length");

           // var player = FindViewById<YouTubePlayerFragment> (Resource.Id.youtube_fragment);
        }
    }
}

