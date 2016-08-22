using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using Newtonsoft.Json;
using FiveMin.Portable.Entities;

namespace FiveMin.Portable.Data
{
    public class FirebaseManager
    {
        private static FirebaseManager _instance;
        private const string BasePath = "https://fivemin-aa516.firebaseio.com/";
        private const string FirebaseSecret = "gfgKbS039HLSpPEfhwvZzcFAx4dezhndxWymbC7V";
        private static FirebaseClient _client;
        private const string CategoriesName = "categories";
        private const string VideosName = "videos";
        private Dictionary<string, FiveMinVideo> _videos;
        private Dictionary<string, FiveMinVideo> _trendingVideos;
        private Dictionary<string, FiveMinVideo> _newestVideos;
        private Dictionary<string, Category> _categories;

        // Competitions with more than the threshold vote counts will be considered "trending"
        const int WATCH_COUNT_THRESHOLD = 10;
        private readonly TimeSpan ENDING_SOON_THRESHOLD = TimeSpan.FromDays (3);


        public static FirebaseManager Instance => _instance ?? (_instance = new FirebaseManager ());

        private FirebaseManager ()
        {
            var config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };

            _client = new FirebaseClient (config);
            GetNewestVideos (true);
        }

        public FirebaseClient Client => _client;

        /// <summary>
        /// Deletes ALL the data from the server. USE WITH CAUTION!
        /// </summary>
        private async void DeleteAllData ()
        {
            await DeleteNode (CategoriesName);
            await DeleteNode (VideosName);
        }

        public async Task<bool> DeleteNode (string nodeName)
        {
            var response = await _client.DeleteAsync (nodeName);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all the ending soon competitions
        /// </summary>
        /// <returns>The ending soon competitions.</returns>
        /// <param name="shouldRefresh">If set to <c>true</c> should refresh.</param>
        public async Task<Dictionary<string, FiveMinVideo>> GetNewestVideos (bool shouldRefresh = false)
        {
            if (_newestVideos == null || (_videos == null || shouldRefresh))
            {
                await GetAllVideos (shouldRefresh);
                _newestVideos = new Dictionary<string, FiveMinVideo> ();
            }

            _newestVideos = _videos
                .OrderByDescending (v => v.Value.DateAdded)
                .ToDictionary (o => o.Key, o => o.Value);

            return _newestVideos;
        }

        public Dictionary<string, FiveMinVideo> AllVideos
        {
            get { return _videos; }
        }

        public async Task<bool> IsUserAuthenticatedAsync ()
        {
            return true;
            // throw new NotImplementedException();
        }

        private async void UpdateVideo (FiveMinVideo value)
        {
            var key = GetKeyForVideo (value);
            await _client.UpdateAsync ($"{VideosName}/{key}", value);
        }

        public void UpdateLikesDislikesCount (FiveMinVideo v, bool isIncrement, bool isLikes)
        {
            if (isLikes)
            {
                if (isIncrement)
                {
                    v.Likes++;
                }
                else
                {
                    v.Likes--;
                }
            }
            else
            {
                if (isIncrement)
                {
                    v.Dislikes++;
                }
                else
                {
                    v.Dislikes--;
                }
            }

            UpdateVideo (v);
        }

        public void UpdateWatchCount (FiveMinVideo v)
        {
            if (v != null)
            {
                v.WatchCount++;
                UpdateVideo (v);
            }
        }

        public string GetKeyForVideo (FiveMinVideo v)
        {
            return _videos.FirstOrDefault (vid => vid.Value == v).Key;
        }

        public async Task<bool> AddVideo (FiveMinVideo video)
        {
            var response = await _client.PushAsync (VideosName, video);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> AddCategory (Category category)
        {
            var response = await _client.PushAsync (CategoriesName, category);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Returns a Dictionary of all of the competitions keys and values. (With caching)
        /// </summary>
        /// <param name="shouldRefresh">Indicates if we want to force an update of the competitions list</param>
        /// <returns>Dictionary of competitions and their keys</returns>
        public async Task<Dictionary<string, FiveMinVideo>> GetAllVideos (bool shouldRefresh = false)
        {
            // if we want to force the update, or the competitions dictionary have not yet been initialized, update it!
            if (shouldRefresh || _videos == null)
            {
                var response = await _client.GetAsync (VideosName);

                _videos = JsonConvert.DeserializeObject<Dictionary<string, FiveMinVideo>> (response.Body);
            }

            return _videos;
        }

        /// <summary>
        /// Gets all the currently trending competitions
        /// </summary>
        /// <returns>The all competitions.</returns>
        /// <param name="shouldRefresh">If set to <c>true</c> should refresh.</param>
        public async Task<Dictionary<string, FiveMinVideo>> GetTrendingVideos (int maxVideos = 20, bool shouldRefresh = false)
        {
            if (_trendingVideos == null || (_videos == null || shouldRefresh))
            {
                await GetAllVideos (shouldRefresh);
                _trendingVideos = new Dictionary<string, FiveMinVideo> ();
            }

            _trendingVideos = _videos
                .Where (cm => cm.Value.WatchCount > WATCH_COUNT_THRESHOLD && cm.Value.Likes >= cm.Value.Dislikes * 2)
                .ToDictionary (o => o.Key, o => o.Value);

            return _trendingVideos;
        }

        /// <summary>
        /// Gets a list of videos by their category name
        /// </summary>
        /// <param name="categoryName">Name of the category we wish to get all videos for</param>
        /// <param name="refreshBefore">Indicates whether to refresh the cache before fetching the videos</param>
        /// <returns>An enumerable of all videos related to that category</returns>
        public async Task<IEnumerable<FiveMinVideo>> GetVideos (string categoryName, bool refreshBefore = false)
        {
            var dict = await GetAllVideos (refreshBefore);

            return dict.Values.Where (c => c.Categories.Contains (categoryName));
        }

        /// <summary>
        /// Gets a specific competitions by name
        /// </summary>
        /// <param name="videoName">The name of the competition to look for</param>
        /// <param name="refreshBefore">Indicates whether we want to refresh the cache before searching</param>
        /// <returns>The VsCompetition entity we found</returns>
        public async Task<FiveMinVideo> GetVideo (string videoName, bool refreshBefore = false)
        {
            var dict = await GetAllVideos (refreshBefore);

            return dict.Values.FirstOrDefault (c => string.Equals (c.Name, videoName, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<Dictionary<string, Category>> GetAllCategories (bool shouldRefresh = false)
        {
            if (shouldRefresh || _categories == null)
            {
                var response = await _client.GetAsync (CategoriesName);

                _categories = JsonConvert.DeserializeObject<Dictionary<string, Category>> (response.Body);
            }

            return _categories;
        }
    }
}
