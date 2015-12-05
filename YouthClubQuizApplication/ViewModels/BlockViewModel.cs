namespace YouthClubQuizApplication.ViewModels
{
    public class BlockViewModel
    {
        public BlockViewModel(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public int Left { get; private set; }

        public int Top { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }
    }
}
