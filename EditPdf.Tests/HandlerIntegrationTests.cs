using EditPdf;

using iText.Kernel.Pdf;

namespace EditPdf.Tests;

public class HandlerIntegrationTests
{
    [Fact]
    public void DeletePagesHandler_RemovesSelectedRange()
    {
        using TestPdfContext context = TestPdfContext.Create(basePageCount: 5);

        using (PdfDocument document = new(new PdfReader(context.InputPath), new PdfWriter(context.OutputPath)))
        using (new ConsoleScope("2\n4\n"))
        {
            DeletePagesHandler handler = new();
            handler.Execute(document, context.InputPath, context.OutputPath, document.GetNumberOfPages());
        }

        using PdfDocument output = new(new PdfReader(context.OutputPath));
        Assert.Equal(2, output.GetNumberOfPages());
    }

    [Fact]
    public void RotatePagesHandler_RotatesRequestedPages()
    {
        using TestPdfContext context = TestPdfContext.Create(basePageCount: 3);

        using (PdfDocument document = new(new PdfReader(context.InputPath), new PdfWriter(context.OutputPath)))
        using (new ConsoleScope("1\n2\n90\n"))
        {
            RotatePagesHandler handler = new();
            handler.Execute(document, context.InputPath, context.OutputPath, document.GetNumberOfPages());
        }

        using PdfDocument output = new(new PdfReader(context.OutputPath));
        Assert.Equal(90, output.GetPage(1).GetRotation());
        Assert.Equal(90, output.GetPage(2).GetRotation());
        Assert.Equal(0, output.GetPage(3).GetRotation());
    }

    [Fact]
    public void MetadataHandler_UpdatesProvidedFields()
    {
        using TestPdfContext context = TestPdfContext.Create(basePageCount: 1);

        using (PdfDocument document = new(new PdfReader(context.InputPath), new PdfWriter(context.OutputPath)))
        using (new ConsoleScope("Test Title\nTest Author\n\nalpha,beta\nEditPdf Tests\n"))
        {
            MetadataHandler handler = new();
            handler.Execute(document, context.InputPath, context.OutputPath, document.GetNumberOfPages());
        }

        using PdfDocument output = new(new PdfReader(context.OutputPath));
        PdfDocumentInfo info = output.GetDocumentInfo();

        Assert.Equal("Test Title", info.GetTitle());
        Assert.Equal("Test Author", info.GetAuthor());
        Assert.Equal("alpha,beta", info.GetKeywords());
        Assert.Equal("EditPdf Tests", info.GetCreator());
    }

    [Fact]
    public void MergeDocumentsHandler_AppendsAllProvidedFiles()
    {
        using TestPdfContext context = TestPdfContext.Create(basePageCount: 1);
        string mergeSourceOne = Path.Combine(context.WorkingDirectory, "source1.pdf");
        string mergeSourceTwo = Path.Combine(context.WorkingDirectory, "source2.pdf");

        TestPdfContext.CreatePdfWithPages(mergeSourceOne, 2);
        TestPdfContext.CreatePdfWithPages(mergeSourceTwo, 1);

        string consoleInput = $"\"{mergeSourceOne}\"\n{mergeSourceTwo}\n\n";

        using (PdfDocument document = new(new PdfReader(context.InputPath), new PdfWriter(context.OutputPath)))
        using (new ConsoleScope(consoleInput))
        {
            MergeDocumentsHandler handler = new();
            handler.Execute(document, context.InputPath, context.OutputPath, document.GetNumberOfPages());
        }

        using PdfDocument output = new(new PdfReader(context.OutputPath));
        Assert.Equal(4, output.GetNumberOfPages());
    }

    private sealed class ConsoleScope : IDisposable
    {
        private readonly TextReader _originalInput;
        private readonly TextWriter _originalOutput;

        public ConsoleScope(string input)
        {
            _originalInput = Console.In;
            _originalOutput = Console.Out;

            Console.SetIn(new StringReader(input));
            Console.SetOut(TextWriter.Null);
        }

        public void Dispose()
        {
            Console.SetIn(_originalInput);
            Console.SetOut(_originalOutput);
        }
    }

    private sealed class TestPdfContext : IDisposable
    {
        public string WorkingDirectory { get; }
        public string InputPath { get; }
        public string OutputPath { get; }

        private TestPdfContext(string workingDirectory, string inputPath, string outputPath)
        {
            WorkingDirectory = workingDirectory;
            InputPath = inputPath;
            OutputPath = outputPath;
        }

        public static TestPdfContext Create(int basePageCount)
        {
            string workingDirectory = Path.Combine(Path.GetTempPath(), "EditPdf.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(workingDirectory);

            string inputPath = Path.Combine(workingDirectory, "input.pdf");
            string outputPath = Path.Combine(workingDirectory, "output.pdf");

            CreatePdfWithPages(inputPath, basePageCount);

            return new TestPdfContext(workingDirectory, inputPath, outputPath);
        }

        public static void CreatePdfWithPages(string path, int pageCount)
        {
            using PdfDocument document = new(new PdfWriter(path));
            for (int i = 0; i < pageCount; i++)
            {
                document.AddNewPage();
            }
        }

        public void Dispose()
        {
            if (Directory.Exists(WorkingDirectory))
            {
                Directory.Delete(WorkingDirectory, recursive: true);
            }
        }
    }
}
