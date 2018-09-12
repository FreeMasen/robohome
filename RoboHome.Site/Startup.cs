using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.SpaServices.Prerendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using RoboHome.Data;
using RoboHome.Extensions;
using RoboHome.Services;

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
            var MqConnection = Configuration.GetSection("MqUrl");
            var weatherUri = Configuration.GetConnectionString("WeatherServiceUri");
            services.UseRoboContext(dbConnectionString)
                    .AddMqClient(options => {
                        options.UseConfig(MqConnection);
                    })
                    .AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) => {
                Console.WriteLine($"{DateTime.Now}::{context.Request.Method}: {context.Request.Path}");
                await next();
            });
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            
                routes.MapSpaFallbackRoute("spa-fallback", new { controller = "Home", action = "Index" });

            });
            // if (env.IsDevelopment()) {
            //     var webpackPath = $"{Environment.CurrentDirectory}/webpack.config.js";
            //     app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions() {
            //         // HotModuleReplacement = true,
            //         ConfigFile = webpackPath
            //     });
            // }
            app.UseStaticFiles();
        }
    }
}
