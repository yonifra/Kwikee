using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Square.Picasso;
using FiveMin.Droid.Helpers;
using FiveMin.Portable.Entities;
using FiveMin.Portable.Helpers;

namespace FiveMin.Droid.Adapters
{
    internal class CategoryAdapterWrapper : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView Description { get; set; }
        public TextView Tags { get; set; }
        public ImageView Backdrop { get; set; }
    }

    public class CategoriesListAdapter : BaseAdapter
    {
        private readonly Activity _context;
        private readonly IEnumerable<Category> _categories;

        public CategoriesListAdapter (Activity context, IEnumerable<Category> categories)
        {
            _context = context;
            _categories = categories;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            if (position < 0)
                return null;

            var view = convertView ?? _context.LayoutInflater.Inflate (Resource.Layout.category_listitem_layout, parent, false);

            if (view == null)
                return null;

            var wrapper = view.Tag as CategoryAdapterWrapper;
            if (wrapper == null)
            {
                wrapper = new CategoryAdapterWrapper
                {
                    Name = view.FindViewById<TextView> (Resource.Id.catNameTextView),
                    Description = view.FindViewById<TextView> (Resource.Id.catDescriptionTextView),
                    Backdrop = view.FindViewById<ImageView> (Resource.Id.catBackdropImageView),
                    Tags = view.FindViewById<TextView> (Resource.Id.catTagsTextView)
                };
                view.Tag = wrapper;
            }

            var category = _categories.ElementAt (position);

            wrapper.Backdrop.SetBackgroundResource (Android.Resource.Color.Transparent);
            wrapper.Name.Text = category.Name;
            wrapper.Description.Text = StringHelper.TrimText(category.Description, 100);
            wrapper.Tags.Text = StringHelper.TagsFormatter (category.Keywords);

            FontsHelper.ApplyTypeface (_context.Assets, new List<TextView> { wrapper.Name, wrapper.Description, wrapper.Tags });

            // Load the image asynchonously
            Picasso.With (_context).Load (category.ImageUrl).Into (wrapper.Backdrop);

            return view;
        }

        public override int Count => _categories.Count ();

        public override Java.Lang.Object GetItem (int position)
        {
            return position;
        }

        public List<Category> Categories => _categories.ToList ();

        public override long GetItemId (int position)
        {
            return position;
        }

        public override bool HasStableIds => true;
    }
}

