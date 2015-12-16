using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace QuizApp.Services
{
    public class QuestionsService
    {
        public static QuestionsService Instance { get; }
        static QuestionsService()
        {
            Instance = Instance ?? new QuestionsService();
        }

        public async Task<string> GetAnswersAsync()
        {
            var sb = new StringBuilder();
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Quiz");
            if (folder == null)
            {
                return string.Empty;
            }

            var files = await folder.GetFilesAsync();
            foreach (var item in files)
            {
                sb.AppendLine(GetAnswer(item.Name));
            }

            return sb.ToString();
        }

        private QuestionsService()
        {
        }

        public async Task PickFiles()
        {
            var folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();
            var quizFolder = folders.FirstOrDefault(f => f.Name == "Quiz");
            if (folders.FirstOrDefault(f => f.Name == "Quiz") != null)
            {
                await quizFolder.DeleteAsync();
            }

            quizFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Quiz");
            var openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            openPicker.FileTypeFilter.Add("*");
            var files = await openPicker.PickMultipleFilesAsync();
            var random = new Random();
            foreach (var file in files)
            {
                await file.CopyAsync(quizFolder, string.Format("{0:0000}_{1}", random.Next() % 10000, file.Name));
            }
        }

        public string GetAnswer(string filename)
        {
            var answer = Path.GetFileNameWithoutExtension(filename);
            answer = answer.Substring(4).Replace("Zoomed_", string.Empty).Replace("_", " ");
            answer = Regex.Replace(answer, "(?!^)([A-Z])", " $1").Trim();
            return answer;
        }
    }
}
