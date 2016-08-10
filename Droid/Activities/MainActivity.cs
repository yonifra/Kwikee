using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using FiveMin.Droid.Fragments;

namespace FiveMin.Droid.Activities
{
    [Activity (Label = "Kwikee")]
    public class MainActivity : BaseActivity
    {
        private NavigationView _navigationView;
        private DrawerLayout _drawerLayout;
        private Toolbar _toolbar;
        private SearchView _searchView;
        private Android.Support.V4.App.Fragment _prevFragment;

        protected override int LayoutResource => Resource.Layout.Main;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Set our view from the "main" layout resource
            _navigationView = FindViewById<NavigationView> (Resource.Id.nav_view);
            _drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
            _toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);

            _navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked (true);

                ListItemClicked (e.MenuItem.ItemId);
            };

            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                ListItemClicked (0);
            }
        }

        public override bool OnPrepareOptionsMenu (IMenu menu)
        {
            menu.Clear ();

            // Check if the toolbar is initialized, and if so, inflate the menu onto it
            if (_toolbar != null)
            {
                MenuInflater.Inflate (Resource.Menu.menu, menu);

                // Locate MenuItem with ShareActionProvider
                var item = menu.FindItem (Resource.Id.menu_item_share);

                // Fetch and store ShareActionProvider
                if (item != null)
                {
                    item.SetIntent (CreateShareIntent ());
                }

                var searchItem = menu.FindItem (Resource.Id.action_search);
                _searchView = (SearchView)MenuItemCompat.GetActionView (searchItem);

                _searchView.QueryTextChange += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty (e.NewText))
                    {
                        // User has inputted some search text
                        SupportFragmentManager.BeginTransaction ()
                                                  .Replace (Resource.Id.content_frame, new SearchResultsFragment (e.NewText))
                                                  .Commit ();
                    }
                    else
                    {
                        // No search string, so display the previous fragment
                        SupportFragmentManager.BeginTransaction ()
                                              .Replace (Resource.Id.content_frame, _prevFragment)
                                              .Commit ();
                    }
                };
            }

            return base.OnPrepareOptionsMenu (menu);
        }

        Intent CreateShareIntent ()
        {
            var setShareIntent = new Intent (Intent.ActionSend);
            setShareIntent.SetType ("text/plain");
            setShareIntent.PutExtra (Intent.ExtraSubject, GetString (Resource.String.share_global_message_subject));
            setShareIntent.PutExtra (Intent.ExtraText, GetString (Resource.String.share_global_message_text) + " " + GetString (Resource.String.app_store_url));

            return setShareIntent;
        }

        private void ListItemClicked (int itemId, bool shouldCloseDrawer = true)
        {
            // Close the drawer if item was clicked (configurable)
            if (_drawerLayout != null && shouldCloseDrawer)
            {
                _drawerLayout.CloseDrawers ();
            }

            // Depending on the item ID that was selected in the drawer,
            // replace the fragment with the corresponding fragment
            switch (itemId)
            {
            case Resource.Id.nav_profile:
                _prevFragment = new ProfileFragment ();
                break;
            case Resource.Id.nav_categories:
                _prevFragment = new CategoriesFragment ();
                break;
            case Resource.Id.nav_about:
                _prevFragment = new AboutFragment ();
                break;
            case Resource.Id.nav_logoutLogin:
                _prevFragment = new LoginFragment ();
                break;
            case Resource.Id.nav_trending:
                _prevFragment = new TrendingVideosFragment ();
                break;
            case Resource.Id.nav_endingSoon:
                _prevFragment = new NewestVideosFragment ();
                break;
            default:
                _prevFragment = new CategoriesFragment ();
                break;
            }

            // Make the actual change of fragments
            SupportFragmentManager.BeginTransaction ()
                .Replace (Resource.Id.content_frame, _prevFragment)
                .Commit ();
        }
    }
}


