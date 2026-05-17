using EditPdf;

using iText.Kernel.Pdf;

//string? inputFile = "";
//string? outputFile = "";
//string? action = "";

//if (args.Length > 0)
//{
//    inputFile = Path.GetFullPath(args[0]);
//    if (!File.Exists(inputFile))
//    {
//        Console.WriteLine($"File not found: \"{inputFile}\"");
//        return;
//    }

//    Console.WriteLine($"Input file: \"{inputFile}\"");

//    outputFile = $"{inputFile}_edited.pdf";
//    if (File.Exists(outputFile))
//    {
//        Console.WriteLine($"Output file already exists: \"{outputFile}\"");
//        Console.Write("Overwrite existing? (Y/n): ");

//        string overwrite = Console.ReadLine() ?? "Y";
//        overwrite = string.IsNullOrWhiteSpace(overwrite) ? "Y" : overwrite;
//        if (!overwrite.Equals("y", StringComparison.InvariantCultureIgnoreCase))
//        {
//            Console.WriteLine("Operation cancelled.");
//            return;
//        }
//    }

//    Console.Clear();
//    Console.WriteLine($"Input  file: \"{inputFile}\"");
//    Console.WriteLine($"Output file: \"{outputFile}\"");
//}

//if (args.Length > 1)
//{
//    action = args[1];
//}

//using PdfDocument pdfDoc = new(new PdfReader(inputFile), new PdfWriter(outputFile));
//var maxPage = pdfDoc.GetNumberOfPages();

//if (action.Contains("pages", StringComparison.InvariantCultureIgnoreCase))
//{
//    Console.Clear();
//    Console.WriteLine($"Input  file: \"{inputFile}\"");
//    Console.WriteLine($"Output file: \"{outputFile}\"");
//    Console.Write("Enter start ");
//    int startPage = GetPageNumber(1, maxPage);

//    if (action.Equals("deletepages", StringComparison.CurrentCultureIgnoreCase))
//    {
//        Console.Write("Enter end ");
//        int endPage = GetPageNumber(startPage, maxPage);

//        Console.WriteLine("Delete pages in the document.");
//        Console.WriteLine();

//        for (int i = endPage; i >= startPage; i--)
//        {
//            pdfDoc.RemovePage(i);
//            Console.WriteLine($"Page {i}: Deleted");

//        }
//    }
//    else if (action.Equals("insertPages", StringComparison.InvariantCultureIgnoreCase))
//    {
//        Console.Clear();
//        Console.WriteLine("Insert pages in the document.");
//        Console.WriteLine();
//        Console.WriteLine($"Input  file: \"{inputFile}\"");
//        Console.WriteLine($"Output file: \"{outputFile}\"");
//        Console.Write("Enter file to insert from: ");
//        string? insertFile = Console.ReadLine();

//        if (insertFile != null && File.Exists(insertFile))
//        {
//            using PdfDocument insertDoc = new(new PdfReader(insertFile));

//            var insertMaxPage = insertDoc.GetNumberOfPages();

//            Console.Write("Enter insert start ");
//            var insertStart = GetPageNumber(1, insertMaxPage);

//            Console.Write("Enter insert end ");
//            var insertEnd = GetPageNumber(insertStart, insertMaxPage);

//            for (int i = 1; i <= insertMaxPage; i++)
//            {
//                pdfDoc.AddPage(startPage, insertDoc.GetPage(i));
//            }
//        }
//    }
//    else if (action.Equals("rotatepages", StringComparison.CurrentCultureIgnoreCase))
//    {
//        Console.Write("Enter end ");
//        int endPage = GetPageNumber(startPage, maxPage);

