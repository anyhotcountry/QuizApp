using System;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Printing;

namespace QuizApp.Services
{
    public class PrintService : IPrintService
    {
        private PrintDocument printDocument;
        private IPrintDocumentSource printDocumentSource;
        private Page pageToPrint;

        public PrintService(Page pageToPrint)
        {
            this.pageToPrint = pageToPrint;
        }

        public async Task ShowPrintUIAsync()
        {
            // Catch and print out any errors reported
            try
            {
                printDocument = new PrintDocument();
                printDocumentSource = printDocument.DocumentSource;
                printDocument.Paginate += CreatePrintPreviewPages;
                printDocument.GetPreviewPage += GetPrintPreviewPage;
                printDocument.AddPages += AddPrintPages;

                var printMan = PrintManager.GetForCurrentView();
                printMan.PrintTaskRequested += PrintTaskRequested; await PrintManager.ShowPrintUIAsync();
            }
            catch (Exception e)
            {
                // Swallow silently
            }
        }

        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask("Quiz Answers", sourceRequested =>
            {
                sourceRequested.SetSource(printDocumentSource);
            });
        }

        private void AddPrintPages(object sender, AddPagesEventArgs e)
        {
            printDocument.AddPage(pageToPrint);
            var printDoc = (PrintDocument)sender;
            printDoc.AddPagesComplete();
        }

        private void GetPrintPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            var printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPage(e.PageNumber, pageToPrint);
        }

        private void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            var printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }
    }
}
