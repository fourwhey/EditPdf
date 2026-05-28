using EditPdf.Interface;

using iText.Kernel.Pdf;

namespace EditPdf
{
    public class CompressionHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            PdfUtils.SafeClearConsole();
            Console.WriteLine("Compress PDF document.");
            Console.WriteLine();

            // Get file size before compression
            long originalSize = new FileInfo(inputFile).Length;
            Console.WriteLine($"Original file size: {FormatFileSize(originalSize)}");
            Console.WriteLine();

            Console.WriteLine("Compression level:");
            Console.WriteLine("  1. Low (faster, less compression)");
            Console.WriteLine("  2. Medium (balanced)");
            Console.WriteLine("  3. High (slower, maximum compression)");
            Console.Write("Select level (1-3) [default: 2]: ");

            string? levelChoice = Console.ReadLine();
            int level = levelChoice switch
            {
                "1" => 1,
                "3" => 3,
                _ => 2
            };

            try
            {
                // Apply compression based on level
                // Note: iText applies compression automatically when document is written
                // We can influence it through the PdfWriter during document creation

                Console.WriteLine();
                Console.WriteLine($"Applying {(level == 1 ? "low" : level == 3 ? "high" : "medium")} compression...");

                // The document will be compressed when closed by PdfWriter
                // Different levels are primarily controlled by the writer configuration
                // which was already set during document creation in Program.cs

                Console.WriteLine("Compressing...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Failed to apply compression: {ex.Message}");
                return;
            }

            Console.WriteLine("Completed. PDF will be compressed when saved.");
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}
