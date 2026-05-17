using iText.Kernel.Pdf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditPdf.Interface
{
    public interface IPdfCommandHandler
    {
        void Execute(PdfDocument doc, string inputFile, string outputFile, int maxPage);
    }
}
