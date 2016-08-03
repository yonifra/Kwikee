using Android.OS;
using Android.Views;

namespace FiveMin.Droid.Fragments
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
    }
}

