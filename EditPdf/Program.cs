using EditPdf;

using iText.Kernel.Pdf;

return await Main(args);

async Task<int> Main(string[] args)
{
  // Exit codes:
  // 0 = Success
  // 1 = General error / exception
  // 2 = Validation error (invalid input, file not found, etc.)
  // 3 = File not found
  // 4 = Output directory creation failed

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

  if (args.Length < 1)
  {
    ShowUsage();
    return 2; // Validation error
  }

  // Parse CLI arguments using the new parser
  var cliArgs = CliArgumentParser.Parse(args);

  // Validate parsed arguments
  if (!cliArgs.IsValid(out string? validationError))
  {
    Console.WriteLine(validationError);
    return 2; // Validation error
  }

  string inputFile = Path.GetFullPath(cliArgs.InputFile!);
  string action = cliArgs.Action!;
  bool silentMode = cliArgs.SilentMode;

  if (!File.Exists(inputFile))
  {
    if (!silentMode) Console.WriteLine($"File not found: {inputFile}");
    return 3; // File not found
  }

  string outputFile;
  if (!string.IsNullOrEmpty(cliArgs.OutputFile))
  {
    // Use provided output path (can be relative or full)
    outputFile = Path.GetFullPath(cliArgs.OutputFile);
  }
  else
  {
    // Default: same directory as input, with .edited suffix
    outputFile = Path.Combine(
        Path.GetDirectoryName(inputFile) ?? "",
        $"{Path.GetFileNameWithoutExtension(inputFile)}.edited.pdf");
  }

  // Validate output directory can be created/exists
  if (!PdfUtils.ValidateOutputDirectory(outputFile))
  {
    if (!silentMode) Console.WriteLine("Error: Could not create or access output directory.");
    return 4; // Output directory creation failed
  }

  // Prevent operations on the same file
  if (PdfUtils.IsSamePath(inputFile, outputFile))
  {
    if (!silentMode) Console.WriteLine("Error: Input and output files must be different.");
    return 2; // Validation error
  }

  // Check if output file already exists
  if (File.Exists(outputFile))
  {
    if (silentMode)
    {
      // In silent mode, auto-confirm overwrite
      if (!silentMode) Console.WriteLine($"Output file already exists: {outputFile}");
    }
    else
    {
      // In interactive mode, ask user
      Console.WriteLine($"Output file already exists: {outputFile}");
      Console.Write("Overwrite? (y/n): ");
      string? response = Console.ReadLine();

      if (response?.Equals("y", StringComparison.InvariantCultureIgnoreCase) != true)
      {
        Console.WriteLine("Operation cancelled.");
        return 2; // Validation error / user cancelled
      }
    }
  }

  try
  {
    // Get input file size before operation
    long inputFileSize = new FileInfo(inputFile).Length;

    // Warn if file is large (> 100MB), unless in silent mode
    const long LARGE_FILE_THRESHOLD = 100L * 1024L * 1024L; // 100MB
    if (!silentMode && inputFileSize > LARGE_FILE_THRESHOLD)
    {
      Console.WriteLine($"⚠️  Warning: This file is {FormatFileSize(inputFileSize)}. Processing may take a while...");
    }

    // Track operation timing
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

    using PdfDocument pdfDoc = new(new PdfReader(inputFile), new PdfWriter(outputFile));
    int maxPage = pdfDoc.GetNumberOfPages();

    // Check if PDF is empty
    if (!PdfUtils.ValidateNonEmptyPdf(maxPage))
    {
      if (!silentMode) Console.WriteLine("Error: PDF is empty.");
      return 2; // Validation error
    }

    var handler = PdfCommandRegistry.GetHandler(action);
    if (handler != null)
    {
      handler.Execute(pdfDoc, inputFile, outputFile, maxPage);

      // Stop timing and get output file size
      stopwatch.Stop();
      long outputFileSize = new FileInfo(outputFile).Length;
      double sizeReduction = inputFileSize > 0 ? (1.0 - (double)outputFileSize / inputFileSize) * 100.0 : 0;

      // Report success with metrics, unless in silent mode
      if (!silentMode)
      {
        string inputSizeStr = FormatFileSize(inputFileSize);
        string outputSizeStr = FormatFileSize(outputFileSize);
        Console.WriteLine();
        Console.WriteLine($"✅ Operation completed in {stopwatch.Elapsed.TotalSeconds:F2}s");
        Console.WriteLine($"   File size: {inputSizeStr} → {outputSizeStr}" +
            (sizeReduction > 0 ? $" ({sizeReduction:F1}% reduction)" :
             sizeReduction < 0 ? $" ({-sizeReduction:F1}% increase)" : ""));
      }

      // Log operation (always, even in silent mode)
      LogOperation(outputFile, action, "Success", stopwatch.Elapsed);

      return 0; // Success
    }
    else
    {
      if (!silentMode) ShowUsage();
      return 2; // Validation error - invalid action
    }
  }
  catch (Exception ex)
  {
    if (!silentMode) Console.WriteLine($"Error processing PDF: {ex.Message}");
    LogOperation(outputFile, action, $"Error: {ex.Message}", System.TimeSpan.Zero);
    if (File.Exists(outputFile))
    {
      try
      {
        File.Delete(outputFile);
      }
      catch
      {
        // Silently ignore cleanup errors
      }
    }
    return 1; // General error
  }
}


