using GeradorDeTiagoes.Domain.Entities;

namespace GeradorDeTiagoes.Domain.PdfModule
{
    public interface IPdfGenerator
    {
        byte[] GenerateTestPdf(Test test, bool withAnswerKey);
    }
}
