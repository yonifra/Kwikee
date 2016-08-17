using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using FiveMin.Droid.Helpers;
using Google.YouTube.Player;

namespace FiveMin.Droid.Activities
{
    [Activity (Label = "Video")]
    public class VideoActivity : YouTubeFailureRecoveryActivity
    {
        private string _videoId;
        protected override IYouTubePlayerProvider GetYouTubePlayerProvider ()
        {
            return FragmentManager.FindFragmentById<YouTubePlayerFragment> (Resource.Id.youtube_fragment);
        }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.fragment_videoPage);

            _videoId = Intent.GetStringExtra  ("VideoId");
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

            var youTubePlayerFragment = FragmentManager.FindFragmentById<YouTubePlayerFragment> (Resource.Id.youtube_fragment);
            youTubePlayerFragment.Initialize (DeveloperKey.Key, this);

            FontsHelper.ApplyTypeface (Assets, new List<TextView> { videoMetadata, videoNameTextView, descriptionTextView });

            progressBar.Visibility = ViewStates.Invisible;
            mainLayout.Visibility = ViewStates.Visible;
            fab.Visibility = ViewStates.Visible;
        }

        public override void OnInitializationSuccess (IYouTubePlayerProvider provider, IYouTubePlayer player, bool wasRestored)
        {
            if (!wasRestored)
            {
                player.CueVideo (_videoId);
            }
        }
    }
}

