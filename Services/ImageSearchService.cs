using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace QuizApp.Services
{
    public class ImageSearchService : IImageSearchService
    {
        public async Task<IEnumerable<Uri>> Search(string query, bool animated)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            var animatedQuery = animated ? "&imageType=AnimatedGif" : string.Empty;
            var uri = $"https://www.googleapis.com/customsearch/v1?cx={ApiKey.Cx}&searchType=image&safe=high&key={ApiKey.Key}&q={WebUtility.UrlEncode(query)}";

            var response = await client.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();
            var searchResults = JsonConvert.DeserializeObject<SearchResults>(json);
            return searchResults.items.Select(r => new Uri(r.link));
        }
    }
}