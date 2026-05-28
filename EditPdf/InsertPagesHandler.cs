using EditPdf.Interface;

using iText.Kernel.Pdf;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class InsertPagesHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            Console.Clear();
            Console.WriteLine("Insert pages into the document.");
            Console.WriteLine($"Current document has {maxPage} pages.");
            Console.WriteLine();

            Console.Write("Enter file to insert from: ");
            string? insertFile = Console.ReadLine()?.Trim('"');

            if (string.IsNullOrWhiteSpace(insertFile) || !File.Exists(insertFile))
            {
                Console.WriteLine("File not found.");
                return;
            }

            // Prevent inserting a file into itself
            if (IsSamePath(inputFile, insertFile))
            {
                Console.WriteLine("Error: Cannot insert a file into itself.");
                return;
            }

            try
            {
                using (PdfDocument insertDoc = new(new PdfReader(insertFile)))
                {
                    int insertMaxPage = insertDoc.GetNumberOfPages();

                    // Validate the source PDF is not empty
                    if (!ValidateNonEmptyPdf(insertMaxPage))
                    {
                        return;
                    }

                    Console.WriteLine($"Source document has {insertMaxPage} pages.");
                    Console.WriteLine();

                    PageRange insertRange = GetPageRange(1, insertMaxPage);

                    Console.Write($"Insert at page number (1 to {maxPage + 1}): ");
                    int insertPosition;
                    while (!int.TryParse(Console.ReadLine(), out insertPosition) || insertPosition < 1 || insertPosition > maxPage + 1)
                    {
                        Console.WriteLine($"Invalid position. Please enter a number between 1 and {maxPage + 1}.");
                        Console.Write($"Insert at page number (1 to {maxPage + 1}): ");
                    }

                    int startPage = insertRange.StartPage ?? 1;
                    int endPage = insertRange.EndPage ?? insertMaxPage;

                    // Validate the range
                    if (!ValidatePageRange(startPage, endPage, insertMaxPage))
                    {
                        return;
                    }

                    Console.WriteLine();
                    // Copy pages from source document to target document
                    for (int i = startPage; i <= endPage; i++)
                    {
                        var pageToInsert = insertDoc.GetPage(i).CopyTo(doc);
                        doc.AddPage(insertPosition, pageToInsert);
                        Console.WriteLine($"Page {i} from source: Inserted at position {insertPosition}");
                        insertPosition++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Failed to insert pages: {ex.Message}");
                return;
            }

            Console.WriteLine("Completed.");
        }
    }

}
