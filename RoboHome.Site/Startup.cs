using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using RoboHome.Data;
using RoboHome.Services;
using Microsoft.AspNetCore.SpaServices;

namespace RoboHome
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
            var MQConnectionString = Configuration.GetConnectionString("MQConnectionString");
            var weatherUri = Configuration.GetConnectionString("WeatherServiceUri");
            services.AddDbContext<RoboContext>(options =>  
                            options.UseNpgsql(dbConnectionString));
            services.AddMvc();
            services.AddMqClient(MQConnectionString);
            services.AddFlipScheduler(weatherUri);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use( async (context, next) => {
                Console.WriteLine($"{System.DateTime.Now}::{context.Request.Method}: {context.Request.Path}");
                await next();
            });
            // using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            // {
            //     var context =  serviceScope.ServiceProvider.GetService<RoboContext>();       
            //     context.EnsureSeedData();
            // }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            
                routes.MapSpaFallbackRoute("spa-fallback", new { controller = "Home", action = "Index" });

            });
            app.UseStaticFiles();
            app.StartFlipScheduler();
        }
    }
}
