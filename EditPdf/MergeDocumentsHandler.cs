using EditPdf.Interface;

using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class MergeDocumentsHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            PdfUtils.SafeClearConsole();
            Console.WriteLine("Merge multiple documents into one document.");
            Console.WriteLine($"Base document: {inputFile} ({maxPage} pages)");
            Console.WriteLine();

            List<string> filesToMerge = new();

            Console.WriteLine("Enter file paths to merge (one per line, empty line to finish):");
            while (true)
            {
                Console.Write($"File #{filesToMerge.Count + 1} (or press Enter to finish): ");
                string? file = Console.ReadLine()?.Trim('"');

                if (string.IsNullOrWhiteSpace(file))
                {
                    break;
                }

                if (!File.Exists(file))
                {
                    Console.WriteLine($"File not found: {file}");
                    continue;
                }

                string fullPath = Path.GetFullPath(file);

                // Prevent self-merge
                if (IsSamePath(inputFile, fullPath))
                {
                    Console.WriteLine("Error: Cannot merge a file into itself.");
                    continue;
                }

                filesToMerge.Add(fullPath);
                Console.WriteLine($"  Added: {Path.GetFileName(fullPath)}");
            }

            if (filesToMerge.Count == 0)
            {
                Console.WriteLine("No files to merge.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Merging {filesToMerge.Count} document(s) into base document...");
            Console.WriteLine();

            int currentPageCount = maxPage;
            int successCount = 0;

            foreach (var file in filesToMerge)
            {
                try
                {
                    using (PdfDocument sourceDoc = new(new PdfReader(file)))
                    {
                        int sourcePageCount = sourceDoc.GetNumberOfPages();

                        // Validate source PDF is not empty
                        if (sourcePageCount < 1)
                        {
                            Console.WriteLine($"  Skipped: {Path.GetFileName(file)} (empty)");
                            continue;
                        }

                        // Copy all pages from source document
                        for (int i = 1; i <= sourcePageCount; i++)
                        {
                            var pageToCopy = sourceDoc.GetPage(i).CopyTo(doc);
                            doc.AddPage(pageToCopy);
                        }

                        Console.WriteLine($"  Merged: {Path.GetFileName(file)} ({sourcePageCount} pages)");
                        currentPageCount += sourcePageCount;
                        successCount++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Error merging {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            Console.WriteLine();
            if (successCount == 0)
            {
                Console.WriteLine("Error: No files were successfully merged.");
                return;
            }

            Console.WriteLine($"Completed. Merged {successCount}/{filesToMerge.Count} files. Total pages: {currentPageCount}");
        }
    }
}
