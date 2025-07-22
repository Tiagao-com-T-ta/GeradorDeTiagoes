using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.PdfModule;
using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.Domain.Shared;
using GeradorDeTiagoes.Structure.Files.PdfSharpPdfModule;
using GeradorDeTiagoes.Structure.Files.Shared;
using GeradorDeTiagoes.Structure.Orm.DisciplineModule;
using GeradorDeTiagoes.Structure.Orm.QuestionModule;
using GeradorDeTiagoes.Structure.Orm.TestModule;
using GeradorDeTiagoes.WebApp.DependencyInjection;
using GeradorDeTiagoes.WebApp.Validators;

namespace GeradorDeTiagoes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<DataContext>(_ => new DataContext(true));
            builder.Services.AddScoped<IRepository<Test>, TestRepositoryOrm>();
            builder.Services.AddScoped<IDisciplineRepository, DisciplineRepositoryOrm>();
            builder.Services.AddScoped<IRepository<Discipline>, DisciplineRepositoryOrm>();
            builder.Services.AddScoped<IRepository<Subject>, SubjectRepositoryOrm>();
            builder.Services.AddScoped<IRepository<Question>, QuestionRepositoryOrm>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepositoryOrm>();
            builder.Services.AddScoped<IPdfGenerator, PdfGenerator>();
            builder.Services.AddScoped<TestValidator>();

            builder.Services.AddEntityFrameworkConfig(builder.Configuration);
            builder.Services.AddSerilogConfig(builder.Logging);

            var app = builder.Build();

            app.UseAntiforgery();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
