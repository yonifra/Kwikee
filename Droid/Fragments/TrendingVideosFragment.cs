using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Android.Support.Design.Widget;
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
    public class TrendingVideosFragment : Android.Support.V4.App.Fragment
    {
        private const string LOG_TAG = "TRENDING_VIDEOS_FRAGMENT";
        private IEnumerable<FiveMinVideo> _trendingVideos;
        private View _view;

        public TrendingVideosFragment ()
        {
            RetainInstance = true;
            _trendingVideos = new List<FiveMinVideo> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            _view = inflater.Inflate (Resource.Layout.trending_fragment, null);

            LoadDataToGridAsync (_view);

            return _view;
        }

        public override void OnAttach (Android.Content.Context context)
        {
            base.OnAttach (context);
            var toolbar = (context as BaseActivity)?.Toolbar;

            toolbar?.SetTitle (Resource.String.fragment_title_trending);
        }

        private async void LoadDataToGridAsync (View view)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await GetTrendingVideosAsync ();

                if (_trendingVideos != null && _trendingVideos.Any ())
                {
                    var textView = view.FindViewById<TextView> (Resource.Id.noTrendingVideosMessage);
                    textView.Visibility = ViewStates.Invisible;
                }
                else
                {
                    Snackbar.Make (_view, "No trending videos found", Snackbar.LengthShort).Show ();
                    return;
                }

                var videosListView = view.FindViewById<ListView> (Resource.Id.trendingVideosListView);
                videosListView.Visibility = ViewStates.Visible;
                videosListView.Adapter = new VideosListAdapter (Activity, _trendingVideos);

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
                            VideoHelper.StartVideo (video, _view);
                        }
                    }
                };
            }
            else
                Snackbar.Make (_view, Resource.String.no_internet_message, Snackbar.LengthLong).Show ();
        }

        private async Task GetTrendingVideosAsync ()
        {
            Log.Debug (LOG_TAG, "Fetching trending videos");
            var trending = await FirebaseManager.Instance.GetTrendingVideos ();

            _trendingVideos = trending.Values;
        }
    }
}

