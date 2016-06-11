using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace QuizApp.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly HttpClient httpClient = new HttpClient();

        public async Task<string> GetAnswersAsync()
        {
            var sb = new StringBuilder();
            var answers = await GetAnswersListAsync();
            foreach (var answer in answers)
            {
                sb.AppendLine(answer);
            }

            return sb.ToString();
        }

        public async Task<IList<string>> GetAnswersListAsync()
        {
            var sb = new StringBuilder();
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Quiz");
            if (folder == null)
            {
                return new List<string>();
            }

            var files = await folder.GetFilesAsync();
            return files.Select(f => GetAnswer(f.Name)).ToList();
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

        public async Task SaveQuiz(IDictionary<string, Uri> images, IDictionary<string, string> otherQuestions)
        {
            var folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();
            var quizFolder = folders.FirstOrDefault(f => f.Name == "Quiz");
            if (folders.FirstOrDefault(f => f.Name == "Quiz") != null)
            {
                await quizFolder.DeleteAsync();
            }

            var random = new Random();
            quizFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Quiz");
            foreach (var image in images)
            {
                var extension = image.Value.AbsolutePath.EndsWith("png", StringComparison.OrdinalIgnoreCase) ? "png" : "jpg";
                string fileName = string.Format("{0:0000}_{1}.{2}", random.Next() % 10000, image.Key, extension);
                var file = await quizFolder.CreateFileAsync(fileName);
                var response = await httpClient.GetAsync(image.Value);

                using (var inputStream = await response.Content.ReadAsInputStreamAsync())
                {
                    using (var outputStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await RandomAccessStream.CopyAndCloseAsync(inputStream, outputStream);
                    }
                }
            }

            foreach (var question in otherQuestions)
            {
                string fileName = string.Format("{0:0000}_{1}", random.Next() % 10000, question.Key);
                var file = await quizFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, question.Value);
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