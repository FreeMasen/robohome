using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoboHome.Data;
using RoboHome.Models;

namespace RoboHome.Services
{
    public class FlipScheduler
    {
        private string WeatherServiceUri;
        private readonly RoboContext _context;
        
        public FlipScheduler(string uri, RoboContext context)
        {
            this.WeatherServiceUri = uri;
            this._context = context;
        }

        public void Start(double intervalMs = (60 *1000))
        {
            var msToMidnight = DateTime.Today.AddDays(1) - DateTime.Now;
            var t = new Timer(msToMidnight.Milliseconds);
            t.Elapsed += this.TimerCb;
            t.Start();
        }

        public void TimerCb(object sender, EventArgs e)
        {
            this.GetDailyInfo()
                .GetAwaiter()
                .OnCompleted(() => {
                    System.Console.WriteLine("TimerCb");
                    var timer = (Timer)sender;
                    timer.Interval = DateTime.Now.MsToNextMidnight();
                    timer.Start();
                });
        }

        public async Task<object> GetDailyInfo()
        {
            var client = new HttpClient();
            var res = await client.GetAsync(this.WeatherServiceUri);
            var weatherInfo = JObject.Parse(await res.Content.ReadAsStringAsync());
            var sunriseEpoc = (double?)weatherInfo["daily"]["data"][0]["sunrise"];
            var sunsetEpoc = (double?)weatherInfo["daily"]["data"][0]["sunrise"];
            if (sunriseEpoc.HasValue) {
                var sunrise = sunriseEpoc.ToDateAsUnix();
                var sunset = sunsetEpoc.ToDateAsUnix();
            }

            return weatherInfo;
        }

        private async Task SaveDailyInfo(DateTime sunrise, DateTime sunset)
        {
            this._context.KeyTimes.Add(new KeyTimes() {
                Dawn = new KeyTime() {
                    Hours = sunrise.Hour,
                    Minutes = sunrise.Minute
                },
                Light = new KeyTime() {
                    Hours = sunrise.Hour + 1,
                    Minutes = sunrise.Minute
                },
                Dusk = new KeyTime() {
                    Hours = sunset.Hour - 1,
                    Minutes = sunset.Minute
                },
                Dark = new KeyTime() {
                    Hours = sunset.Hour,
                    Minutes = sunset.Minute
                }
            });
        }
    }

    public static class FlipSchedulerExtensions
    {
        public static void AddFlipScheduler(this IServiceCollection services, string weatherServiceUri)
        {
            services.AddSingleton<FlipScheduler, FlipScheduler>(options => {
                var context = options.GetService<RoboContext>();
                return new FlipScheduler(weatherServiceUri, context);
            });
        }

        public static void StartFlipScheduler(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var scheduler =  serviceScope.ServiceProvider.GetService<FlipScheduler>();       
                scheduler.Start();
            }
        }
        public static DateTime? ToDateAsUnix( this double? unixTimeStamp )
        {
            if (!unixTimeStamp.HasValue) return null;
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds( unixTimeStamp.Value ).ToLocalTime();
            return dtDateTime;
        }

        public static int MsToNextMidnight(this DateTime dt) 
        {
            var toMidnight = dt.AddDays(1) - dt;
            return toMidnight.Milliseconds;
        }
    }

}