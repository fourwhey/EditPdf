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
            { "deletepages", new DeletePagesHandler() },
            { "insertpages", new InsertPagesHandler() },
            { "rotatepages", new RotatePagesHandler() },
            { "movepage", new MovePagesHandler() }
            };

        public static IPdfCommandHandler? GetHandler(string action)
        {
            _handlers.TryGetValue(action ?? "", out var handler);
            return handler;
        }
    }

}
