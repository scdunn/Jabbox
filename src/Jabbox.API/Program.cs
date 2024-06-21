using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Jabbox.Data.Services;
using Jabbox.Data.Interfaces;
using Jabbox.API.DTOs;

namespace Jabbox.API
{
    public class Program
    {
        /// <summary>
        /// Entry point for application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);
            
            // Build application
            var app = builder.Build();
            ConfigureApplication(app);
            InitializeDatabase(app);

            app.Run();

        }

        /// <summary>
        /// Cofigures application services
        /// </summary>
        /// <param name="builder"></param>
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
     
 
            // CORS Policies
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.SetIsOriginAllowed(_ => true).AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            AddTokenHandler(builder);
            AddDataContext(builder);

            builder.Services.AddControllers();

            // Swagger Configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        /// <summary>
        /// Adds authentication through JWT Handler
        /// </summary>
        /// <param name="builder"></param>
        private static void AddTokenHandler(WebApplicationBuilder builder)
        {
            // JWT and authentication configuration
            var jwtSettings = builder.Configuration.GetSection("TokenSettings");
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(jwtSettings.GetSection("securityKey").Value))
                };
            });

            builder.Services.AddScoped<TokenHandler>();
        }

        /// <summary>
        /// Adds DataContext from connection including Repo services and automapping to DTOs
        /// </summary>
        /// <param name="builder"></param>
        private static void AddDataContext(WebApplicationBuilder builder)
        {


            // Data Context Configuration
            var connectionString = builder.Configuration.GetConnectionString("Jabbox");
            builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddTransient<IPostRepository, PostRepository>();
            builder.Services.AddTransient<IAccountRepository, AccountRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            // Auto Mapper Configuration
            builder.Services.AddAutoMapper(typeof(MappingProfile));

        }

        /// <summary>
        /// Configures application
        /// </summary>
        /// <param name="app"></param>
        private static void ConfigureApplication(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            //app.UseHttpsRedirection();
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

        }

        /// <summary>
        /// Ensure database exists and seeds with sample data from xml file
        /// </summary>
        /// <param name="app"></param>
        private static void InitializeDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                uow.EnsureDatabaseCreated();
                var filename = "Seed.xml";
                var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var seedFilePath = Path.Combine(currentDirectory, "Data", filename);

                uow.SeedData(seedFilePath);
            }
        }
    }
}