//        Console.Clear();
//        Console.WriteLine("Rotate pages in the document.");
//        Console.WriteLine();
//        Console.WriteLine($"Input  file: \"{inputFile}\"");
//        Console.WriteLine($"Output file: \"{outputFile}\"");
//        Console.WriteLine("Valid rotation degrees are:");
//        Console.WriteLine("Rotate Right:  90,  180,  270");
//        Console.WriteLine("Rotate Left : -90, -180, -270");
//        Console.Write("Enter rotation degrees: ");
//        int rotationDegrees;
//        while (!int.TryParse(Console.ReadLine(), out rotationDegrees) || !(new[] { 90, 180, 270 }).Contains(Math.Abs(rotationDegrees)))
//        {
//            Console.Clear();
//            Console.WriteLine($"Input  file: \"{inputFile}\"");
//            Console.WriteLine($"Output file: \"{outputFile}\"");
//            Console.WriteLine("Valid rotation degrees are:");
//            Console.WriteLine("Rotate Right:  90,  180,  270");
//            Console.WriteLine("Rotate Left : -90, -180, -270");
//            Console.Write("Please enter a valid rotation degrees: ");
//        }

//        for (int i = startPage; i <= endPage; i++)
//        {
//            var page = pdfDoc.GetPage(i);
//            page.SetRotation(page.GetRotation() + rotationDegrees);
//            Console.WriteLine($"Page {i}: Rotated {rotationDegrees} degrees");
//        }
//    }
//    else
//    {
//        ShowUsage();
//    }
//    Console.WriteLine("Completed.");
//}

//else if (action.Equals("movepage", StringComparison.InvariantCultureIgnoreCase))
//{
//    Console.Clear();
//    Console.WriteLine("Move a page in the document.");
//    Console.WriteLine();
//    Console.WriteLine($"Input  file: \"{inputFile}\"");
//    Console.WriteLine($"Output file: \"{outputFile}\"");
//    Console.WriteLine($"Page Number(s): 1 to {maxPage}");
//    Console.Write("Enter page number to move: ");
//    string? num = Console.ReadLine();
//    Console.Write("Enter new number: ");
//    //if (!string.IsNullOrWhiteSpace(newOrder))
//    //{
//    //    //var pageNumbers = newOrder.Split(',',
//    //    //    StringSplitOptions.TrimEntries |
//    //    //    StringSplitOptions.RemoveEmptyEntries)
//    //    //    .Select(s => int.TryParse(s, out int num) ? num : (int?)null)  // return ints, return null for everything else
//    //    //    .Where(num => num.HasValue)  // Remove null values
//    //    //    .Select(num => num.Value)    // Extract int values
//    //    //    .OrderByDescending(x => x)
//    //    //    .ToArray();

//    //        pdfDoc.MovePage(pageNumbers[i], i + 1);
//    //}
//}
//else
//{
//    ShowUsage();
//}

if (args.Length < 2)
{
    ShowUsage();
    return;
}

string inputFile = Path.GetFullPath(args[0]);
string action = args[1];

if (!File.Exists(inputFile))
{
    Console.WriteLine($"File not found: {inputFile}");
    return;
}

string outputFile = Path.Combine(
    Path.GetDirectoryName(inputFile) ?? "",
    $"{Path.GetFileNameWithoutExtension(inputFile)}.edited.pdf");
// Ask to overwrite if exists...

using PdfDocument pdfDoc = new(new PdfReader(inputFile), new PdfWriter(outputFile));
int maxPage = pdfDoc.GetNumberOfPages();

var handler = PdfCommandRegistry.GetHandler(action);
if (handler != null)
{
    handler.Execute(pdfDoc, inputFile, outputFile, maxPage);
}
else
{
    ShowUsage();
}


void ShowUsage()
{
    Console.WriteLine("No action specified. Please specify an action.");
    Console.WriteLine("Usage: EditPdf.exe <inputFile> <action>");
    Console.WriteLine("  deletePages      (Saves input document to output document after deleting the inputted page range)");
    Console.WriteLine("  insertPages      (Inserts pages from one document or blank pages into a new document)");
    Console.WriteLine("  rotatePages      (Rotates inputted pages in the page range by a given angle: 90, 180, or 270 degrees)");
    Console.WriteLine("  movePage         (Move a page in the document from page X to Y)");
    Console.WriteLine("  mergeDocuments   (Combines many documents into one document)");
    Console.WriteLine("  splitDocument    (Splits the one document to many documents from page ranges)");

    return;
}

