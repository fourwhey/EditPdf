using EditPdf.Interface;

using iText.Kernel.Pdf;
using iText.Kernel.Geom;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class BlankPageHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            PdfUtils.SafeClearConsole();
            Console.WriteLine("Insert blank pages into the document.");
            Console.WriteLine($"Current document has {maxPage} pages.");
            Console.WriteLine();

            Console.Write($"Insert at page number (1 to {maxPage + 1}): ");
            int insertPosition;
            while (!int.TryParse(Console.ReadLine(), out insertPosition) || insertPosition < 1 || insertPosition > maxPage + 1)
            {
                Console.WriteLine($"Invalid position. Please enter a number between 1 and {maxPage + 1}.");
                Console.Write($"Insert at page number (1 to {maxPage + 1}): ");
            }

            Console.Write("How many blank pages to insert: ");
            int numPages;
            while (!int.TryParse(Console.ReadLine(), out numPages) || numPages < 1)
            {
                Console.WriteLine("Please enter a number greater than 0.");
                Console.Write("How many blank pages to insert: ");
            }

            Console.WriteLine();
            Console.WriteLine("Blank page size:");
            Console.WriteLine("  1. A4 (210mm x 297mm)");
            Console.WriteLine("  2. Letter (8.5\" x 11\")");
            Console.WriteLine("  3. A3 (297mm x 420mm)");
            Console.WriteLine("  4. Legal (8.5\" x 14\")");
            Console.Write("Select page size (1-4) [default: 1]: ");

            string? sizeChoice = Console.ReadLine();
            PageSize pageSize = sizeChoice switch
            {
                "2" => PageSize.LETTER,
                "3" => PageSize.A3,
                "4" => PageSize.LEGAL,
                _ => PageSize.A4
            };

            Console.WriteLine();
            for (int i = 0; i < numPages; i++)
            {
                PdfPage newPage = doc.AddNewPage(insertPosition, pageSize);
                Console.WriteLine($"Blank page inserted at position {insertPosition}");
                insertPosition++;
            }

            Console.WriteLine("Completed.");
        }
    }
}
