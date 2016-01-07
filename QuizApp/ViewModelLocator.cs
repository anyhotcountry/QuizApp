using QuizApp.Services;
using QuizApp.ViewModels;
using System;

namespace QuizApp
{
    public class ViewModelLocator
    {
        private readonly Lazy<QuizPageViewModel> quizPageViewModel = new Lazy<QuizPageViewModel>(() =>
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return null;

            return new QuizPageViewModel(QuestionsService.Instance, new MediaService(), new PresentationService(typeof(object)));
        });

        public QuizPageViewModel QuizPageViewModel
        {
            get
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    return null;

                return quizPageViewModel.Value;
            }
        }

        public AnswersPageViewModel AnswersPageViewModel
        {
            get { return new AnswersPageViewModel(QuestionsService.Instance); }
        }
    }
}
