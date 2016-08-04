using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using FiveMin.Droid.Activities;
using FiveMin.Droid.Adapters;
using FiveMin.Droid.Helpers;
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
        private SwipeRefreshLayout _refreshLayout;

        public VideosFragment ()
        {
            RetainInstance = true;
            _videos = new List<FiveMinVideo> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            _view = inflater.Inflate (Resource.Layout.fragment_videos, null);

            _refreshLayout = _view.FindViewById<SwipeRefreshLayout> (Resource.Id.swipeRefreshLayout);
            _refreshLayout.Refresh += (o, e) => { LoadDataToGridAsync (); };

            LoadDataToGridAsync ();

            return _view;
        }

        public override void OnAttach (Context context)
        {
            base.OnAttach (context);
            var toolbar = (context as BaseActivity)?.Toolbar;

            toolbar?.SetTitle (Resource.String.fragment_title_videos);
        }

        private async void LoadDataToGridAsync ()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await GetAllVideosAsync (SelectedCategoryName);

                var listView = _view.FindViewById<ListView> (Resource.Id.videosListView);

                listView.ItemClick -= OnListItemClick;

                listView.Adapter = new VideosListAdapter (Activity, _videos);

                listView.ItemClick += OnListItemClick;

                _refreshLayout.Refreshing = false;
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

        public string SelectedCategoryName { get;set; }

        void OnListItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
            var index = e.Position;

            var lv = (sender as ListView);

            if (lv != null)
            {
                var videosListAdapter = lv.Adapter as VideosListAdapter;
                var video = videosListAdapter?.Videos [index];

                if (video != null)
                {
                    VideoHelper.StartVideo (video, _view);
                }
            }
        }
        
    }
}

