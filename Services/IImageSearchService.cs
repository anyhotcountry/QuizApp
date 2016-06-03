using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace QuizApp.Services
{
    public interface IImageSearchService
    {
        Task<IEnumerable<ImageSource>> Search(string query, int count);
    }
}