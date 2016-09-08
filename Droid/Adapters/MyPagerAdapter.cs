using Android.Support.V4.App;
using Kwikee.Droid.ProfileFragments;

namespace Kwikee.Droid.Adapters
{
    public class MyPagerAdapter : FragmentPagerAdapter
    {
        private static int NUM_ITEMS = 5;

        public override int Count
        {
            get
            {
                // Total number of pages
                return NUM_ITEMS;
            }
        }

        public MyPagerAdapter (FragmentManager fragmentManager)
            :base(fragmentManager)
        {
        }

        // Returns the fragment to display for that page
        public override Fragment GetItem (int position)
        {
            switch (position)
            {
            case 0: // Fragment # 0 - This will show FirstFragment
                return ProfileFragment.NewInstance (0, "Page # 1");
            case 1: // Fragment # 0 - This will show FirstFragment different title
                return ProfileFragment.NewInstance (1, "Page # 2");
            case 2: // Fragment # 1 - This will show SecondFragment
                return ProfileFragment.NewInstance (2, "Page # 3");
            case 3: // Fragment # 0 - This will show FirstFragment different title
                return ProfileFragment.NewInstance (3, "Page # 4");
            case 4: // Fragment # 1 - This will show SecondFragment
                return ProfileFragment.NewInstance (4, "Page # 5");
            default:
                return null;
            }
        }

        // Returns the page title for the top indicator
        //public override Java.Lang.ICharSequence GetPageTitleFormatted (int position)
        //{
        //    return ("Page " + position).ToCharArray();
        //}
    }
}