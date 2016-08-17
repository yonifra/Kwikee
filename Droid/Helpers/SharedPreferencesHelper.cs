using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using FiveMin.Portable.Entities;

namespace FiveMin.Droid.Helpers
{
    public class SharedPreferencesHelper
    {
        private static SharedPreferencesHelper _instance;
        private ISharedPreferences _favPrefs;
        private ISharedPreferences _watchListPrefs;
        private ISharedPreferences _likedVidsPrefs;
        private ISharedPreferences _dislikedVidsPrefs;
        private ISharedPreferences _watchedVidsPrefs;

        private SharedPreferencesHelper ()
        {
            _favPrefs = Application.Context.GetSharedPreferences ("FavoriteVideos", FileCreationMode.Private);
            _watchListPrefs = Application.Context.GetSharedPreferences ("WatchListVideos", FileCreationMode.Private);
            _likedVidsPrefs = Application.Context.GetSharedPreferences ("LikedVideos", FileCreationMode.Private);
            _dislikedVidsPrefs = Application.Context.GetSharedPreferences ("DislikedVideos", FileCreationMode.Private);
            _watchedVidsPrefs = Application.Context.GetSharedPreferences ("WatchedVideos", FileCreationMode.Private);
        }

        public static SharedPreferencesHelper Instance
        {
            get { return _instance ?? (_instance = new SharedPreferencesHelper ()); }
        }

        public void AddFavoriteVideo (string videoKey)
        {
            var key = _favPrefs.GetString (videoKey, string.Empty);

            if (key == string.Empty)
            {
                // Not in favorites, add it
                _favPrefs.Edit ().PutString (key, videoKey);
            }
        }

        public void RemoveFavoriteVideo (string videoKey)
        {
            var key = _favPrefs.GetString (videoKey, string.Empty);

            if (key != string.Empty)
            {
                // Video is in favorites, remove it
                _favPrefs.Edit ().Remove (key);
            }
        }

        public List<string> GetAllFavorites ()
        {
            return (List<string>)_favPrefs.All.Values;
        }
    }
}

