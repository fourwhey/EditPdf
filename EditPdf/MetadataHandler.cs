using EditPdf.Interface;

using iText.Kernel.Pdf;

namespace EditPdf
{
    public class MetadataHandler : IPdfCommandHandler
    {
        public void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage)
        {
            PdfUtils.SafeClearConsole();
            Console.WriteLine("Edit PDF metadata.");
            Console.WriteLine();

            // Get current metadata
            var docInfo = doc.GetDocumentInfo();

            Console.WriteLine("Current metadata:");
            Console.WriteLine($"  Title: {docInfo.GetTitle() ?? "(not set)"}");
            Console.WriteLine($"  Author: {docInfo.GetAuthor() ?? "(not set)"}");
            Console.WriteLine($"  Subject: {docInfo.GetSubject() ?? "(not set)"}");
            Console.WriteLine($"  Keywords: {docInfo.GetKeywords() ?? "(not set)"}");
            Console.WriteLine();

            Console.WriteLine("Enter new metadata (leave blank to keep current value):");
            Console.WriteLine();

            // Title
            Console.Write("Title: ");
            string? title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                docInfo.SetTitle(title);
            }

            // Author
            Console.Write("Author: ");
            string? author = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(author))
            {
                docInfo.SetAuthor(author);
            }

            // Subject
            Console.Write("Subject: ");
            string? subject = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(subject))
            {
                docInfo.SetSubject(subject);
            }

            // Keywords
            Console.Write("Keywords: ");
            string? keywords = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                docInfo.SetKeywords(keywords);
            }

            // Creator
            Console.Write("Creator: ");
            string? creator = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(creator))
            {
                docInfo.SetCreator(creator);
            }

            Console.WriteLine();
            Console.WriteLine("Metadata updated:");
            Console.WriteLine($"  Title: {docInfo.GetTitle() ?? "(not set)"}");
            Console.WriteLine($"  Author: {docInfo.GetAuthor() ?? "(not set)"}");
            Console.WriteLine($"  Subject: {docInfo.GetSubject() ?? "(not set)"}");
            Console.WriteLine($"  Keywords: {docInfo.GetKeywords() ?? "(not set)"}");
            Console.WriteLine($"  Creator: {docInfo.GetCreator() ?? "(not set)"}");
            Console.WriteLine();
            Console.WriteLine("Completed.");
        }
    }
}
