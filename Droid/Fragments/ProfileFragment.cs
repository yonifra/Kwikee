using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Kwikee.Droid.Adapters;
using Kwikee.Droid.Helpers;
using Kwikee.Portable.Enums;

namespace Kwikee.Droid.Fragments
{
    public class ProfileFragment : Fragment
    {
        private TextView _liked;
        private TextView _watchlist;
        private TextView _disliked;
        private TextView _favs;
        private TextView _watched;
        private View _mainView;
        private MyPagerAdapter _adapterViewPager;

        public ProfileFragment ()
        {
            RetainInstance = true;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView (inflater, container, savedInstanceState);

            _mainView = inflater.Inflate (Resource.Layout.fragment_profile, null);

            var profileImage = _mainView.FindViewById<ImageView> (Resource.Id.profile_image);
            var profileToolbar = _mainView.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.profile_toolbar);

            profileImage.SetImageResource (Resource.Drawable.video);
            profileToolbar.Title = Resources.GetString (Resource.String.profile_page_header);

            FetchViews ();
            UpdateData ();

            var vpPager = (ViewPager)_mainView.FindViewById (Resource.Id.viewpager);
            _adapterViewPager = new MyPagerAdapter (Activity.SupportFragmentManager);
            vpPager.Adapter = _adapterViewPager;

            return _mainView;
        }

        void UpdateData ()
        {
      //      _liked.Text = SharedPreferencesHelper.Instance.GetAllVideos (SharedPreferenceType.Liked).Count.ToString ();
       //     _watchlist.Text = SharedPreferencesHelper.Instance.GetAllVideos (SharedPreferenceType.Watchlist).Count.ToString ();
       //     _disliked.Text = SharedPreferencesHelper.Instance.GetAllVideos (SharedPreferenceType.Disliked).Count.ToString ();
       //     _favs.Text = SharedPreferencesHelper.Instance.GetAllVideos (SharedPreferenceType.Favorites).Count.ToString ();
       //     _watched.Text = SharedPreferencesHelper.Instance.GetAllVideos (SharedPreferenceType.Watched).Count.ToString ();
        }

        public override void OnResume ()
        {
            base.OnResume ();
            UpdateData ();
        }

        void FetchViews ()
        {
            if (_mainView != null)
            {
                //_disliked = _mainView.FindViewById<TextView> (Resource.Id.dislikedTextView);
                //_liked = _mainView.FindViewById<TextView> (Resource.Id.likedTextView);
                //_watchlist = _mainView.FindViewById<TextView> (Resource.Id.watchListTextView);
                //_favs = _mainView.FindViewById<TextView> (Resource.Id.favTextView);
                //_watched = _mainView.FindViewById<TextView> (Resource.Id.watchedTextView);
            }
        }

        public override void OnAttach (Android.Content.Context context)
        {
            base.OnAttach (context);
            //var toolbar = (context as BaseActivity)?.Toolbar;

            //toolbar?.SetTitle (Resource.String.fragment_title_my_profile);
        }
    }
}

