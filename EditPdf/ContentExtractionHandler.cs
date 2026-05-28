using EditPdf.Interface;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class ContentExtractionHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            Console.Clear();
            Console.WriteLine("Extract content from PDF.");
            Console.WriteLine();

            PageRange range = GetPageRange(1, maxPage);
            int startPage = range.StartPage ?? 1;
            int endPage = range.EndPage ?? maxPage;

            // Validate the range
            if (!ValidatePageRange(startPage, endPage, maxPage))
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Extract format:");
            Console.WriteLine("  1. Text only");
            Console.WriteLine("  2. Images only");
            Console.WriteLine("  3. Both text and images");
            Console.Write("Select format (1-3) [default: 1]: ");

            string? formatChoice = Console.ReadLine();
            int format = formatChoice switch
            {
                "2" => 2,
                "3" => 3,
                _ => 1
            };

            try
            {
                string outputDir = Path.GetDirectoryName(outputFile) ?? "";
                string baseName = Path.GetFileNameWithoutExtension(outputFile);

                Console.WriteLine();

                if (format == 1 || format == 3)
                {
                    // Extract text
                    string textFile = Path.Combine(outputDir, $"{baseName}_text.txt");
                    ExtractText(doc, startPage, endPage, textFile);
                }

                if (format == 2 || format == 3)
                {
                    // Extract images
                    string imageDir = Path.Combine(outputDir, $"{baseName}_images");
                    ExtractImages(doc, startPage, endPage, imageDir);
                }

                Console.WriteLine();
                Console.WriteLine("Completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Failed to extract content: {ex.Message}");
            }
        }

        private void ExtractText(PdfDocument doc, int startPage, int endPage, string outputFile)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    for (int i = startPage; i <= endPage; i++)
                    {
                        var page = doc.GetPage(i);
                        var strategy = new SimpleTextExtractionStrategy();
                        var text = PdfTextExtractor.GetTextFromPage(page, strategy);

                        writer.WriteLine($"--- Page {i} ---");
                        writer.WriteLine(text);
                        writer.WriteLine();

                        Console.WriteLine($"Page {i}: Text extracted");
                    }
                }

                Console.WriteLine($"Text saved to: {Path.GetFileName(outputFile)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting text: {ex.Message}");
            }
        }

        private void ExtractImages(PdfDocument doc, int startPage, int endPage, string outputDir)
        {
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                int imageCount = 0;

                for (int i = startPage; i <= endPage; i++)
                {
                    var page = doc.GetPage(i);
                    var resources = page.GetResources();

                    if (resources != null && resources.GetResourceNames(iText.Kernel.Pdf.PdfName.XObject) != null)
                    {
                        var xObjects = resources.GetResourceNames(iText.Kernel.Pdf.PdfName.XObject);

                        foreach (var xObjectName in xObjects)
                        {
                            var xObject = resources.GetResourceObject(iText.Kernel.Pdf.PdfName.XObject, xObjectName);

                            if (xObject != null && xObject.IsStream())
                            {
                                var stream = (iText.Kernel.Pdf.PdfStream)xObject;
                                byte[] imageBytes = stream.GetBytes();

                                // Save image with generic name
                                string imagePath = Path.Combine(outputDir, $"image_page{i}_{imageCount}.jpg");
                                File.WriteAllBytes(imagePath, imageBytes);
                                imageCount++;
                            }
                        }
                    }

                    if (imageCount > 0)
                    {
                        Console.WriteLine($"Page {i}: {imageCount} image(s) extracted");
                    }
                }

                if (imageCount > 0)
                {
                    Console.WriteLine($"Images saved to: {Path.GetFileName(outputDir)}");
                }
                else
                {
                    Console.WriteLine("No images found in specified pages.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting images: {ex.Message}");
            }
        }
    }
}
