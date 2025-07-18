using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.PdfModule;
using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.Domain.Shared;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.Domain.TestsModule;
using GeradorDeTiagoes.Structure.Files.DisciplineModule;
using GeradorDeTiagoes.Structure.Files.PdfSharpPdfModule;
using GeradorDeTiagoes.Structure.Files.QuestionModule;
using GeradorDeTiagoes.Structure.Files.Shared;
using GeradorDeTiagoes.Structure.Files.SubjectsModule;
using GeradorDeTiagoes.Structure.Files.TestsModule;

namespace GeradorDeTiagoes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<DataContext>(_ => new DataContext(true));

            builder.Services.AddScoped<IRepository<Test>, TestRepositoryFile>();
            builder.Services.AddScoped<IRepository<Discipline>, DisciplineRepositoryFile>();
            builder.Services.AddScoped<IRepository<Subject>, SubjectRepositoryFile>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepositoryFile>();
            builder.Services.AddScoped<IPdfGenerator, PdfGenerator>();

            var app = builder.Build();

            app.UseAntiforgery();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
