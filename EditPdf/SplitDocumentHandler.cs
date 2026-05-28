using EditPdf.Interface;

using iText.Kernel.Pdf;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class SplitDocumentHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            Console.Clear();
            Console.WriteLine("Split document into multiple documents.");
            Console.WriteLine($"Source document: {inputFile} ({maxPage} pages)");
            Console.WriteLine();

            Console.WriteLine("Choose split method:");
            Console.WriteLine("  1. Split by page ranges");
            Console.WriteLine("  2. Split every N pages");
            Console.WriteLine("  3. Split into N equal parts");
            Console.Write("Enter choice (1-3): ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SplitByRanges(inputFile, maxPage);
                    break;
                case "2":
                    SplitByInterval(inputFile, maxPage);
                    break;
                case "3":
                    SplitIntoParts(inputFile, maxPage);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }
        }

        private void SplitByRanges(string inputFile, int maxPage)
        {
            Console.WriteLine();
            Console.WriteLine("Split by page ranges (e.g., 1-5, 6-10, 11-15)");

            List<PageRange> ranges = new();
            int rangeNumber = 1;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"Range #{rangeNumber} (or press Enter for start page to finish):");

                PageRange range = GetPageRange(1, maxPage);

                if (range.StartPage == null || range.EndPage == null)
                {
                    break;
                }

                int startPage = range.StartPage.Value;
                int endPage = range.EndPage.Value;

                // Validate range
                if (!ValidatePageRange(startPage, endPage, maxPage))
                {
                    continue;
                }

                // Check for overlaps with existing ranges
                bool overlaps = ranges.Any(r =>
                {
                    int rStart = r.StartPage ?? 1;
                    int rEnd = r.EndPage ?? maxPage;
                    return !(endPage < rStart || startPage > rEnd);
                });

                if (overlaps)
                {
                    Console.WriteLine("Error: This range overlaps with an existing range.");
                    continue;
                }

                ranges.Add(range);
                rangeNumber++;

                Console.Write("Add another range? (y/n): ");
                string? response = Console.ReadLine();
                if (response?.ToLower() != "y")
                {
                    break;
                }
            }

            if (ranges.Count == 0)
            {
                Console.WriteLine("No ranges specified.");
                return;
            }

            Console.WriteLine();
            CreateSplitDocuments(inputFile, ranges);
        }

        private void SplitByInterval(string inputFile, int maxPage)
        {
            Console.WriteLine();
            Console.Write("Enter number of pages per split: ");

            if (!int.TryParse(Console.ReadLine(), out int pagesPerSplit) || pagesPerSplit < 1)
            {
                Console.WriteLine("Error: Please enter a number greater than 0.");
                return;
            }

            if (pagesPerSplit > maxPage)
            {
                Console.WriteLine($"Error: Pages per split ({pagesPerSplit}) cannot exceed total pages ({maxPage}).");
                return;
            }

            List<PageRange> ranges = new();
            for (int start = 1; start <= maxPage; start += pagesPerSplit)
            {
                int end = Math.Min(start + pagesPerSplit - 1, maxPage);
                ranges.Add(new PageRange { StartPage = start, EndPage = end });
            }

            Console.WriteLine();
            Console.WriteLine($"Will create {ranges.Count} document(s):");
            for (int i = 0; i < ranges.Count; i++)
            {
                Console.WriteLine($"  Part {i + 1}: Pages {ranges[i].StartPage}-{ranges[i].EndPage}");
            }

            Console.WriteLine();            Console.WriteLine();
            CreateSplitDocuments(inputFile, ranges);
        }

        private void SplitIntoParts(string inputFile, int maxPage)
        {
            Console.WriteLine();
            Console.Write("Enter number of parts to split into: ");

            if (!int.TryParse(Console.ReadLine(), out int parts) || parts < 2)
            {
                Console.WriteLine("Error: Please enter a number greater than or equal to 2.");
                return;
            }

            if (parts > maxPage)
            {
                Console.WriteLine($"Error: Cannot split into {parts} parts when document has only {maxPage} page(s).");
                return;
            }

            int pagesPerPart = maxPage / parts;
            int remainder = maxPage % parts;

            List<PageRange> ranges = new();
            int currentPage = 1;

            for (int i = 0; i < parts; i++)
            {
                int pagesInThisPart = pagesPerPart + (i < remainder ? 1 : 0);
                int start = currentPage;
                int end = currentPage + pagesInThisPart - 1;

                ranges.Add(new PageRange { StartPage = start, EndPage = end });
                currentPage = end + 1;
            }

            Console.WriteLine();
            Console.WriteLine($"Will create {ranges.Count} document(s):");
            for (int i = 0; i < ranges.Count; i++)
            {
                Console.WriteLine($"  Part {i + 1}: Pages {ranges[i].StartPage}-{ranges[i].EndPage}");
            }

            Console.WriteLine();            Console.WriteLine();
            CreateSplitDocuments(inputFile, ranges);
        }

        private void CreateSplitDocuments(string inputFile, List<PageRange> ranges)
        {
            string baseFileName = Path.GetFileNameWithoutExtension(inputFile);
            string directory = Path.GetDirectoryName(inputFile) ?? "";

            // Validate output directory
            if (!ValidateOutputDirectory(Path.Combine(directory, baseFileName)))
            {
                return;
            }

            try
            {
                // Open the source document separately for splitting
                using (PdfDocument sourceDoc = new(new PdfReader(inputFile)))
                {
                    int successCount = 0;

                    for (int i = 0; i < ranges.Count; i++)
                    {
                        string outputFileName = Path.Combine(directory, $"{baseFileName}.part{i + 1}.pdf");

                        try
                        {
                            using (PdfDocument targetDoc = new(new PdfWriter(outputFileName)))
                            {
                                int startPage = ranges[i].StartPage ?? 1;
                                int endPage = ranges[i].EndPage ?? sourceDoc.GetNumberOfPages();

                                // Validate range one more time
                                if (!ValidatePageRange(startPage, endPage, sourceDoc.GetNumberOfPages()))
                                {
                                    continue;
                                }

                                for (int pageNum = startPage; pageNum <= endPage; pageNum++)
                                {
                                    var page = sourceDoc.GetPage(pageNum).CopyTo(targetDoc);
                                    targetDoc.AddPage(page);
                                }
                            }

                            Console.WriteLine($"  Created: {Path.GetFileName(outputFileName)} (pages {ranges[i].StartPage}-{ranges[i].EndPage})");
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"  Error creating {outputFileName}: {ex.Message}");
                            // Attempt cleanup
                            try
                            {
                                if (File.Exists(outputFileName))
                                {
                                    File.Delete(outputFileName);
                                }
                            }
                            catch
                            {
                                // Silently ignore cleanup errors
                            }
                        }
                    }

                    Console.WriteLine();
                    if (successCount == 0)
                    {
                        Console.WriteLine("Error: No parts were successfully created.");
                    }
                    else
                    {
                        Console.WriteLine($"Completed. Created {successCount}/{ranges.Count} document(s).");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Failed to split document: {ex.Message}");
            }
        }
    }
}
