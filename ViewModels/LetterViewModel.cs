namespace QuizApp.ViewModels
{
    public class LetterViewModel : QuizApp.Mvvm.ViewModelBase
    {
        private string letter;
        private bool visible;

        public string Letter
        {
            get { return letter; }

            set { Set(ref letter, value); }
        }

        public bool Visible
        {
            get { return visible; }

            set { Set(ref visible, value); }
        }
    }
}