using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using PublicChat.Hubs;
using PublicChat.Services;

namespace PublicChat
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();

            // Register Swagger services
            services.AddSwaggerDocument(config =>
            {
                // config.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT token"));
                // config.AddSecurity("JWT token", new OpenApiSecurityScheme
                // {
                //     Type = OpenApiSecuritySchemeType.ApiKey,
                //     Name = "Authorization",
                //     Description = "Copy 'Bearer ' + valid JWT token into field",
                //     In = OpenApiSecurityApiKeyLocation.Header
                // });

                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Vagachat API";
                    document.Info.Description = "Backend for Vagachat";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Vagabundo"
                    };
                };
            });

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder
                        //.AllowAnyOrigin()
                        .WithOrigins("http://localhost", "http://vagabundo-webchat-front.westeurope.azurecontainer.io",
                        "https://webchatfront-dev-appservice.azurewebsites.net", "http://webchatfront-dev-appservice.azurewebsites.net")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                })
            );

            services.AddScoped<IAIChatService, ChatGPTService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseCors("CorsPolicy");

            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatsocket");
            });
        }
    }
}
