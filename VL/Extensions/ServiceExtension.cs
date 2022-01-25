using AutoMapper;
using Entities;
using Entities.Mappings;
using LoggerService;
using MailingService.Contracts;
using MailingService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new UserProfile());
                m.AddProfile(new BookProfile());
                m.AddProfile(new AuthorProfile());
                m.AddProfile(new ReviewProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
