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
    public class EndingSoonFragment: Android.Support.V4.App.Fragment
    {
        private const string LOG_TAG = "ENDING_COMPETITIONS_FRAGMENT";
        private IEnumerable<FiveMinVideo> _endingSoonCompetitions;
        private View _view;

        public EndingSoonFragment ()
        {
            RetainInstance = true;
            _endingSoonCompetitions = new List<FiveMinVideo> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            _view = inflater.Inflate (Resource.Layout.ending_soon_fragment, null);

            LoadDataToGridAsync ();

            return _view;
        }

        private async void LoadDataToGridAsync ()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await GetEndingCompetitionsAsync ();

                if (_endingSoonCompetitions != null && _endingSoonCompetitions.Any ())
                {
                    // Hide the text view
                    var textView = _view.FindViewById<TextView> (Resource.Id.noEndingSoonCompetitionsMessage);
                    textView.Visibility = ViewStates.Invisible;
                }
                else
                {
                    return;
                }

                var competitionsListView = _view.FindViewById<ListView> (Resource.Id.endingSoonCompetitionsListView);
                competitionsListView.Visibility = ViewStates.Visible;
                competitionsListView.Adapter = new VideosListAdapter (Activity, _endingSoonCompetitions);

                competitionsListView.ItemClick += (sender, e) =>
                {
                    var index = e.Position;

                    var lv = (sender as ListView);

                    if (lv != null)
                    {
                        var competitionListAdapter = lv.Adapter as VideosListAdapter;
                        var competition = competitionListAdapter?.Videos [index];

                        if (competition != null)
                        {
                            // Put the name of the selected category into the intent
                            var fragment = new VideoPageFragment { Competition = competition };

                            Activity.SupportFragmentManager.BeginTransaction ()
                                .Replace (Resource.Id.content_frame, fragment)
                                .AddToBackStack (fragment.Tag)
                            .Commit ();
                        }
                    }
                };
            }
            else
                Snackbar.Make (_view, Resource.String.no_internet_message, Snackbar.LengthLong).Show();
        }

        private async Task GetEndingCompetitionsAsync ()
        {
            Log.Debug (LOG_TAG, "Fetching ending soon competitions");
            var ending = await FirebaseManager.Instance.GetEndingSoonCompetitions ();

            if (ending != null)
            {
                _endingSoonCompetitions = ending.Values;
            }
        }
    }
}

