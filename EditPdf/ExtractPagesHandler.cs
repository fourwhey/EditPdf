using EditPdf.Interface;

using iText.Kernel.Pdf;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class ExtractPagesHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            PdfUtils.SafeClearConsole();
            Console.WriteLine("Extract specific pages to a new document.");
            Console.WriteLine($"Source document has {maxPage} pages.");
            Console.WriteLine();

            PageRange range = GetPageRange(1, maxPage);
            int startPage = range.StartPage ?? 1;
            int endPage = range.EndPage ?? maxPage;

            // Validate the range
            if (!ValidatePageRange(startPage, endPage, maxPage))
            {
                return;
            }

            try
            {
                // Close the existing document to extract pages cleanly
                doc.Close();

                // Open source and create target for extraction
                using (PdfDocument sourceDoc = new(new PdfReader(inputFile)))
                using (PdfDocument targetDoc = new(new PdfWriter(outputFile)))
                {
                    Console.WriteLine();
                    for (int i = startPage; i <= endPage; i++)
                    {
                        var page = sourceDoc.GetPage(i).CopyTo(targetDoc);
                        targetDoc.AddPage(page);
                        Console.WriteLine($"Page {i}: Extracted");
                    }
                }

                int totalExtracted = endPage - startPage + 1;
                Console.WriteLine();
                Console.WriteLine($"Completed. Extracted {totalExtracted} page(s) to output file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Failed to extract pages: {ex.Message}");

                // Cleanup on failure
                if (File.Exists(outputFile))
                {
                    try
                    {
                        File.Delete(outputFile);
                    }
                    catch { }
                }
            }
        }
    }
}
