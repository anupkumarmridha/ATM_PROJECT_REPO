using ATMAPPAPI.Contexts;
using ATMAPPAPI.Repositoris;
using ATMAPPAPI.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ATMAPPAPI
{
    public class Program
    {
        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AtmDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("defaultConnection")));
        }

        #region RegisterRepositories
        /// <summary>
        /// Registering Repositories
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<ICardOperations, CardOperations>();
        }
        #endregion RegisterRepositories

        #region RegisterServices
        /// <summary>
        /// Registering Services
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterServices(IServiceCollection services)
        {
            //services.AddScoped<IUserService, UserService>(); 
            services.AddScoped<IEmailService, EmailService>();


            services.AddScoped<IAtmService, AtmService>();
            services.AddScoped<ICardValidationService, CardValidationService>();
        }
        #endregion RegisterServices


        #region ConfigureServices
        /// <summary>
        /// Registering Services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddMemoryCache();
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            //    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            //});
            #region CORS
            services.AddCors(opts =>
            {
                opts.AddPolicy("MyCors", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            #endregion
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //AddDbContext(services, configuration);

            RegisterRepositories(services);
            RegisterServices(services);
        }
        #endregion ConfigureServices


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.UseCors("MyCors");
            app.MapControllers();

            app.Run();
        }
    }
}
