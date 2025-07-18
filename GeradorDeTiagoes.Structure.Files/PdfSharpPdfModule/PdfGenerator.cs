using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.PdfModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace GeradorDeTiagoes.Structure.Files.PdfSharpPdfModule
{
    public class PdfGenerator : IPdfGenerator
    {
        public byte[] GenerateTestPdf(Test test, bool withAnswerKey)
        {
            using var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var fontTitle = new XFont("Verdana", 20, XFontStyle.Bold);
            var fontNormal = new XFont("Verdana", 12, XFontStyle.Regular);
            var fontAnswer = new XFont("Verdana", 12, XFontStyle.Italic);

            double y = 40;

            
            gfx.DrawString($"Prova: {test.Title}", fontTitle, XBrushes.Black, new XRect(20, y, page.Width, 30), XStringFormats.TopLeft);
            y += 40;
            gfx.DrawString($"Série: {test.GradeLevel}", fontNormal, XBrushes.Black, new XRect(20, y, page.Width, 20), XStringFormats.TopLeft);
            y += 30;

            
            int count = 1;
            foreach (var question in test.Questions)
            {
                if (y > page.Height - 100)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 40;
                }

                gfx.DrawString($"{count}) {question.Text}", fontNormal, XBrushes.Black, new XRect(20, y, page.Width - 40, 40), XStringFormats.TopLeft);
                y += 20;

                char option = 'A';
                foreach (var alt in question.Alternatives)
                {
                    gfx.DrawString($"{option}) {alt.Text}", fontNormal, XBrushes.Black, new XRect(40, y, page.Width - 60, 20), XStringFormats.TopLeft);
                    y += 20;
                    option++;
                }

                
                if (withAnswerKey)
                {
                    var correct = question.Alternatives.FirstOrDefault(a => a.isCorrect);
                    var correctLetter = (char)('A' + question.Alternatives.IndexOf(correct));
                    gfx.DrawString($"Resposta: {correctLetter}", fontAnswer, XBrushes.DarkGreen, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
                    y += 20;
                }

                y += 10;
                count++;
            }

            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }
    }
}