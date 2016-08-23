using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Kwikee.Droid.Activities;
using Kwikee.Droid.Adapters;
using Kwikee.Portable.Data;
using Kwikee.Portable.Entities;
using Plugin.Connectivity;

namespace Kwikee.Droid.Fragments
{
    public class CategoriesFragment : Android.Support.V4.App.Fragment
    {
        private Dictionary<string, Category> _categories;
        private View _view;

        public CategoriesFragment ()
        {
            RetainInstance = true;
        }

        public override void OnAttach (Android.Content.Context context)
        {
            base.OnAttach (context);
            var toolbar = (context as BaseActivity)?.Toolbar;

            toolbar?.SetTitle (Resource.String.fragment_title_categories);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView (inflater, container, savedInstanceState);

            //var view = inflater.Inflate(Resource.Layout.fragment_categories, container, false);
            _view = inflater.Inflate (Resource.Layout.fragment_categories, null);

            LoadDataToGrid ();

            return _view;
        }

        private async void LoadDataToGrid ()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var categoriesListView = _view.FindViewById<ListView> (Resource.Id.categoriesListView);

                _categories = await FirebaseManager.Instance.GetAllCategories ();

                if (_categories != null && _categories.Any())
                {
                    categoriesListView.Adapter = new CategoriesListAdapter (Activity, _categories.Values);

                    var msgText = _view.FindViewById<TextView> (Resource.Id.noCategoriesMessage);
                    msgText.Visibility = ViewStates.Invisible;
                    categoriesListView.Visibility = ViewStates.Visible;
                }

                categoriesListView.ItemClick += (sender, e) =>
                {
                    var index = e.Position;

                    var lv = (sender as ListView);

                    if (lv != null)
                    {
                        var categoriesListAdapter = lv.Adapter as CategoriesListAdapter;

                        if (categoriesListAdapter != null)
                        {
                            var category = categoriesListAdapter.Categories [index];
                            var fragment = new VideosFragment { SelectedCategoryName = category.Name };

                            Activity.SupportFragmentManager.BeginTransaction ()
                                    .Replace (Resource.Id.content_frame, fragment)
                                    .AddToBackStack (fragment.Tag)
                                    .Commit ();
                        }
                    }
                };
            }
            else
            {
                Snackbar.Make (_view, Resource.String.no_internet_message, Snackbar.LengthLong).Show ();
            }
        }


        private async void GetAllCategoriesAsync ()
        {
            _categories = await FirebaseManager.Instance.GetAllCategories ();
        }
    }
}

