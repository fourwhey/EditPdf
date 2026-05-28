using EditPdf.Interface;

using iText.Kernel.Pdf;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class DuplicatePagesHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            PdfUtils.SafeClearConsole();
            Console.WriteLine("Duplicate pages in the document.");

            PageRange range = GetPageRange(1, maxPage);
            int startPage = range.StartPage ?? 1;
            int endPage = range.EndPage ?? maxPage;

            // Validate the range
            if (!ValidatePageRange(startPage, endPage, maxPage))
            {
                return;
            }

            Console.Write("How many times to duplicate (1-10): ");
            int times;
            while (!int.TryParse(Console.ReadLine(), out times) || times < 1 || times > 10)
            {
                Console.WriteLine("Please enter a number between 1 and 10.");
                Console.Write("How many times to duplicate (1-10): ");
            }

            Console.WriteLine();
            int pagesAdded = 0;

            // For each page in the range, duplicate it N times
            for (int i = startPage; i <= endPage; i++)
            {
                var pageToClone = doc.GetPage(i);

                for (int dup = 0; dup < times; dup++)
                {
                    var clonedPage = pageToClone.CopyTo(doc);
                    doc.AddPage(clonedPage);
                    pagesAdded++;
                }

                Console.WriteLine($"Page {i}: Duplicated {times} times");
            }

            Console.WriteLine();
            Console.WriteLine($"Completed. Added {pagesAdded} duplicate page(s).");
        }
    }
}
