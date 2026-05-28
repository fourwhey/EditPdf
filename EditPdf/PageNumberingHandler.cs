using EditPdf.Interface;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class PageNumberingHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            PdfUtils.SafeClearConsole();
            Console.WriteLine("Add page numbers to the document.");
            Console.WriteLine();

            Console.WriteLine("Position:");
            Console.WriteLine("  1. Bottom center");
            Console.WriteLine("  2. Bottom right");
            Console.WriteLine("  3. Bottom left");
            Console.WriteLine("  4. Top center");
            Console.Write("Select position (1-4) [default: 1]: ");

            string? posChoice = Console.ReadLine();
            int position = posChoice switch
            {
                "2" => 2,
                "3" => 3,
                "4" => 4,
                _ => 1
            };

            Console.Write("Starting page number [default: 1]: ");
            int startNum = 1;
            if (int.TryParse(Console.ReadLine(), out int parsed))
            {
                startNum = parsed;
            }

            Console.Write("Font size [default: 10]: ");
            float fontSize = 10f;
            if (float.TryParse(Console.ReadLine(), out float parsedSize))
            {
                fontSize = parsedSize;
            }

            Console.WriteLine();

            try
            {
                for (int i = 1; i <= maxPage; i++)
                {
                    var page = doc.GetPage(i);
                    Rectangle pageSize = page.GetPageSize();

                    // Create canvas
                    var canvas = new PdfCanvas(page);

                    // Calculate position
                    float x, y;
                    string pageNumText = (startNum + i - 1).ToString();

                    switch (position)
                    {
                        case 2: // Bottom right
                            x = pageSize.GetRight() - 40;
                            y = 20;
                            break;
                        case 3: // Bottom left
                            x = 20;
                            y = 20;
                            break;
                        case 4: // Top center
                            x = pageSize.GetWidth() / 2 - 10;
                            y = pageSize.GetTop() - 20;
                            break;
                        default: // Bottom center
                            x = pageSize.GetWidth() / 2 - 10;
                            y = 20;
                            break;
                    }

                    // Draw page number
                    canvas.BeginText()
                        .SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(), fontSize)
                        .MoveText(x, y)
                        .ShowText(pageNumText)
                        .EndText();

                    Console.WriteLine($"Page {i}: Added page number {pageNumText}");
                }

                Console.WriteLine();
                Console.WriteLine("Completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Failed to add page numbers: {ex.Message}");
            }
        }
    }
}
