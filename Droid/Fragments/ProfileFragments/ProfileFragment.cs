using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace Kwikee.Droid.ProfileFragments
{
    public class ProfileFragment : Fragment
    {
        // Store instance variables
        private String title;
        private int page;

        // newInstance constructor for creating fragment with arguments
        public static ProfileFragment NewInstance (int page, String title)
        {
            var fragmentFirst = new ProfileFragment ();
            Bundle args = new Bundle ();
            args.PutInt ("tabNum", page);
            args.PutString ("tabTitle", title);
            fragmentFirst.Arguments = args;
            return fragmentFirst;
        }

        // Store instance variables based on arguments passed
        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            if (savedInstanceState != null)
            {
                page = savedInstanceState.GetInt ("tabNum", 0);
                title = savedInstanceState.GetString ("tabTitle");
            }
            else
            {
                page = 0;
                title = "TEST";
            }
        }

        // Inflate the view for the fragment based on layout XML
        public override View OnCreateView (LayoutInflater inflater, ViewGroup container,
            Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.fragment_videos, container, false);
        //    var tvLabel = (TextView)view.FindViewById (Resource.Id.tvLabel);
        //    tvLabel.Text = (page + " -- " + title);
            return view;
        }
    }
}
