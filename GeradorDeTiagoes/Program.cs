using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.Shared;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.Structure.Files.DisciplineModule;
using GeradorDeTiagoes.Structure.Files.Shared;
using GeradorDeTiagoes.Structure.Files.SubjectsModule;

namespace GeradorDeTiagoes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ISubjectRepository, SubjectRepositoryFile>();
            builder.Services.AddScoped<IRepository<Subject>, SubjectRepositoryFile>();
            builder.Services.AddScoped<IDisciplineRepository, DisciplineRepositoryFile>();
            builder.Services.AddScoped<IRepository<Discipline>, DisciplineRepositoryFile>();
            builder.Services.AddScoped<DataContext>();
            var app = builder.Build();

            app.UseAntiforgery();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
