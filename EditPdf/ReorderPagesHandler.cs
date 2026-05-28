using EditPdf.Interface;

using iText.Kernel.Pdf;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class ReorderPagesHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            Console.Clear();
            Console.WriteLine("Reorder pages in the document.");
            Console.WriteLine($"Document has {maxPage} pages.");
            Console.WriteLine();
            Console.WriteLine("Enter the new page order as comma-separated numbers.");
            Console.WriteLine("Example: 3,1,2,5,4 (reorders pages in that sequence)");
            Console.WriteLine();

            Console.Write("New page order: ");
            string? orderInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(orderInput))
            {
                Console.WriteLine("No input provided.");
                return;
            }

            // Parse the input
            int[] newOrder;
            try
            {
                newOrder = orderInput
                    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(s =>
                    {
                        if (!int.TryParse(s, out int num))
                        {
                            throw new ArgumentException($"Invalid page number: {s}");
                        }
                        return num;
                    })
                    .ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing page order: {ex.Message}");
                return;
            }

            // Validate the order
            if (newOrder.Length == 0)
            {
                Console.WriteLine("No page numbers provided.");
                return;
            }

            if (newOrder.Length != maxPage)
            {
                Console.WriteLine($"Error: You must specify exactly {maxPage} page numbers. Got: {newOrder.Length}");
                return;
            }

            // Check for valid page numbers
            foreach (var pageNum in newOrder)
            {
                if (!ValidatePageNumber(pageNum, maxPage, $"Page order value"))
                {
                    return;
                }
            }

            // Check for duplicates
            if (newOrder.Distinct().Count() != newOrder.Length)
            {
                Console.WriteLine("Error: Duplicate page numbers detected in order.");
                return;
            }

            // Apply reordering
            try
            {
                Console.WriteLine();

                // Create a list of pages in new order
                var reorderedPages = new List<PdfPage>();
                foreach (var pageNum in newOrder)
                {
                    reorderedPages.Add(doc.GetPage(pageNum));
                }

                // Remove all pages from document
                for (int i = maxPage; i >= 1; i--)
                {
                    doc.RemovePage(i);
                }

                // Add pages back in new order
                foreach (var page in reorderedPages)
                {
                    doc.AddPage(page);
                }

                Console.WriteLine("Page order changed to: " + string.Join(", ", newOrder));
                Console.WriteLine("Completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reordering pages: {ex.Message}");
            }
        }
    }
}
