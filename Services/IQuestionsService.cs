using System.Threading.Tasks;

namespace QuizApp.Services
{
    public interface IQuestionsService
    {
        Task PickFiles();

        string GetAnswer(string filename);

        Task<string> GetAnswersAsync();
    }
}
