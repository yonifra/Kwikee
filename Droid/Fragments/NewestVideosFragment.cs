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
    public class NewestVideosFragment : Android.Support.V4.App.Fragment
    {
        private const string LOG_TAG = "NEWEST_VIDEOS_FRAGMENT";
        private IEnumerable<FiveMinVideo> _newestVideos;
        private View _view;

        public NewestVideosFragment ()
        {
            RetainInstance = true;
            _newestVideos = new List<FiveMinVideo> ();
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            _view = inflater.Inflate (Resource.Layout.ending_soon_fragment, null);

            LoadDataToGridAsync ();

            return _view;
        }

        public override void OnAttach (Android.Content.Context context)
        {
            base.OnAttach (context);
            var toolbar = (context as BaseActivity)?.Toolbar;

            toolbar?.SetTitle (Resource.String.fragment_title_newest);
        }

        private async void LoadDataToGridAsync ()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await GetEndingVideosAsync ();

                if (_newestVideos != null && _newestVideos.Any ())
                {
                    // Hide the text view
                    var textView = _view.FindViewById<TextView> (Resource.Id.noVideosMessageTextView);
                    textView.Visibility = ViewStates.Invisible;
                }
                else
                {
                    return;
                }

                var videosListView = _view.FindViewById<ListView> (Resource.Id.newestVideosListView);
                videosListView.Visibility = ViewStates.Visible;
                videosListView.Adapter = new VideosListAdapter (Activity, _newestVideos);

                videosListView.ItemClick += (sender, e) =>
                {
                    var index = e.Position;

                    var lv = (sender as ListView);

                    if (lv != null)
                    {
                        var competitionListAdapter = lv.Adapter as VideosListAdapter;
                        var video = competitionListAdapter?.Videos [index];

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

        private async Task GetEndingVideosAsync ()
        {
            Log.Debug (LOG_TAG, "Fetching newest videos");
            var ending = await FirebaseManager.Instance.GetNewestVideos ();

            if (ending != null)
            {
                _newestVideos = ending.Values;
            }
        }
    }
}

