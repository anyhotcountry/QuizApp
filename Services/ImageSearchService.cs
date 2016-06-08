﻿using Bing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Services
{
    public class ImageSearchService : IImageSearchService
    {
        public Task<IEnumerable<Uri>> Search(string query, int count)
        {
            var bsc = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/Search/"));
            bsc.Credentials = new System.Net.NetworkCredential(ApiKey.Key, ApiKey.Key);
            var webQuery = bsc.Image(query, null, null, "Strict", null, null, "Size:Large");
            return Task<IEnumerable<Uri>>.Factory.FromAsync(webQuery.BeginExecute, x => webQuery.EndExecute(x).Take(count).Select(r => new Uri(r.MediaUrl, UriKind.Absolute)), null);
        }
    }
}