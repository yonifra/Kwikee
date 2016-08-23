using Android.OS;
using Android.Views;
using Kwikee.Droid.Activities;

namespace Kwikee.Droid.Fragments
{
    public class AboutFragment : Android.Support.V4.App.Fragment
    {
        public AboutFragment ()
        {
            RetainInstance = true;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView (inflater, container, savedInstanceState);

            var view = inflater.Inflate (Resource.Layout.about_fragment, null);

            return view;
        }

        public override void OnAttach (Android.Content.Context context)
        {
            base.OnAttach (context);
            var toolbar = (context as BaseActivity)?.Toolbar;

            toolbar?.SetTitle (Resource.String.fragment_title_about);
        }
    }
}

