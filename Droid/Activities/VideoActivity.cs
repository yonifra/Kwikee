using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Google.YouTube.Player;
using Kwikee.Droid.Helpers;
using Kwikee.Portable.Enums;

namespace Kwikee.Droid.Activities
{
    [Activity (Label = "Video")]
    public class VideoActivity : YouTubeFailureRecoveryActivity
    {
        string _videoYouTubeId;
        string _videoKey;  // The UID of the video (not YouTube ID)
        LinearLayout _mainLayout;
        bool _isWatchlistClicked;
        bool _isFavoriteClicked;
        int _likesDiff;
        string _length;
        string _watchCount;
        TextView _metadata;

        protected override IYouTubePlayerProvider GetYouTubePlayerProvider ()
        {
            return FragmentManager.FindFragmentById<YouTubePlayerFragment> (Resource.Id.youtube_fragment);
        }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.fragment_videoPage);

            _videoYouTubeId = Intent.GetStringExtra ("VideoId");
            _videoKey = Intent.GetStringExtra ("VideoKey");
            var videoName = Intent.GetStringExtra ("VideoName");
            var videoDescription = Intent.GetStringExtra ("VideoDescription");
            _watchCount = Intent.GetStringExtra ("WatchCount");
            _likesDiff = int.Parse (Intent.GetStringExtra ("LikesDiff"));
            _length = Intent.GetStringExtra ("Length");

            var videoNameTextView = FindViewById<TextView> (Resource.Id.videoNameTextView);
            _metadata = FindViewById<TextView> (Resource.Id.videoMetadataTextView);
            var descriptionTextView = FindViewById<TextView> (Resource.Id.videoDescriptionTextView);

            UpdateVideoStats ();
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

            SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Watched);

            FontsHelper.ApplyTypeface (Assets, new List<TextView> { _metadata, videoNameTextView, descriptionTextView });

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

        void Fab_Click (object sender, EventArgs e)
        {
            if (_isFavoriteClicked)
            {
                SharedPreferencesHelper.Instance.RemoveVideoFromSharedPreferences (_videoKey, SharedPreferenceType.Favorites);
                Snackbar.Make (_mainLayout, "Removed from favorites", Snackbar.LengthLong).Show ();
            }
            else
            {
                SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Favorites);
                Snackbar.Make (_mainLayout, "Added to favorites", Snackbar.LengthLong).Show ();
            }

            _isFavoriteClicked = !_isFavoriteClicked;
        }

        void WatchlistButton_Click (object sender, EventArgs e)
        {
            if (_isWatchlistClicked)
            {
                SharedPreferencesHelper.Instance.RemoveVideoFromSharedPreferences (_videoKey, SharedPreferenceType.Watchlist);
                Snackbar.Make (_mainLayout, "Removed from watchlist", Snackbar.LengthLong).Show ();
            }
            else
            {
                SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Watchlist);
                Snackbar.Make (_mainLayout, "Added to watchlist", Snackbar.LengthLong).Show ();
            }

            _isWatchlistClicked = !_isWatchlistClicked;
        }

        void Disliked_Click (object sender, EventArgs e)
        {
            if (SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Disliked))
            {
                SharedPreferencesHelper.Instance.RemoveVideoFromSharedPreferences (_videoKey, SharedPreferenceType.Liked);
                _likesDiff--;
                UpdateVideoStats ();
            }
        }

        void Liked_Click (object sender, EventArgs e)
        {
            if (SharedPreferencesHelper.Instance.AddVideoToSharedPreferences (_videoKey, SharedPreferenceType.Liked))
            {
                SharedPreferencesHelper.Instance.RemoveVideoFromSharedPreferences (_videoKey, SharedPreferenceType.Disliked);
                _likesDiff++;
                UpdateVideoStats ();
            }
        }

        private void UpdateVideoStats ()
        {
            _metadata.Text = _watchCount + " views, " + _likesDiff + " likesdiff, " + _length + " minutes";
        }
    }
}

