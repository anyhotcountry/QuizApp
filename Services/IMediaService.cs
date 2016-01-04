using System.Threading.Tasks;

namespace QuizApp.Services
{
    public interface IMediaService
    {
        object MediaElement { get; }

        Task SpeakAsync(string text);
    }
}