string FormatFileSize(long bytes)
{
  string[] sizes = { "B", "KB", "MB", "GB", "TB" };
  double len = bytes;
  int order = 0;
  while (len >= 1024 && order < sizes.Length - 1)
  {
    order++;
    len = len / 1024;
  }
  return $"{len:F1} {sizes[order]}";
}

void LogOperation(string outputFile, string action, string result, System.TimeSpan elapsed)
{
  try
  {
    string logFile = Path.Combine(
        Path.GetDirectoryName(outputFile) ?? "",
        "EditPdf.log");

    string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    string logEntry = $"[{timestamp}] {action.ToUpper()} - {result}" +
        (elapsed.TotalMilliseconds > 0 ? $" ({elapsed.TotalSeconds:F2}s)" : "");

    File.AppendAllText(logFile, logEntry + Environment.NewLine);
  }
  catch
  {
    // Silently ignore logging errors
  }
}

void ShowUsage()
{
  Console.WriteLine("EditPdf - PDF manipulation tool");
  Console.WriteLine();
  Console.WriteLine("Usage (Positional):");
  Console.WriteLine("  EditPdf <inputFile> <action> [outputFile] [--silent]");
  Console.WriteLine();
  Console.WriteLine("Usage (Named Arguments - CLI mode):");
  Console.WriteLine("  EditPdf --input <file> <action> [--output <file>] [--silent] [operation options]");
  Console.WriteLine();
  Console.WriteLine("Positional Arguments:");
  Console.WriteLine("  inputFile    - Path to input PDF (relative or full path)");
  Console.WriteLine("  action       - Operation to perform (see below)");
  Console.WriteLine("  outputFile   - Path for output PDF (optional, relative or full path)");
  Console.WriteLine("               - If not specified, defaults to: {input}.edited.pdf");
  Console.WriteLine();
  Console.WriteLine("Global Options:");
  Console.WriteLine("  --input <file>    - Path to input PDF (named argument form)");
  Console.WriteLine("  --output <file>   - Path for output PDF (optional)");
  Console.WriteLine("  --silent          - Run silently (no prompts, auto-confirm overwrites)");
  Console.WriteLine();
  Console.WriteLine("PDF Operations:");
  Console.WriteLine();
  Console.WriteLine("MODIFY PAGES:");
  Console.WriteLine("  deletePages      - Delete a page range");
  Console.WriteLine("                     CLI: --start <n> --end <n>");
  Console.WriteLine("  insertPages      - Insert pages from another PDF");
  Console.WriteLine("  blankPage        - Insert blank pages");
  Console.WriteLine("                     CLI: --position <n> --count <n> --size <A4|Letter|A3|Legal>");
  Console.WriteLine("  duplicatePages   - Duplicate a page range N times");
  Console.WriteLine("                     CLI: --start <n> --end <n> --count <1-10>");
  Console.WriteLine("  rotatePages      - Rotate pages by 90°, 180°, or 270°");
  Console.WriteLine("                     CLI: --start <n> --end <n> --degrees <90|180|270>");
  Console.WriteLine("  movePage         - Move a page to a new position");
  Console.WriteLine("                     CLI: --page <n> --position <n>");
  Console.WriteLine("  reorderPages     - Reorder pages via comma-separated list");
  Console.WriteLine("                     CLI: --order <3,1,2>");
  Console.WriteLine();
  Console.WriteLine("DOCUMENT OPERATIONS:");
  Console.WriteLine("  mergeDocuments   - Merge multiple PDFs into one");
  Console.WriteLine("  splitDocument    - Split PDF into multiple documents");
  Console.WriteLine("  extractPages     - Extract page range to new PDF");
  Console.WriteLine("                     CLI: --start <n> --end <n>");
  Console.WriteLine("  compress         - Compress PDF with configurable level");
  Console.WriteLine("                     CLI: --level <low|medium|high>");
  Console.WriteLine();
  Console.WriteLine("ADD CONTENT:");
  Console.WriteLine("  pageNumbering    - Add page numbers to pages");
  Console.WriteLine("                     CLI: --start <n> --end <n> --position <pos> --font-size <n>");
  Console.WriteLine("  watermark        - Add text watermark to pages");
  Console.WriteLine("                     CLI: --text <text> --start <n> --end <n> --position <pos> --opacity <0-100>");
  Console.WriteLine();
  Console.WriteLine("METADATA & EXTRACTION:");
  Console.WriteLine("  editMetadata     - Edit PDF metadata (Title, Author, Subject, Keywords)");
  Console.WriteLine("                     CLI: --title <str> --author <str> --subject <str> --keywords <str>");
  Console.WriteLine("  extractContent   - Extract text and/or images from pages");
  Console.WriteLine("                     CLI: --start <n> --end <n> --type <text|images|both>");
  Console.WriteLine();
  Console.WriteLine("Examples (Positional/Interactive):");
  Console.WriteLine("  EditPdf input.pdf deletePages output.pdf              - Interactive prompts for pages");
  Console.WriteLine("  EditPdf input.pdf compress --silent                   - Compress with no prompts");
  Console.WriteLine("  EditPdf input.pdf mergeDocuments                      - Merge (interactive)");
  Console.WriteLine();
  Console.WriteLine("Examples (Named Arguments/CLI Mode):");
  Console.WriteLine("  EditPdf --input in.pdf deletePages --start 1 --end 5 --output out.pdf");
  Console.WriteLine("  EditPdf --input in.pdf compress --level high --output out.pdf --silent");
  Console.WriteLine("  EditPdf --input in.pdf editMetadata --title \"New Title\" --author \"John\"");

  return;
}

