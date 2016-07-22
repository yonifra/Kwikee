using System;
using System.Collections.Generic;

namespace FiveMin.Portable.Entities
{
    public class User
    {
        public string Username { get; private set; }
        public DateTime JoinedDate { get; set; }
        public List<FiveMinVideo> FavoriteVideos { get; set; }
        public List<User> Friends { get; set; }
        public List<FiveMinVideo> WatchLater { get; set; }
        public List<Category> FavoriteCategories { get; set; }
        public Tuple<int, int> LikesDislikes { get; set; }

        public User(string username)
        {
            LikesDislikes = new Tuple<int, int>(0, 0);
            JoinedDate = DateTime.Now;
            FavoriteVideos = new List<FiveMinVideo>();
            Friends = new List<User>();
            Username = username;
        }
    }
}
