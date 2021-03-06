﻿using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Square.Picasso;
using Kwikee.Droid.Helpers;
using Kwikee.Portable.Entities;
using Kwikee.Portable.Helpers;

namespace Kwikee.Droid.Adapters
{
    internal class VideoAdapterWrapper : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView Description { get; set; }
        public ImageView Backdrop { get; set; }
        public TextView Length { get; set; }
        public TextView Tags { get; set; }
        public TextView WatchCount { get; set; }
        public TextView LikeDislikeDifference { get; set; }
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
                    Name = view.FindViewById<TextView> (Resource.Id.videoNameTextView),
                    Description = view.FindViewById<TextView> (Resource.Id.videoDescriptionTextView),
                    Backdrop = view.FindViewById<ImageView> (Resource.Id.videoBackdropImageView),
                    Length = view.FindViewById<TextView>(Resource.Id.videoLengthTextView),
                    Tags = view.FindViewById<TextView>(Resource.Id.videoTagsTextView),
                    WatchCount = view.FindViewById<TextView>(Resource.Id.videoWatchCountTextView),
                    LikeDislikeDifference = view.FindViewById<TextView>(Resource.Id.videoLikesDiffTextView)
                };
                view.Tag = wrapper;
            }

            var video = _videos.ElementAt (position);

            wrapper.Backdrop.SetBackgroundResource (Resource.Color.button_material_light);
            wrapper.Name.Text = video.Name;
            wrapper.Description.Text = StringHelper.TrimText(video.Description, 100);
            wrapper.Tags.Text = StringHelper.TagsFormatter (video.Keywords);
            wrapper.Length.Text = StringHelper.TimeSpanFormatter (video.Length);
            wrapper.WatchCount.Text = video.WatchCount.ToString ("N0");
            wrapper.LikeDislikeDifference.Text = (video.Likes - video.Dislikes).ToString ("N0");


            FontsHelper.ApplyTypeface (_context.Assets, 
                                       new List<TextView> { 
                       wrapper.Name, 
                       wrapper.Description, 
                       wrapper.Length, 
                       wrapper.Tags, 
                       wrapper.WatchCount, 
                       wrapper.LikeDislikeDifference 
            });

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

