using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditPdf
{
    internal static class PdfUtils
    {
        /// <summary>
        /// Validates that a PDF has at least one page.
        /// </summary>
        public static bool ValidateNonEmptyPdf(int maxPage)
        {
            if (maxPage < 1)
            {
                Console.WriteLine("Error: PDF contains no pages. Operation cancelled.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a page number is within valid bounds.
        /// </summary>
        public static bool ValidatePageNumber(int pageNumber, int maxPage, string context = "Page")
        {
            if (pageNumber < 1 || pageNumber > maxPage)
            {
                Console.WriteLine($"Error: {context} number must be between 1 and {maxPage}. Got: {pageNumber}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a page range is valid (start and end bounds).
        /// </summary>
        public static bool ValidatePageRange(int startPage, int endPage, int maxPage)
        {
            if (startPage < 1)
            {
                Console.WriteLine($"Error: Start page must be >= 1. Got: {startPage}");
                return false;
            }
            if (endPage > maxPage)
            {
                Console.WriteLine($"Error: End page must be <= {maxPage}. Got: {endPage}");
                return false;
            }
            if (startPage > endPage)
            {
                Console.WriteLine($"Error: Start page ({startPage}) cannot be greater than end page ({endPage})");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that the output directory exists or can be created.
        /// </summary>
        public static bool ValidateOutputDirectory(string outputFilePath)
        {
            try
            {
                string? directory = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Cannot create output directory: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if input and output paths are the same (case-insensitive).
        /// </summary>
        public static bool IsSamePath(string path1, string path2)
        {
            string fullPath1 = Path.GetFullPath(path1);
            string fullPath2 = Path.GetFullPath(path2);
            return fullPath1.Equals(fullPath2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static PageRange GetPageRange(int minPage, int maxPage)
        {
            PageRange range = new();

            Console.WriteLine($"Page Number(s): {minPage} to {maxPage}");

            // Get Start Page
            while (true)
            {
                Console.Write("Please enter start page number: ");
                if (int.TryParse(Console.ReadLine(), out int start) && start >= minPage && start <= maxPage)
                {
                    range.StartPage = start;
                    break;
                }
                Console.WriteLine("Invalid start page number.");
            }

            // Get End Page
            while (true)
            {
                Console.Write("Please enter end page number: ");
                if (int.TryParse(Console.ReadLine(), out int end) && end >= range.StartPage && end <= maxPage)
                {
                    range.EndPage = end;
                    break;
                }
                Console.WriteLine($"Invalid end page number. Must be >= {range.StartPage} and <= {maxPage}.");
            }

            return range;
        }
    }

    public class PageRange
    {
        public int? StartPage { get; set; }
        public int? EndPage { get; set; }
    }
}
