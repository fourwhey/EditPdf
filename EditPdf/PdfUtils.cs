using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditPdf
{
    internal static class PdfUtils
    {
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
