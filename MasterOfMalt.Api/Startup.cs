using MasterOfMalt.Api.Services;
using MasterOfMalt.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;

namespace MasterOfMalt.Api
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
            services.AddSingleton<IImagePropertiesProvider, ImagePropertiesProvider>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IImageInfoCache, ImageInfoInMemoryCache>();

            services.AddResponseCaching();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromHours(Configuration.GetValue<int>("CacheRetentionAgeInHours"))
                };

                context.Response.Headers[HeaderNames.Vary] = new[] {"Accept-Encoding"};

                await next();
            });

            var imageDirectoryService = (IImageInfoCacheInit) app.ApplicationServices.GetService<IImageInfoCache>();
            imageDirectoryService.Init();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
