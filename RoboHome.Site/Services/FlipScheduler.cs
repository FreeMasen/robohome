using System;
using System.Collections.Generic;
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

        public async void TimerCb(object sender, EventArgs e)
        {
            var times = await this.GetDailyInfo();
            if (times.HasValue) {
                await this.SaveDailyInfo(times.Value.Item1, times.Value.Item2);
            }
        }

        public async Task<(DateTime, DateTime)?> GetDailyInfo()
        {
            var client = new HttpClient();
            var res = await client.GetAsync(this.WeatherServiceUri);
            var weatherInfo = JObject.Parse(await res.Content.ReadAsStringAsync());
            var sunriseEpoc = (double?)weatherInfo["daily"]["data"][0]["sunrise"];
            var sunsetEpoc = (double?)weatherInfo["daily"]["data"][0]["sunrise"];
            if (sunriseEpoc.HasValue) {
                var sunrise = sunriseEpoc.ToDateAsUnix();
                var sunset = sunsetEpoc.ToDateAsUnix();
                return (sunrise.Value, sunset.Value);
            }
            return null;
        }

        private async Task SaveDailyInfo(DateTime sunrise, DateTime sunset)
        {
            var today = DateTime.Now.AtMidnight();
            var keyTimes = new List<KeyTime>() {
                new KeyTime() {
                    Date = today,
                    Time = new Time() {
                                TimeType = TimeType.Dawn,
                                Hour = sunrise.Hour,
                                Minute = sunrise.Minute
                    }
                },
                new KeyTime() {
                    Date = today,                     
                    Time = new Time() {
                        TimeType = TimeType.Dawn,
                        Hour = sunrise.Hour + 1,
                        Minute = sunrise.Minute,
                    }
                },
                new KeyTime() {
                    Date = today,
                    Time = new Time() {
                                TimeType = TimeType.Dusk,
                                Hour = sunrise.Hour - 1,
                                Minute = sunset.Minute,
                            }
                },
                new KeyTime() {
                    Date = today,
                    Time = new Time() {
                            TimeType = TimeType.Sunset,
                            Hour = sunset.Hour,
                            Minute = sunset.Minute,
                        }
                }
            };
            await this._context.KeyTimes.AddRangeAsync(keyTimes);
            await this._context.SaveChangesAsync();
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

        public static DateTime AtMidnight(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }

        public static int MsToNextMidnight(this DateTime dt) 
        {
            var toMidnight = dt.AddDays(1) - dt;
            return toMidnight.Milliseconds;
        }
    }

}