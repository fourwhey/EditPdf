using EditPdf;

namespace EditPdf.Tests;

public class CliArgumentParserTests
{
    [Fact]
    public void Parse_WithNamedArguments_ParsesExpectedValues()
    {
        string[] args =
        [
            "--input", "input.pdf",
            "deletePages",
            "--output", "output.pdf",
            "--silent",
            "--start", "2",
            "--end", "4"
        ];

        CliArgumentParser result = CliArgumentParser.Parse(args);

        Assert.Equal("input.pdf", result.InputFile);
        Assert.Equal("deletePages", result.Action);
        Assert.Equal("output.pdf", result.OutputFile);
        Assert.True(result.SilentMode);
        Assert.Equal(2, result.StartPage);
        Assert.Equal(4, result.EndPage);
    }

    [Fact]
    public void Parse_WithPositionOption_ParsesNumericPosition()
    {
        string[] args = ["--position", "3"];

        CliArgumentParser result = CliArgumentParser.Parse(args);

        Assert.Equal(3, result.Position);
        Assert.Null(result.WatermarkPosition);
    }

    [Fact]
    public void Parse_WithPositionOption_ParsesWatermarkPositionWhenNotNumeric()
    {
        string[] args = ["--position", "center"];

        CliArgumentParser result = CliArgumentParser.Parse(args);

        Assert.Null(result.Position);
        Assert.Equal("center", result.WatermarkPosition);
    }

    [Fact]
    public void Parse_WithTextOption_SetsBothTextFields()
    {
        string[] args = ["--text", "CONFIDENTIAL"];

        CliArgumentParser result = CliArgumentParser.Parse(args);

        Assert.Equal("CONFIDENTIAL", result.TextValue);
        Assert.Equal("CONFIDENTIAL", result.WatermarkText);
    }

    [Fact]
    public void Parse_WithPositionalArguments_ParsesInputActionAndOutput()
    {
        string inputPath = Path.GetTempFileName();

        try
        {
            string[] args = [inputPath, "rotatePages", "result.pdf"];

            CliArgumentParser result = CliArgumentParser.Parse(args);

            Assert.Equal(inputPath, result.InputFile);
            Assert.Equal("rotatePages", result.Action);
            Assert.Equal("result.pdf", result.OutputFile);
        }
        finally
        {
            File.Delete(inputPath);
        }
    }

    [Fact]
    public void IsValid_WhenInputMissing_ReturnsFalseWithError()
    {
        CliArgumentParser parser = new() { Action = "deletePages" };

        bool isValid = parser.IsValid(out string? error);

        Assert.False(isValid);
        Assert.Equal("Error: Input PDF file is required (--input <file> or positional)", error);
    }

    [Fact]
    public void IsValid_WhenActionMissing_ReturnsFalseWithError()
    {
        CliArgumentParser parser = new() { InputFile = "input.pdf" };

        bool isValid = parser.IsValid(out string? error);

        Assert.False(isValid);
        Assert.Equal("Error: Action is required (see help for available actions)", error);
    }

    [Fact]
    public void IsValid_WhenRequiredValuesProvided_ReturnsTrue()
    {
        CliArgumentParser parser = new() { InputFile = "input.pdf", Action = "deletePages" };

        bool isValid = parser.IsValid(out string? error);

        Assert.True(isValid);
        Assert.Null(error);
    }
}
