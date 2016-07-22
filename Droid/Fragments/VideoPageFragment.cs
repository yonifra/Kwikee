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

namespace FiveMin.Droid.Fragments
{
    public class VideoPageFragment : Android.Support.V4.App.Fragment
    {
        private FiveMinVideo _video;
        private TextView _leftVotesTextView;
        private TextView _rightVotesTextView;

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

            //var view = inflater.Inflate(Resource.Layout.fragment_categories, container, false);
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
            var parentView = view.FindViewById<FrameLayout> (Resource.Id.parentLayout);

            if (video != null)
            {
             //   FontsHelper.ApplyTypeface (view.Context.Assets, new List<TextView> { entityName, entityDescription, _leftVotesTextView, _rightVotesTextView });

                var progressBar = view.FindViewById<ProgressBar> (Resource.Id.loadingCompetitionProgressBar);
                var mainLayout = view.FindViewById<LinearLayout> (Resource.Id.mainCompetitionLayout);

                progressBar.Visibility = Android.Views.ViewStates.Gone;
                mainLayout.Visibility = Android.Views.ViewStates.Visible;
            }
        }

        public FiveMinVideo Video { get { return _video; } set{ _video = value;} }
    }
}

