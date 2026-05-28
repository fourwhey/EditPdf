using EditPdf.Interface;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.Kernel.Colors;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class WatermarkHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            Console.Clear();
            Console.WriteLine("Add text watermark to pages.");
            Console.WriteLine();

            Console.Write("Watermark text: ");
            string? watermarkText = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(watermarkText))
            {
                Console.WriteLine("No watermark text provided.");
                return;
            }

            PageRange range = GetPageRange(1, maxPage);
            int startPage = range.StartPage ?? 1;
            int endPage = range.EndPage ?? maxPage;

            // Validate the range
            if (!ValidatePageRange(startPage, endPage, maxPage))
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Opacity (0-100) [default: 30]:");
            float opacity = 0.3f;
            if (int.TryParse(Console.ReadLine(), out int opacityPercent) && opacityPercent >= 0 && opacityPercent <= 100)
            {
                opacity = opacityPercent / 100f;
            }

            Console.WriteLine();
            Console.WriteLine("Position:");
            Console.WriteLine("  1. Diagonal (top-left to bottom-right)");
            Console.WriteLine("  2. Center");
            Console.WriteLine("  3. Top");
            Console.WriteLine("  4. Bottom");
            Console.Write("Select position (1-4) [default: 1]: ");

            string? posChoice = Console.ReadLine();
            int position = posChoice switch
            {
                "2" => 2,
                "3" => 3,
                "4" => 4,
                _ => 1
            };

            Console.WriteLine();

            try
            {
                for (int i = startPage; i <= endPage; i++)
                {
                    var page = doc.GetPage(i);
                    Rectangle pageSize = page.GetPageSize();

                    var canvas = new PdfCanvas(page);

                    // Calculate position
                    float centerX = pageSize.GetWidth() / 2;
                    float centerY = pageSize.GetHeight() / 2;

                    // Add watermark based on position
                    string pageNumText = watermarkText;

                    canvas.SetFillColor(ColorConstants.LIGHT_GRAY)
                        .BeginText()
                        .SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(), 48);

                    switch (position)
                    {
                        case 2: // Center
                            canvas.SetTextMatrix(1, 0, 0, 1, centerX - watermarkText.Length * 12, centerY)
                                .ShowText(watermarkText);
                            break;
                        case 3: // Top
                            canvas.SetTextMatrix(1, 0, 0, 1, centerX - watermarkText.Length * 12, pageSize.GetTop() - 100)
                                .ShowText(watermarkText);
                            break;
                        case 4: // Bottom
                            canvas.SetTextMatrix(1, 0, 0, 1, centerX - watermarkText.Length * 12, 100)
                                .ShowText(watermarkText);
                            break;
                        default: // Diagonal
                            canvas.SetTextMatrix(0.5f, 0.5f, -0.5f, 0.5f, centerX - 100, centerY)
                                .ShowText(watermarkText);
                            break;
                    }

                    canvas.EndText();

                    Console.WriteLine($"Page {i}: Watermark added");
                }

                Console.WriteLine();
                Console.WriteLine("Completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Failed to add watermark: {ex.Message}");
            }
        }
    }
}
