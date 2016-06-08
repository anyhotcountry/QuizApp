using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace QuizApp.Services
{
    public interface IQuestionsService
    {
        Task PickFiles();

        string GetAnswer(string filename);

        Task<string> GetAnswersAsync();

        Task<IList<string>> GetAnswersListAsync();

        Task SaveQuiz(IDictionary<string, BitmapImage> images);
    }
}