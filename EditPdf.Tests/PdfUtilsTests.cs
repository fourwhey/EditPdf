using EditPdf;

namespace EditPdf.Tests;

public class PdfUtilsTests
{
    [Fact]
    public void ValidateNonEmptyPdf_ReturnsFalse_WhenNoPages()
    {
        bool result = PdfUtils.ValidateNonEmptyPdf(0);

        Assert.False(result);
    }

    [Fact]
    public void ValidateNonEmptyPdf_ReturnsTrue_WhenPagesExist()
    {
        bool result = PdfUtils.ValidateNonEmptyPdf(1);

        Assert.True(result);
    }

    [Fact]
    public void ValidatePageNumber_ReturnsFalse_WhenBelowRange()
    {
        bool result = PdfUtils.ValidatePageNumber(0, 10);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePageNumber_ReturnsFalse_WhenAboveRange()
    {
        bool result = PdfUtils.ValidatePageNumber(11, 10);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePageNumber_ReturnsTrue_WhenInsideRange()
    {
        bool result = PdfUtils.ValidatePageNumber(5, 10);

        Assert.True(result);
    }

    [Fact]
    public void ValidatePageRange_ReturnsFalse_WhenStartGreaterThanEnd()
    {
        bool result = PdfUtils.ValidatePageRange(5, 4, 10);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePageRange_ReturnsFalse_WhenEndGreaterThanMax()
    {
        bool result = PdfUtils.ValidatePageRange(1, 11, 10);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePageRange_ReturnsTrue_WhenRangeIsValid()
    {
        bool result = PdfUtils.ValidatePageRange(2, 5, 10);

        Assert.True(result);
    }

    [Fact]
    public void ValidateOutputDirectory_CreatesMissingDirectory()
    {
        string tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        string outputPath = Path.Combine(tempRoot, "nested", "out.pdf");

        try
        {
            bool result = PdfUtils.ValidateOutputDirectory(outputPath);

            Assert.True(result);
            Assert.True(Directory.Exists(Path.GetDirectoryName(outputPath)!));
        }
        finally
        {
            if (Directory.Exists(tempRoot))
            {
                Directory.Delete(tempRoot, recursive: true);
            }
        }
    }

    [Fact]
    public void IsSamePath_ReturnsTrue_ForEquivalentPaths()
    {
        string tempFile = Path.GetTempFileName();

        try
        {
            string equivalentPath = Path.Combine(Path.GetDirectoryName(tempFile)!, ".", Path.GetFileName(tempFile));

            bool result = PdfUtils.IsSamePath(tempFile, equivalentPath);

            Assert.True(result);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void GetPageRange_ReadsValidRangeFromConsoleInput()
    {
        TextReader originalInput = Console.In;

        try
        {
            Console.SetIn(new StringReader("2\n4\n"));

            PageRange range = PdfUtils.GetPageRange(1, 10);

            Assert.Equal(2, range.StartPage);
            Assert.Equal(4, range.EndPage);
        }
        finally
        {
            Console.SetIn(originalInput);
        }
    }

    [Fact]
    public void GetPageRange_RePromptsUntilValidInput()
    {
        TextReader originalInput = Console.In;

        try
        {
            Console.SetIn(new StringReader("0\nabc\n3\n2\n9\n5\n"));

            PageRange range = PdfUtils.GetPageRange(1, 8);

            Assert.Equal(3, range.StartPage);
            Assert.Equal(5, range.EndPage);
        }
        finally
        {
            Console.SetIn(originalInput);
        }
    }
}
