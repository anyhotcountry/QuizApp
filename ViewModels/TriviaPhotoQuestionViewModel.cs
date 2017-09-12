using QuizApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace QuizApp.ViewModels
{
    public class TriviaPhotoQuestionViewModel : TriviaQuestionViewModel
    {
        private BitmapImage imageSource;
        private readonly string localFilename;

        public TriviaPhotoQuestionViewModel(string filename, int index, IQuestionsService questionsService, IMediaService mediaService, bool isPreview) : base(filename, index, questionsService, mediaService, isPreview)
        {
        }

        public BitmapImage ImageSource
        {
            get { return imageSource; }

            set { Set(ref imageSource, value); }
        }

        protected override async Task ShowQuestion()
        {
            Answer = QuestionsService.GetAnswer(Filename.Replace(".trivia", string.Empty));
            for (var i = 0; i < Answer.Length; i++)
            {
                Letters.Add(new LetterViewModel { Letter = Answer[i].ToString(), Visible = false, Position = i });
            }

            leftLetters = new List<LetterViewModel>(Letters);
            gameTimer.Interval = TimeSpan.FromMilliseconds((isPreview ? 5000 : 30000) / (double)Answer.Length);
            gameTimer.Start();
            var file = await StorageFile.GetFileFromPathAsync(Filename);
            var fileStream = await file.OpenAsync(FileAccessMode.Read);
            var img = new BitmapImage();
            img.SetSource(fileStream);

            ImageSource = img;
        }
    }
}