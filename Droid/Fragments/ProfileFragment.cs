using Android.OS;
using Android.Views;
using FiveMin.Droid.Activities;

namespace FiveMin.Droid.Fragments
{
    public class ProfileFragment : Android.Support.V4.App.Fragment
    {
        public ProfileFragment ()
        {
            RetainInstance = true;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView (inflater, container, savedInstanceState);

            var view = inflater.Inflate (Resource.Layout.fragment_profile, null);

            return view;
        }

        public override void OnAttach (Android.Content.Context context)
        {
            base.OnAttach (context);
            //var toolbar = (context as BaseActivity)?.Toolbar;

            //toolbar?.SetTitle (Resource.String.fragment_title_my_profile);
        }
    }
}

