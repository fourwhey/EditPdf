using EditPdf.Interface;

using iText.Kernel.Pdf;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class DeletePagesHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            Console.Clear();
            Console.WriteLine("Delete pages in the document.");
            PageRange range = GetPageRange(1, maxPage);

            int startPage = range.StartPage ?? 1;
            int endPage = range.EndPage ?? maxPage;

            // Validate the range
            if (!ValidatePageRange(startPage, endPage, maxPage))
            {
                return;
            }

            // Delete from highest to lowest to avoid index shifts
            for (int i = endPage; i >= startPage; i--)
            {
                doc.RemovePage(i);
                Console.WriteLine($"Page {i}: Deleted");
            }

            Console.WriteLine("Completed.");
        }
    }

}
