using System;
using System.CodeDom.Compiler;
using System.Linq;
using FirebaseAdmin;
using litter_tracker.CloudDatastore.DAL;
using litter_tracker.Objects.InternalObjects;
using litter_tracker.Services.OpenWeatherApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using Scrutor;
using static Google.Apis.Auth.OAuth2.GoogleCredential;

namespace store_api
{
    /*
    Handles all of the dependency injection, CORS setup and Swagger/OpenAPI setup for the application.
    Automatic interface class registration with Scrutor so no need to register each interface to class.
    Also optionally sets the Google Application credential environment variable when in development or staging.
    When deployed it magically exists already.
    */
    public class Startup
    {

        private readonly string CorsKey = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsKey,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Litter Tracker API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: {Firebase JWT Token}"
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            ScanForAllRemainingRegistrations(services);

            AddConfigs(services);
        }

        public static void ScanForAllRemainingRegistrations(IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(Startup), typeof(Repository), typeof(OpenWeatherServiceAgent))
                .AddClasses(x => x.WithoutAttribute(typeof(GeneratedCodeAttribute)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }

        public void AddConfigs(IServiceCollection services)
        {
            services.Configure<ConnectionStrings>(option =>
                Configuration.GetSection(nameof(ConnectionStrings)).Bind(option));
            services.Configure<OpenWeatherApi>(option =>
                Configuration.GetSection(nameof(OpenWeatherApi)).Bind(option));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:\\Users\\harry\\litter-tracker-b243db1f5c12.json");
            }

            //for home staging server saving the $$ on cloud builds
            if (env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:\\Secrets\\litter-tracker-b243db1f5c12.json");
            }

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsKey);

            app.UseReDoc();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            FirebaseApp.Create(new AppOptions
            {
                Credential = GetApplicationDefault()
            });
        }
    }
}