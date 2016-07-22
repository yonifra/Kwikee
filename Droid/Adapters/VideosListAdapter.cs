﻿using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Square.Picasso;
using FiveMin.Droid.Helpers;
using FiveMin.Portable.Entities;
using Android;

namespace FiveMin.Droid.Adapters
{
    internal class VideoAdapterWrapper : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView Description { get; set; }
        public ImageView Backdrop { get; set; }
    }

    internal class VideosListAdapter : BaseAdapter
    {
        private readonly Activity _context;
        private readonly IEnumerable<FiveMinVideo> _videos;

        public VideosListAdapter (Activity context, IEnumerable<FiveMinVideo> videos)
        {
            _context = context;
            _videos = videos;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            if (position < 0)
                return null;

            var view = convertView ?? _context.LayoutInflater.Inflate (Resource.Layout.VideoListItemLayout, parent, false);

            if (view == null)
                return null;

            var wrapper = view.Tag as VideoAdapterWrapper;
            if (wrapper == null)
            {
                wrapper = new VideoAdapterWrapper
                {
                    Name = view.FindViewById<TextView> (Resource.Id.compNameTextView),
                    Description = view.FindViewById<TextView> (Resource.Id.compDescriptionTextView),
                    Backdrop = view.FindViewById<ImageView> (Resource.Id.compBackdropImageView)
                };
                view.Tag = wrapper;
            }

            var video = _videos.ElementAt (position);

            wrapper.Backdrop.SetBackgroundResource (Resource.Color.button_material_light);
            wrapper.Name.Text = video.Name;
            wrapper.Description.Text = video.Description;

            FontsHelper.ApplyTypeface (_context.Assets, new List<TextView> { wrapper.Name, wrapper.Description });

            // Load the image asynchonously
            Picasso.With (_context).Load (video.ImageUrl).Into (wrapper.Backdrop);

            return view;
        }

        public override int Count => _videos.Count ();

        public override Java.Lang.Object GetItem (int position)
        {
            return position;
        }

        public List<FiveMinVideo> Videos => _videos.ToList ();

        public override long GetItemId (int position)
        {
            return position;
        }

        public override bool HasStableIds => true;
    }
}

