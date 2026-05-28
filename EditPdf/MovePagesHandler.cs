using iText.Kernel.Pdf;

using EditPdf.Interface;
using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class MovePagesHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            Console.Clear();
            Console.WriteLine("Move a page in the document.");
            Console.WriteLine($"Document has {maxPage} pages.");
            Console.WriteLine();

            Console.Write("Enter page number to move: ");
            int pageToMove;
            while (!int.TryParse(Console.ReadLine(), out pageToMove) || pageToMove < 1 || pageToMove > maxPage)
            {
                Console.WriteLine($"Invalid page number. Please enter a number between 1 and {maxPage}.");
                Console.Write("Enter page number to move: ");
            }

            Console.Write($"Enter new position (1 to {maxPage}): ");
            int newPosition;
            while (!int.TryParse(Console.ReadLine(), out newPosition) || newPosition < 1 || newPosition > maxPage)
            {
                Console.WriteLine($"Invalid position. Please enter a number between 1 and {maxPage}.");
                Console.Write($"Enter new position (1 to {maxPage}): ");
            }

            // Validate page numbers
            if (!ValidatePageNumber(pageToMove, maxPage, "Source page"))
            {
                return;
            }

            if (!ValidatePageNumber(newPosition, maxPage, "Destination position"))
            {
                return;
            }

            doc.MovePage(pageToMove, newPosition);
            Console.WriteLine($"Page {pageToMove}: Moved to position {newPosition}");

            Console.WriteLine("Completed.");
        }
    }

}
