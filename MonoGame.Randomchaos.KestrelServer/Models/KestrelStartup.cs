using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MonoGame.Randomchaos.KestrelServer.Models
{
    public class KestrelStartup
    {
        /// <summary>   The configuration. </summary>
        protected readonly IConfiguration _configuration;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <param name="configuration">   The configuration. </param>
        ///-------------------------------------------------------------------------------------------------

        public KestrelStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Configure services. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <param name="services"> The services. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddAuthentication()
                .AddCookie(options => options.SlidingExpiration = true)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["JWT:Issuer"],
                        ValidAudience = _configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
                    };
                });

            services.AddMvc()
                .AddApplicationPart(Assembly.GetEntryAssembly())
                .AddNewtonsoftJson();

            string SiteVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            string SiteName = Assembly.GetEntryAssembly().GetName().Name;

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = SiteName, Version = SiteVersion });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,


                        },
                        new string[] {}
                    }
                });
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Configures the given application. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <param name="app">  The application. </param>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            if (_configuration.GetValue<bool>("EnableHttps"))
                app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            app.UseStaticFiles();
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new
                List<string> { "index" }
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.ConfigObject.AdditionalItems.Add("syntaxHighlight", true);
            });
        }
    }
}
