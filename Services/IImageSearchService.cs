using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApp.Services
{
    public interface IImageSearchService
    {
        Task<IEnumerable<Uri>> Search(string query, bool animated);
    }
}