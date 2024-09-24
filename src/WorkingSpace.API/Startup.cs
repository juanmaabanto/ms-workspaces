using System;
using System.IO;
using System.Reflection;
using Autofac;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Sofisoft.Accounts.WorkingSpace.API.Infrastructure.AutofacModules;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Sofisoft.Accounts.WorkingSpace.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<WorkingSpaceSetting>(Configuration);

            if(_env.IsDevelopment())
            {
                services.AddDataProtection()
                    .UseEphemeralDataProtectionProvider();
            }
            else
            {
                services.AddDataProtection()
                    .PersistKeysToAzureBlobStorage(new Uri(Configuration["BlobSasUri"]));
            }

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(Configuration["AllowedOrigins"].Split(";"))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            services.AddOpenIddict()
            .AddValidation(options => {
                options.SetIssuer(Configuration["Services:IdentityUrl"]);
                options.AddAudiences("sofisoft");

                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String(Configuration["EncryptionKey"])));

                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });

            services.AddControllers()
                .AddFluentValidation(options =>
                {
                    options.ValidatorOptions.LanguageManager.Culture = new System.Globalization.CultureInfo("es");
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Title = "Working Space API",
                    Version = "v1",
                    Description = "Specifying services for the workspace.",
                    Contact = new OpenApiContact {
                        Email = "juanmanuel.abanto@sofisofttech.com", 
                        Name = "Sofisoft Technologies SAC", 
                        Url = new Uri(Configuration["HomePage"])
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule(Configuration));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
                .UseSwaggerUI(c =>
                    {
                        c.DocumentTitle = "Sofisoft - WorkingSpaceAPI";
                        c.RoutePrefix = string.Empty;
                        c.SupportedSubmitMethods(Array.Empty<SubmitMethod>());
                        c.SwaggerEndpoint(Configuration["SwaggerEndPoint"], "Working Space V1");
                        c.DefaultModelsExpandDepth(-1);
                    });

            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
