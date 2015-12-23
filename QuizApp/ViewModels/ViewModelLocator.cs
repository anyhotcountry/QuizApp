namespace QuizApp.ViewModels
{
    public class ViewModelLocator
    {
        private QuizPageViewModel quizPageViewModel;

        public QuizPageViewModel QuizPageViewModel
        {
            get
            {
                return quizPageViewModel = quizPageViewModel ?? new QuizPageViewModel();
            }
        }
    }
}
