using Entities;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.Contracts;
using VL.Services;

namespace VL.Extensions
{
    public static class ServiceExtensions
    {

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var dbConnectionString = config["ConnectionStrings:VLDBConnectionString"];            
            services.AddDbContext<VLDBContext>(options => options.UseSqlServer(dbConnectionString, b => b.MigrationsAssembly("VL")).LogTo(Console.WriteLine, LogLevel.Information));
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
        }
    }
}
