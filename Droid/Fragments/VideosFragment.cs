using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using FiveMin.Droid.Activities;
using FiveMin.Droid.Adapters;
using FiveMin.Portable.Data;
using FiveMin.Portable.Entities;
using Plugin.Connectivity;

namespace FiveMin.Droid.Fragments
{
    public class VideosFragment : Android.Support.V4.App.Fragment
    {
        private const string LOG_TAG = "VIDEOS_FRAGMENT";
        private IEnumerable<FiveMinVideo> _videos;
        private View _view;

        public VideosFragment ()
        {
            RetainInstance = true;
            _videos = new List<FiveMinVideo> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            _view = inflater.Inflate (Resource.Layout.fragment_videos, null);

            LoadDataToGridAsync ();

            return _view;
        }

        private async void LoadDataToGridAsync ()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await GetAllVideosAsync (SelectedCategoryName);

                var videosListView = _view.FindViewById<ListView> (Resource.Id.videosListView);
                videosListView.Adapter = new VideosListAdapter (Activity, _videos);

                videosListView.ItemClick += (sender, e) =>
                {
                    var index = e.Position;

                    var lv = (sender as ListView);

                    if (lv != null)
                    {
                        var videosListAdapter = lv.Adapter as VideosListAdapter;
                        var video = videosListAdapter?.Videos [index];

                        if (video != null)
                        {
                            FirebaseManager.Instance.UpdateWatchCount (video);

                            // Put the name of the selected category into the intent
                            var intent = new Intent (_view.Context, typeof (VideoActivity));
                            intent.PutExtra ("VideoId", video.VideoId);
                            intent.PutExtra ("VideoName", video.Name);
                            intent.PutExtra ("VideoDescription", video.Description);
                            StartActivity (intent);
                            //var fragment = new VideoPageFragment { Video = video };

                            //Activity.SupportFragmentManager.BeginTransaction ()
                            //    .Replace (Resource.Id.content_frame, fragment)
                            //    .AddToBackStack (fragment.Tag)
                            //    .Commit ();
                        }
                    }
                };
            }
            else
                Snackbar.Make (_view, Resource.String.no_internet_message, Snackbar.LengthLong).Show();
        }

        private async Task GetAllVideosAsync (string categoryName)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (string.IsNullOrEmpty (categoryName))
                {
                    Log.Debug (LOG_TAG, "Could not resolve category name");
                    var all = await FirebaseManager.Instance.GetAllVideos ();
                    _videos = all.Values;
                }
                else
                {
                    Log.Debug (LOG_TAG, "Found category " + categoryName);
                    _videos = await FirebaseManager.Instance.GetVideos (categoryName);
                }
            }
            else
                Snackbar.Make (_view, Resource.String.no_internet_message, Snackbar.LengthLong).Show ();
        }

        public string SelectedCategoryName { get; set; }
    }
}

