using GeradorDeTiagoes.Structure.Orm.Shared;
using Microsoft.EntityFrameworkCore;

namespace GeradorDeTiagoes.WebApp.Orm
{
    public static class DataBaseOperations
    {
        public static void ApplyMigrations(this IHost app)
        {
            var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<GeradorDeTiagoesDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
