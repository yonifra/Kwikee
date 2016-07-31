using System;

namespace FiveMin.Droid.DataModel
{
    public class UserVideo
    {
        public UserVideo ()
        {
        }

        /// <summary>
        /// The Firebase key
        /// </summary>
        /// <value>Identifies the video by its key</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:FiveMin.Droid.DataModel.FavVideoDataModel"/>
        /// is a favorite video.
        /// </summary>
        /// <value><c>true</c> if is favorite; otherwise, <c>false</c>.</value>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:FiveMin.Droid.DataModel.FavVideoDataModel"/>
        /// is in the user's watchlist.
        /// </summary>
        /// <value><c>true</c> if is in watchlist; otherwise, <c>false</c>.</value>
        public bool IsInWatchlist { get; set; }

        /// <summary>
        /// Indicates if the user liked (+1) or disliked (-1) the video
        /// </summary>
        /// <value>The liked.</value>
        public short Liked { get; set; }
    }
}

