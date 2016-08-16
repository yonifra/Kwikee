
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using FiveMin.Droid.Helpers;
using FiveMin.Portable.Helpers;
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

            SetContentView (Resource.Layout.fragment_videoPage);

            var videoId = Intent.GetStringExtra  ("VideoId");
            var videoName = Intent.GetStringExtra  ("VideoName");
            var videoDescription = Intent.GetStringExtra  ("VideoDescription");
            var watchCount = Intent.GetStringExtra  ("WatchCount");
            var likesDiff = Intent.GetStringExtra  ("LikesDiff");
            var length = Intent.GetStringExtra  ("Length");

            var videoNameTextView = FindViewById<TextView> (Resource.Id.videoNameTextView);
            var videoMetadata = FindViewById<TextView> (Resource.Id.videoMetadataTextView);
            var descriptionTextView = FindViewById<TextView> (Resource.Id.videoDescriptionTextView);

            videoMetadata.Text = watchCount + " views, " + likesDiff + " likesdiff, " + length + " minutes";
            videoNameTextView.Text = videoName;
            descriptionTextView.Text = videoDescription;

            var mainLayout = FindViewById<LinearLayout> (Resource.Id.mainVideoLayout);
            var progressBar = FindViewById<ProgressBar> (Resource.Id.loadingVideoProgressBar);
            var fab = FindViewById<FloatingActionButton> (Resource.Id.addToFavorite);

            FontsHelper.ApplyTypeface (Assets, new List<TextView> { videoMetadata, videoNameTextView, descriptionTextView });

            progressBar.Visibility = ViewStates.Invisible;
            mainLayout.Visibility = ViewStates.Visible;
            fab.Visibility = ViewStates.Visible;
        }
    }
}

