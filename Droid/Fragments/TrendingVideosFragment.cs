using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using FiveMin.Droid.Adapters;
using FiveMin.Portable.Data;
using FiveMin.Portable.Entities;
using Plugin.Connectivity;

namespace FiveMin.Droid.Fragments
{
    public class TrendingVideosFragment : Android.Support.V4.App.Fragment
    {
        private const string LOG_TAG = "TRENDING_VIDEOS_FRAGMENT";
        private IEnumerable<FiveMinVideo> _trendingCompetitions;
        private View _view;

        public TrendingVideosFragment ()
        {
            RetainInstance = true;
            _trendingCompetitions = new List<FiveMinVideo> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            _view = inflater.Inflate (Resource.Layout.trending_fragment, null);

            LoadDataToGridAsync (_view);

            return _view;
        }

        private async void LoadDataToGridAsync (View view)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await GetTrendingCompetitionsAsync ();

                if (_trendingCompetitions != null && _trendingCompetitions.Any ())
                {
                    var textView = view.FindViewById<TextView> (Resource.Id.noTrendingCompetitionsMessage);
                    textView.Visibility = ViewStates.Invisible;
                }
                else
                {
                    Snackbar.Make (_view, "No trending competitions found", Snackbar.LengthShort).Show ();
                    return;
                }

                var competitionsListView = view.FindViewById<ListView> (Resource.Id.trendingCompetitionsListView);
                competitionsListView.Visibility = ViewStates.Visible;
                competitionsListView.Adapter = new VideosListAdapter (Activity, _trendingCompetitions);

                competitionsListView.ItemClick += (sender, e) =>
                {
                    var index = e.Position;

                    var lv = (sender as ListView);

                    if (lv != null)
                    {
                        var videosListAdapter = lv.Adapter as VideosListAdapter;
                        var video = videosListAdapter?.Videos [index];

                        if (video != null)
                        {
                            // Put the name of the selected category into the intent
                            var fragment = new VideoPageFragment { Competition = video };

                            Activity.SupportFragmentManager.BeginTransaction ()
                                .Replace (Resource.Id.content_frame, fragment)
                                .AddToBackStack (fragment.Tag)
                                .Commit ();
                        }
                    }
                    else
                    {
                        Snackbar.Make (_view, "Item " + e.Position + " clicked", Snackbar.LengthShort).Show ();
                    }
                };
            }
            else
                Snackbar.Make (_view, Resource.String.no_internet_message, Snackbar.LengthLong).Show();
        }

        private async Task GetTrendingCompetitionsAsync ()
        {
                Log.Debug (LOG_TAG, "Fetching trending competitions");
            var trending = await FirebaseManager.Instance.GetTrendingCompetitions ();

            _trendingCompetitions = trending.Values;
        }
    }
}

