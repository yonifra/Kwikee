using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using Square.Picasso;
using FiveMin.Droid.Helpers;
using FiveMin.Portable.Data;
using FiveMin.Portable.Entities;
using Android;
using Android.Webkit;
using Android.Views;
using System.Text.RegularExpressions;
using Google.YouTube.Player;

namespace FiveMin.Droid.Fragments
{
    public class VideoPageFragment : Android.Support.V4.App.Fragment, IYouTubePlayerProvider
    {
        private FiveMinVideo _video;
        int _displayWidth;
        int _displayHeight;

        public VideoPageFragment ()
        {
            RetainInstance = true;
        }

        private void UpdateUiForSelectedEntity (TextView videoName, TextView videoDescription)
        {
            videoName.Text = _video.Name;
            videoDescription.Text = _video.Description;
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            var view = inflater.Inflate (Resource.Layout.fragment_videoPage, null);
            PopulateDataAsync (view);

            return view;
        }

        Intent CreateShareIntent ()
        {
            var setShareIntent = new Intent (Intent.ActionSend);
            setShareIntent.SetType ("text/plain");
            setShareIntent.PutExtra (Intent.ExtraSubject, GetString (Resource.String.share_competition_message_subject));
            setShareIntent.PutExtra (Intent.ExtraText, "I just watched "+ _video.Name +". Wanna learn too? #FiveMin");

            return setShareIntent;
        }

        async void PopulateDataAsync (Android.Views.View view)
        {
            var video = await FirebaseManager.Instance.GetVideo (Video.Name);
          
            if (video != null)
            {
             //   FontsHelper.ApplyTypeface (view.Context.Assets, new List<TextView> { entityName, entityDescription, _leftVotesTextView, _rightVotesTextView });

                var progressBar = view.FindViewById<ProgressBar> (Resource.Id.loadingVideoProgressBar);
                var mainLayout = view.FindViewById<LinearLayout> (Resource.Id.mainVideoLayout);

                progressBar.Visibility = Android.Views.ViewStates.Gone;
                mainLayout.Visibility = Android.Views.ViewStates.Visible;

                var videoNameTv = view.FindViewById<TextView> (Resource.Id.videoNameTextView);
                var videoDescTv = view.FindViewById<TextView> (Resource.Id.videoDescriptionTextView);
             //   var videoView = view.FindViewById<WebView> (Resource.Id.videoView);

                videoNameTv.Text = video.Name;
                videoDescTv.Text = video.Description;
                // TODO: Remove this!
                if (!string.IsNullOrEmpty (video.VideoUrl))
                {
                    var metrics = Resources.DisplayMetrics;
                }

                FontsHelper.ApplyTypeface (view.Context.Assets, new List<TextView> { videoNameTv, videoDescTv });

            }
        }



        public void Initialize (string p0, IYouTubePlayerOnInitializedListener p1)
        {
            throw new NotImplementedException ();
        }

        public FiveMinVideo Video { get { return _video; } set{ _video = value;} }
    }
}

