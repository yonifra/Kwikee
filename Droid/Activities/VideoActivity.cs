using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using FiveMin.Droid.Helpers;
using FiveMin.Portable.Enums;
using Google.YouTube.Player;

namespace FiveMin.Droid.Activities
{
    [Activity (Label = "Video")]
    public class VideoActivity : YouTubeFailureRecoveryActivity
    {
        private string _videoYouTubeId;
        private string _videoKey;  // The UID of the video (not YouTube ID)
        private LinearLayout _mainLayout;

        protected override IYouTubePlayerProvider GetYouTubePlayerProvider ()
        {
            return FragmentManager.FindFragmentById<YouTubePlayerFragment> (Resource.Id.youtube_fragment);
        }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.fragment_videoPage);

            _videoYouTubeId = Intent.GetStringExtra  ("VideoId");
            _videoKey = Intent.GetStringExtra ("VideoKey");
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

            _mainLayout = FindViewById<LinearLayout> (Resource.Id.mainVideoLayout);
            var progressBar = FindViewById<ProgressBar> (Resource.Id.loadingVideoProgressBar);
            var fab = FindViewById<FloatingActionButton> (Resource.Id.addToFavorite);
            var watchlistButton = FindViewById<Button> (Resource.Id.addToWatchListButton);
            var liked = FindViewById<Button> (Resource.Id.likeButton);
            var disliked = FindViewById<Button> (Resource.Id.dislikeButton);

            // Attach click handlers
            fab.Click += Fab_Click;
            watchlistButton.Click += WatchlistButton_Click;
            liked.Click += Liked_Click;
            disliked.Click += Disliked_Click;

            var youTubePlayerFragment = FragmentManager.FindFragmentById<YouTubePlayerFragment> (Resource.Id.youtube_fragment);
            youTubePlayerFragment.Initialize (DeveloperKey.Key, this);

           // FontsHelper.ApplyTypeface (Assets, new List<TextView> { videoMetadata, videoNameTextView, descriptionTextView });

            progressBar.Visibility = ViewStates.Invisible;
            _mainLayout.Visibility = ViewStates.Visible;
            fab.Visibility = ViewStates.Visible;
        }

        public override void OnInitializationSuccess (IYouTubePlayerProvider provider, IYouTubePlayer player, bool wasRestored)
        {
            if (!wasRestored)
            {
                player.CueVideo (_videoYouTubeId);
                SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Watched);
            }
        }

        void Fab_Click (object sender, System.EventArgs e)
        {
            SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Favorites);

            Snackbar.Make (_mainLayout, "Added to favorites", Snackbar.LengthLong).Show ();
        }

        void WatchlistButton_Click (object sender, System.EventArgs e)
        {
            SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Watchlist);

            Snackbar.Make (_mainLayout, "Added to watchlist", Snackbar.LengthLong).Show ();
        }

        void Disliked_Click (object sender, System.EventArgs e)
        {
            SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Disliked);
        }

        void Liked_Click (object sender, System.EventArgs e)
        {
            var all = SharedPreferencesHelper.Instance.GetAllVideos (SharedPreferenceType.Liked);

            if (!all.Contains (_videoKey))
            {
                SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Liked);
            }

        }
    }
}

