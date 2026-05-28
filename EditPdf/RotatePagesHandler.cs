using EditPdf.Interface;

using iText.Kernel.Pdf;

using static EditPdf.PdfUtils;

namespace EditPdf
{
    public class RotatePagesHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            Console.Clear();
            Console.WriteLine("Rotate pages in the document.");
            PageRange range = GetPageRange(1, maxPage);

            Console.WriteLine();
            Console.WriteLine("Valid rotation degrees are:");
            Console.WriteLine("  90   (Rotate Right)");
            Console.WriteLine("  180  (Rotate 180°)");
            Console.WriteLine("  270  (Rotate Left)");
            Console.WriteLine("  -90  (Rotate Left)");
            Console.WriteLine("  -180 (Rotate 180°)");
            Console.WriteLine("  -270 (Rotate Right)");
            Console.Write("Enter rotation degrees: ");

            int rotationDegrees;
            while (!int.TryParse(Console.ReadLine(), out rotationDegrees) || 
                   !(new[] { 90, 180, 270, -90, -180, -270 }).Contains(rotationDegrees))
            {
                Console.Write("Invalid rotation. Please enter 90, 180, 270, -90, -180, or -270: ");
            }

            int startPage = range.StartPage ?? 1;
            int endPage = range.EndPage ?? maxPage;

            // Validate the range
            if (!ValidatePageRange(startPage, endPage, maxPage))
            {
                return;
            }

            Console.WriteLine();
            for (int i = startPage; i <= endPage; i++)
            {
                var page = doc.GetPage(i);
                int currentRotation = page.GetRotation();
                page.SetRotation(currentRotation + rotationDegrees);
                Console.WriteLine($"Page {i}: Rotated {rotationDegrees} degrees");
            }

            Console.WriteLine("Completed.");
        }
    }

}
