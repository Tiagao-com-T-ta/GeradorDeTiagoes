using GeradorDeTiagoes.Structure.Orm.Shared;
using Microsoft.EntityFrameworkCore;

namespace GeradorDeTiagoes.WebApp.DependencyInjection
{
    public static class EntityFrameworkConfig
    {
        public static void AddEntityFrameworkConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
        {
            var connectionString = configuration["SQL_CONNECTION_STRING"];

            services.AddDbContext<GeradorDeTiagoesDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
