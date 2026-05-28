# EditPdf

EditPdf is a .NET 9 command-line tool for common PDF manipulation tasks such as deleting, inserting, rotating, moving, splitting, merging, metadata editing, watermarking, compression, and content extraction.

## Features

- Page operations
  - Delete page ranges
  - Insert pages from another PDF
  - Insert blank pages
  - Duplicate pages
  - Rotate pages
  - Move pages
  - Reorder pages
- Document operations
  - Merge multiple PDFs
  - Split document by range, interval, or parts
  - Extract selected pages
  - Compress document
- Content operations
  - Add page numbers
  - Add text watermark
  - Extract text/images
- Metadata operations
  - Edit title, author, subject, keywords, and creator

## Requirements

- .NET SDK 9.0+
- Windows/macOS/Linux (CLI)

## Project Structure

- `EditPdf/` - main console application
- `EditPdf.Tests/` - xUnit test project
- `EditPdf.sln` - solution file

## Build

From the solution root:

```powershell
dotnet build EditPdf.sln
```

## Run

### Positional mode

```powershell
dotnet run --project .\\EditPdf\\EditPdf.csproj -- <inputFile> <action> [outputFile] [--silent]
```

Example:

```powershell
dotnet run --project .\\EditPdf\\EditPdf.csproj -- .\\samples\\in.pdf deletePages .\\samples\\out.pdf
```

### Named-argument mode

```powershell
dotnet run --project .\\EditPdf\\EditPdf.csproj -- --input <file> <action> [--output <file>] [--silent] [operation options]
```

Example:

```powershell
dotnet run --project .\\EditPdf\\EditPdf.csproj -- --input in.pdf deletePages --start 1 --end 5 --output out.pdf
```

## Actions

- `deletePages`
- `insertPages`
- `blankPage`
- `duplicatePages`
- `rotatePages`
- `movePage`
- `reorderPages`
- `mergeDocuments`
- `splitDocument`
- `extractPages`
- `compress`
- `pageNumbering`
- `watermark`
- `editMetadata`
- `extractContent`

Run without arguments to print full built-in usage/help:

```powershell
dotnet run --project .\\EditPdf\\EditPdf.csproj --
```

## Testing

Run all tests:

```powershell
dotnet test .\\EditPdf.Tests\\EditPdf.Tests.csproj
```

## Notes

- Input and output files must be different paths.
- Output directory is created automatically when possible.
- In interactive mode, overwrite is prompted.
- In `--silent` mode, overwrite is auto-confirmed.
