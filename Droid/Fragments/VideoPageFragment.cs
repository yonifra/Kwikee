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

namespace FiveMin.Droid.Fragments
{
    public class VideoPageFragment : Android.Support.V4.App.Fragment
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
                var videoView = view.FindViewById<WebView> (Resource.Id.videoView);

                videoNameTv.Text = video.Name;
                videoDescTv.Text = video.Description;

                if (!string.IsNullOrEmpty (video.VideoUrl))
                {
                    var metrics = Resources.DisplayMetrics;
                    //fix video screen height and width
                    _displayWidth = (ConvertPixelsToDp (metrics.WidthPixels) + 200);
                    _displayHeight = (ConvertPixelsToDp (metrics.HeightPixels)) / (2);
                    PlayInWebView (view, video.VideoUrl);
                }

                FontsHelper.ApplyTypeface (view.Context.Assets, new List<TextView> { videoNameTv, videoDescTv });

            }
        }

        void PlayInWebView (View v, string videoUrl)
        {

            string id = FnGetVideoID (videoUrl);

            if (!string.IsNullOrEmpty (id))
            {
                videoUrl = string.Format ("http://youtube.com/embed/{0}", id);
            }
            else
            {
                Snackbar.Make (v, "Video url is not in correct format", Snackbar.LengthLong).Show ();
                return;
            }

            string html = @"<html><body><iframe width=""videoWidth"" height=""videoHeight"" src=""strUrl""></iframe></body></html>";
            var myWebView = v.FindViewById<WebView> (Resource.Id.videoView);
            var settings = myWebView.Settings;
            settings.JavaScriptEnabled = true;
            settings.UseWideViewPort = true;
            settings.LoadWithOverviewMode = true;
            settings.JavaScriptCanOpenWindowsAutomatically = true;
            settings.DomStorageEnabled = true;
            settings.SetRenderPriority (WebSettings.RenderPriority.High);
            settings.BuiltInZoomControls = false;

            settings.JavaScriptCanOpenWindowsAutomatically = true;
            myWebView.SetWebChromeClient (new WebChromeClient ());
            settings.AllowFileAccess = true;
            settings.SetPluginState (WebSettings.PluginState.On);
            string strYouTubeURL = html.Replace ("videoWidth", _displayWidth.ToString ()).Replace ("videoHeight", _displayHeight.ToString ()).Replace ("strUrl", videoUrl);

            myWebView.LoadDataWithBaseURL (null, strYouTubeURL, "text/html", "UTF-8", null);

        }

        int ConvertPixelsToDp (float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        static string FnGetVideoID (string strVideoURL)
        {
            const string regExpPattern = @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)";
            //for Vimeo: vimeo\.com/(?:.*#|.*/videos/)?([0-9]+)
            var regEx = new Regex (regExpPattern);
            var match = regEx.Match (strVideoURL);
            return match.Success ? match.Groups [1].Value : null;
        }

        public FiveMinVideo Video { get { return _video; } set{ _video = value;} }
    }
}

