using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dapper;
using DM.Data.Utilities.DataLoadUtility.RemoteData;
using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility
{
    // Too much premature optimization, but leaving commented
    // so it gets saved on the next merge, then will remove.
    // Might be neat to get a fully generic Load() method working
    // where you just give it a root endpoint (e.g. /api/classes)
    // and it figures everything else out to load
    //
    // Usage:
    // using var test = new DataLoader<AbilityScore>();
    // await test.Load("/api/ability-scores", "AbilityScores", "");
    //
    //public class DataLoader<T> : IDisposable where T : IRemoteData
    //{
    //    private readonly HttpHelper<T> _classHelper;

    //    private readonly SqlConnection _database;

    //    private readonly HttpHelper<NamedAPIResourceList> _listHelper;

    //    public DataLoader()
    //    {
    //        _listHelper = new HttpHelper<NamedAPIResourceList>();
    //        _classHelper = new HttpHelper<T>();
    //        _database = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DM;Trusted_Connection=True;");
    //    }

    //    public void Dispose()
    //    {
    //        _database?.Dispose();
    //        _classHelper?.Dispose();
    //        _listHelper?.Dispose();
    //    }

    //    /// <summary>
    //    /// Load data from an API root of type <see cref="NamedAPIResourceList"/> into the database
    //    /// </summary>
    //    /// <param name="apiRoot">API root of type <see cref="NamedAPIResourceList"/>, e.g. /api/ability-scores</param>
    //    /// <param name="tableName">What table to load the data into</param>
    //    /// <param name="insertQuery">The query to execute for insert</param>
    //    /// <returns>Awaitable Task</returns>
    //    /// todo Currently will fail with multiple levels of <see cref="NamedAPIResourceList"/>, update to handle this recursively
    //    public async Task Load(string apiRoot, string tableName, string insertQuery)
    //    {
    //        var results = new List<T>();
    //        var endpoints = await _listHelper.ReadContent(apiRoot);

    //        foreach (var endpoint in endpoints.Results)
    //        {
    //            var content = await _classHelper.ReadContent(endpoint.URL);

    //            results.Add(content);
    //        }

    //        var resultsToLoad = results.ToArray();

    //        if (resultsToLoad.Any())
    //        {
    //            var deleteBuilder = new SqlBuilder();

    //            deleteBuilder.Select(tableName);

    //            var deleteTemplate = deleteBuilder.AddTemplate("DELETE FROM /**select**/");

    //            await _database.ExecuteAsync(deleteTemplate.RawSql);
    //        }
    //    }
    //}

    public class HttpHelper<T> : IDisposable
    {
        private const string ApiRoot = "http://dnd5eapi.co";

        private readonly HttpClient _client;

        public HttpHelper()
        {
            _client = new HttpClient();

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<T> ReadContent(string route)
        {
            using var httpResponse = await _client.GetAsync($"{ApiRoot}{route}");

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}