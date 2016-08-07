using System;
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
    public class SearchResultsFragment: Android.Support.V4.App.Fragment
    {
        private const string LOG_TAG = "SEARCH_RESULTS_FRAGMENT";
        private IEnumerable<FiveMinVideo> _results;
        private IEnumerable<FiveMinVideo> _allVideos;
        private View _view;
        private SearchView _searchView;
        private ListView _resultsListView;
        private string _query;

        public SearchResultsFragment (string query = "")
        {
            RetainInstance = true;
            _query = query;
            _results = new List<FiveMinVideo> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            _view = inflater.Inflate (Resource.Layout.search_results_fragment, null);
            _searchView = _view.FindViewById<SearchView> (Resource.Id.action_search);

            LoadDataToGridAsync (_view);

            if (!string.IsNullOrEmpty (_query))
            {
                _results = _allVideos.Where (v => v.Name.ToLower ().Contains (_query) || v.Keywords.Contains (_query.ToLower ()));
                ResetAdapter ();
            }

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
                await GetSearchResultsAsync ();

                if (_results != null && _results.Any ())
                {
                    var textView = view.FindViewById<TextView> (Resource.Id.noVideosFoundMessage);
                    textView.Visibility = ViewStates.Invisible;
                }
                else
                {
                    Snackbar.Make (_view, "No videos found", Snackbar.LengthShort).Show ();
                    return;
                }

                _resultsListView = view.FindViewById<ListView> (Resource.Id.searchResultsListView);
                _resultsListView.Visibility = ViewStates.Visible;

                var results = await FirebaseManager.Instance.GetAllVideos ();
                _allVideos = results.Values;
                _results = new List<FiveMinVideo> ();
                ResetAdapter ();
            }
            else
                Snackbar.Make (_view, Resource.String.no_internet_message, Snackbar.LengthLong).Show ();
        }

        void ResetAdapter ()
        {
            _resultsListView.Adapter = new VideosListAdapter (Activity, _results);

            _resultsListView.ItemClick += (sender, e) =>
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

        private async Task GetSearchResultsAsync ()
        {
            Log.Debug (LOG_TAG, "Fetching search results");
            var trending = await FirebaseManager.Instance.GetTrendingVideos ();

            _results = trending.Values;
        }
    }
}

