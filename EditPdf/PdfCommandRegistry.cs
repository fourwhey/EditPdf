using EditPdf.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditPdf
{
    public static class PdfCommandRegistry
    {
        private static readonly Dictionary<string, IPdfCommandHandler> _handlers =
            new(StringComparer.InvariantCultureIgnoreCase)
            {
            // Original operations
            { "deletepages", new DeletePagesHandler() },
            { "insertpages", new InsertPagesHandler() },
            { "rotatepages", new RotatePagesHandler() },
            { "movepage", new MovePagesHandler() },
            { "mergedocuments", new MergeDocumentsHandler() },
            { "splitdocument", new SplitDocumentHandler() },
            // New operations - Phase 1
            { "blankpage", new BlankPageHandler() },
            { "duplicatepages", new DuplicatePagesHandler() },
            { "editmetadata", new MetadataHandler() },
            { "extractpages", new ExtractPagesHandler() },
            // New operations - Phase 2
            { "reorderpages", new ReorderPagesHandler() },
            { "pagenumbering", new PageNumberingHandler() },
            { "watermark", new WatermarkHandler() },
            // New operations - Phase 3
            { "compress", new CompressionHandler() },
            { "extractcontent", new ContentExtractionHandler() }
            };

        public static IPdfCommandHandler? GetHandler(string action)
        {
            _handlers.TryGetValue(action ?? "", out var handler);
            return handler;
        }
    }

}
