using DomowaBiblioteka.SQL.Context;
using DomowaBiblioteka.SQL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

namespace DomowaBiblioteka.SQL
{
    public static class Bootstrap
    {
        public static void AddSQLHomeLibrary(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("BookDBConnection")));
            
            services.AddScoped<IBookRepository, SQLBookRepository>();
        }
    }
}
