using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
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
    public class SearchResultsFragment : Android.Support.V4.App.Fragment
    {
        private const string LOG_TAG = "SEARCH_RESULTS_FRAGMENT";
        private IEnumerable<FiveMinVideo> _results;
        private View _view;
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

            if (!string.IsNullOrEmpty (_query) && CrossConnectivity.Current.IsConnected)
            {
                _results = FirebaseManager.Instance.AllVideos.Values.Where (v => v.Name.ToLower ().Contains (_query) ||
                                             v.Keywords.Contains (_query.ToLower ()) ||
                                             v.Categories.Any (c => c.ToLower ().Contains (_query.ToLower ())));

                CheckForResultsAndUpdateUi ();
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

        private void CheckForResultsAndUpdateUi ()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_results != null && _results.Any ())
                {
                    var textView = _view.FindViewById<TextView> (Resource.Id.noVideosFoundMessage);
                    textView.Visibility = ViewStates.Invisible;
                }
                else
                {
                    Snackbar.Make (_view, "No results found", Snackbar.LengthShort).Show ();
                    return;
                }

                _resultsListView = _view.FindViewById<ListView> (Resource.Id.searchResultsListView);
                _resultsListView.Visibility = ViewStates.Visible;
            }
            else
                Snackbar.Make (_view, Resource.String.no_internet_message, Snackbar.LengthLong).Show ();
        }

        void ResetAdapter ()
        {
            if (_resultsListView == null) return;

            _resultsListView.ItemClick -= VideoItemClick;

            if (_resultsListView.Adapter != null && !_resultsListView.Adapter.IsEmpty)
            {
                _resultsListView.Adapter.Dispose ();
            }

            _resultsListView.Adapter = new VideosListAdapter (Activity, _results);

            _resultsListView.ItemClick += VideoItemClick;

        }

        void VideoItemClick (object sender, AdapterView.ItemClickEventArgs e)
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

