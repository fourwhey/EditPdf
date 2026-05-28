using EditPdf;
using EditPdf.Interface;

namespace EditPdf.Tests;

public class PdfCommandRegistryTests
{
    [Theory]
    [InlineData("deletepages", typeof(DeletePagesHandler))]
    [InlineData("DELETEPAGES", typeof(DeletePagesHandler))]
    [InlineData("watermark", typeof(WatermarkHandler))]
    [InlineData("compress", typeof(CompressionHandler))]
    [InlineData("extractcontent", typeof(ContentExtractionHandler))]
    public void GetHandler_WithKnownAction_ReturnsExpectedHandler(string action, Type expectedType)
    {
        IPdfCommandHandler? handler = PdfCommandRegistry.GetHandler(action);

        Assert.NotNull(handler);
        Assert.IsType(expectedType, handler);
    }

    [Fact]
    public void GetHandler_WithUnknownAction_ReturnsNull()
    {
        IPdfCommandHandler? handler = PdfCommandRegistry.GetHandler("not-a-real-action");

        Assert.Null(handler);
    }

    [Fact]
    public void GetHandler_WithNullAction_ReturnsNull()
    {
        IPdfCommandHandler? handler = PdfCommandRegistry.GetHandler(null!);

        Assert.Null(handler);
    }
}
