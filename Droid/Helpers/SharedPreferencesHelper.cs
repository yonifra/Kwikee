using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Kwikee.Portable.Enums;

namespace Kwikee.Droid.Helpers
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

        public bool AddVideoToSharedPreferences (string videoKey, SharedPreferenceType type)
        {
            if (videoKey == null) return false;

            var sharedPref = GetSharedPreferernceByType (type);
            var key = sharedPref.GetString (videoKey, string.Empty);

            if (key == string.Empty)
            {
                // Not in favorites, add it
                sharedPref.Edit ().PutString (videoKey, videoKey).Apply ();
                return true;
            }

            return false;
        }

        public bool RemoveVideoFromSharedPreferences (string videoKey, SharedPreferenceType type)
        {
            if (videoKey == null) return false;

            var sharedPref = GetSharedPreferernceByType (type);
            var key = sharedPref.GetString (videoKey, string.Empty);

            if (key != string.Empty)
            {
                // Video is in favorites, remove it
                sharedPref.Edit ().Remove (key).Apply ();
                return true;
            }

            return false;
        }

        public List<string> GetAllVideos (SharedPreferenceType type)
        {
            var sharedPref = GetSharedPreferernceByType (type);
            return sharedPref.All.Keys.ToList();
        }

        public int GetCount (SharedPreferenceType type)
        {
            var sharedPref = GetSharedPreferernceByType (type);
            return sharedPref.All.Keys.Count;
        }

        private ISharedPreferences GetSharedPreferernceByType (SharedPreferenceType type)
        {
            switch (type)
            {
            case SharedPreferenceType.Favorites:
                return _favPrefs;
            case SharedPreferenceType.Liked:
                return _likedVidsPrefs;
            case SharedPreferenceType.Disliked:
                return _dislikedVidsPrefs;
            case SharedPreferenceType.Watchlist:
                return _watchListPrefs;
            case SharedPreferenceType.Watched:
                return _watchedVidsPrefs;
            }

            return _favPrefs;
        }
    }
}


