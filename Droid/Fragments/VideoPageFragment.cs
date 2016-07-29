using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Widget;
using FiveMin.Droid.Helpers;
using FiveMin.Portable.Data;
using FiveMin.Portable.Entities;
using Android.Views;
using Google.YouTube.Player;

namespace FiveMin.Droid.Fragments
{
    public class VideoPageFragment : Android.Support.V4.App.Fragment, IYouTubePlayerProvider
    {
        private FiveMinVideo _video;

        public VideoPageFragment ()
        {
            RetainInstance = true;
        }

        private void UpdateUiForSelectedEntity (TextView videoName, TextView videoDescription)
        {
            videoName.Text = _video.Name;
            videoDescription.Text = _video.Description;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
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

        async void PopulateDataAsync (View view)
        {
            var video = await FirebaseManager.Instance.GetVideo (Video.Name);
          
            if (video != null)
            {
                var progressBar = view.FindViewById<ProgressBar> (Resource.Id.loadingVideoProgressBar);
                var mainLayout = view.FindViewById<LinearLayout> (Resource.Id.mainVideoLayout);
                var fab = view.FindViewById<FloatingActionButton> (Resource.Id.addToFavorite);

                progressBar.Visibility = Android.Views.ViewStates.Gone;
                mainLayout.Visibility = Android.Views.ViewStates.Visible;
                fab.Visibility = ViewStates.Visible;

                var videoNameTv = view.FindViewById<TextView> (Resource.Id.videoNameTextView);
                var videoDescTv = view.FindViewById<TextView> (Resource.Id.videoDescriptionTextView);          

                videoNameTv.Text = video.Name;
                videoDescTv.Text = video.Description;

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

