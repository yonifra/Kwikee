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
        private const string BasePath = "https://fivemin.firebaseio.com/";
        private const string FirebaseSecret = "0MyNdWBFTqAfUdRyr801j6lfWvSRb157WpWv9Q9n";
        private static FirebaseClient _client;
        private const string CategoriesName = "categories";
        private const string CompetitionsName = "videos";
        private const string EntitiesName = "entities";
        private Dictionary<string, FiveMinVideo> _competitions;
        private Dictionary<string, FiveMinVideo> _trendingCompetitions;
        private Dictionary<string, FiveMinVideo> _endingSoonCompetitions;
        private Dictionary<string, Category> _categories;
        private Dictionary<string, VsEntity> _entities;

        // Competitions with more than the threshold vote counts will be considered "trending"
        const int THRESHOLD = 40;
        private readonly TimeSpan ENDING_SOON_THRESHOLD = TimeSpan.FromDays (3);


        public static FirebaseManager Instance => _instance ?? (_instance = new FirebaseManager());

        private FirebaseManager()
        {
            var config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };

            _client = new FirebaseClient(config);
        }

        public FirebaseClient Client => _client;

        /// <summary>
        /// Deletes ALL the data from the server. USE WITH CAUTION!
        /// </summary>
        private async void DeleteAllData()
        {
            await DeleteNode(CategoriesName);
            await DeleteNode(CompetitionsName);
            await DeleteNode(EntitiesName);
        }

        public async Task<bool> DeleteNode(string nodeName)
        {
            var response = await _client.DeleteAsync(nodeName);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async void UpdateVote(string entity, string competition)
        {
            var isUserAuthenticated = await IsUserAuthenticatedAsync();

            if (isUserAuthenticated)
            {
                var entities = await GetAllEntities();
                var competitions = await GetAllCompetitions();

                if (entities.Any(e => e.Value.Name == entity))
                {
                    // Entity is valid
                    foreach (var c in competitions.Where(c => c.Value.Name == competition))
                    {
                        if (c.Value.CompetitorName1 == entity)
                        {
                            c.Value.CompetitorScore1++;
                            UpdateCompetition(c.Value, c.Key);
                        }
                        else if (c.Value.CompetitorName2 == entity)
                        {
                            c.Value.CompetitorScore2++;
                            UpdateCompetition(c.Value, c.Key);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets all the ending soon competitions
        /// </summary>
        /// <returns>The ending soon competitions.</returns>
        /// <param name="shouldRefresh">If set to <c>true</c> should refresh.</param>
        public async Task<Dictionary<string, FiveMinVideo>> GetEndingSoonCompetitions (bool shouldRefresh = false)
        {
            if (_endingSoonCompetitions == null || (_competitions == null || shouldRefresh))
            {
                await GetAllCompetitions (shouldRefresh);
                _endingSoonCompetitions = new Dictionary<string, FiveMinVideo> ();
            }

            _endingSoonCompetitions = _competitions
                .Where (cm => DateTime.Now - cm.Value.EndingDate <= ENDING_SOON_THRESHOLD)
                .ToDictionary (o => o.Key, o => o.Value);

            return _endingSoonCompetitions;
        }

        /// <summary>
        /// Updates the vote. Position 1 updates the left side, position 2 updates the right side
        /// </summary>
        /// <returns>The vote.</returns>
        /// <param name="position">Position.</param>
        /// <param name="competition">Competition.</param>
        public async void UpdateVote(int position, string competition)
        {
            var isUserAuthenticated = await IsUserAuthenticatedAsync();

            if (isUserAuthenticated)
            {
                var competitions = await GetAllCompetitions();

                // Entity is valid
                foreach (var c in competitions.Where(c => c.Value.Name == competition))
                {
                    switch (position)
                    {
                        case 1:
                            c.Value.CompetitorScore1++;
                            UpdateCompetition(c.Value, c.Key);
                            break;
                        case 2:
                            c.Value.CompetitorScore2++;
                            UpdateCompetition(c.Value, c.Key);
                            break;
                    }
                }
            }
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            return true;
            // throw new NotImplementedException();
        }

        private static async void UpdateCompetition(FiveMinVideo value, string key)
        {
            if (string.IsNullOrEmpty(value.CompetitorName1))
            {
                value.CompetitorName1 = " ";
            }

            if (string.IsNullOrEmpty(value.CompetitorName2))
            {
                value.CompetitorName2 = " ";
            }

            await _client.UpdateAsync($"{CompetitionsName}/{key}", value);
        }

        public async Task<bool> AddCompetition(FiveMinVideo competition)
        {
            var response = await _client.PushAsync(CompetitionsName, competition);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> AddEntity(VsEntity entity)
        {
            var response = await _client.PushAsync(EntitiesName, entity);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> AddCategory(Category category)
        {
            var response = await _client.PushAsync(CategoriesName, category);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Returns a Dictionary of all of the competitions keys and values. (With caching)
        /// </summary>
        /// <param name="shouldRefresh">Indicates if we want to force an update of the competitions list</param>
        /// <returns>Dictionary of competitions and their keys</returns>
        public async Task<Dictionary<string, FiveMinVideo>> GetAllCompetitions(bool shouldRefresh = false)
        {
            // if we want to force the update, or the competitions dictionary have not yet been initialized, update it!
            if (shouldRefresh || _competitions == null)
            {
                var response = await _client.GetAsync(CompetitionsName);

                _competitions = JsonConvert.DeserializeObject<Dictionary<string, FiveMinVideo>>(response.Body);
            }

            return _competitions;
        }

        /// <summary>
        /// Gets all the currently trending competitions
        /// </summary>
        /// <returns>The all competitions.</returns>
        /// <param name="shouldRefresh">If set to <c>true</c> should refresh.</param>
        public async Task<Dictionary<string, FiveMinVideo>> GetTrendingCompetitions (int maxCompetitions = 20, bool shouldRefresh = false)
        {
            if (_trendingCompetitions == null || (_competitions == null || shouldRefresh))
            {
                await GetAllCompetitions (shouldRefresh);
                _trendingCompetitions = new Dictionary<string, FiveMinVideo> ();
            }

            _trendingCompetitions = _competitions
                .Where (cm => cm.Value.CompetitorScore1 + cm.Value.CompetitorScore2 > THRESHOLD)
                .ToDictionary (o => o.Key, o => o.Value);

            return _trendingCompetitions;
        }

        /// <summary>
        /// Gets a list of competitions by their category name
        /// </summary>
        /// <param name="categoryName">Name of the category we wish to get all competitions for</param>
        /// <param name="refreshBefore">Indicates whether to refresh the cache before fetching the competition</param>
        /// <returns>An enumerable of all competitions related to that category</returns>
        public async Task<IEnumerable<FiveMinVideo>> GetCompetitions(string categoryName, bool refreshBefore = false)
        {
            var dict = await GetAllCompetitions(refreshBefore);

            return dict.Values.Where(c => string.Equals(c.Category, categoryName, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Gets a specific competitions by name
        /// </summary>
        /// <param name="competitionName">The name of the competition to look for</param>
        /// <param name="refreshBefore">Indicates whether we want to refresh the cache before searching</param>
        /// <returns>The VsCompetition entity we found</returns>
        public async Task<FiveMinVideo> GetCompetition(string competitionName, bool refreshBefore = false)
        {
            var dict = await GetAllCompetitions(refreshBefore);

            return dict.Values.FirstOrDefault(c => string.Equals(c.Name, competitionName, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<Dictionary<string, VsEntity>> GetAllEntities(bool shouldRefresh = false)
        {
            if (shouldRefresh || _entities == null)
            {
                var response = await _client.GetAsync(EntitiesName);

                _entities = JsonConvert.DeserializeObject<Dictionary<string, VsEntity>>(response.Body);
            }

            return _entities;
        }

        public async Task<Dictionary<string, Category>> GetAllCategories(bool shouldRefresh = false)
        {
            if (shouldRefresh || _categories == null)
            {
                var response = await _client.GetAsync(CategoriesName);

                _categories = JsonConvert.DeserializeObject<Dictionary<string, Category>>(response.Body);
            }

            return _categories;
        }

        public async Task<VsEntity> GetEntityByName(string name)
        {
            var dic = await GetAllEntities();

            return dic.Values.First(e => string.Equals(e.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
