using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
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
        private Timer Timer;
        
        public FlipScheduler(string uri, RoboContext context)
        {
            this.WeatherServiceUri = uri;
            this._context = context;
            this.Timer = new Timer();
            this.Timer.Elapsed += this.TimerCb;
            this.Timer.Start();
        }

        public async void TimerCb(object sender, EventArgs e)
        {
            var todaysTimes = this._context
                    .KeyTimes
                    .Where(time => time.Date == DateTime.Today)
                    .ToList();
            if (todaysTimes.Count() < 4) {
                var times = await this.GetDailyInfo();
                if (times.HasValue) {
                    await this.SaveDailyInfo(times.Value.Item1, times.Value.Item2);
                }
            }
            var timer = (Timer)sender;
            timer.Interval = this.MsToNextMidnight();
            timer.Start();
        }

        public async Task<(Time, Time)?> GetDailyInfo()
        {
            try {
                var client = new HttpClient();
                var res = await client.GetAsync(this.WeatherServiceUri);
                var contentText = await res.Content.ReadAsStringAsync();
                var weatherInfo = JObject.Parse(contentText);
                var sunriseObj = weatherInfo["sun_phase"]["sunrise"];
                var sunsetObj = weatherInfo["sun_phase"]["sunrise"];
                if (sunriseObj.HasValues && sunsetObj.HasValues) {
                    var sunrise = new Time() {
                        TimeType = TimeType.Sunrise,
                        Hour = (int)sunriseObj["hour"],
                        Minute = (int)sunriseObj["minute"]
                    };
                    var sunset = new Time() {
                        TimeType = TimeType.Sunset,
                        Hour = (int)sunsetObj["hour"],
                        Minute = (int)sunsetObj["minute"]
                    };
                    return (sunrise, sunset);
                }
            } catch (Exception e) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
                return null;
        }

        private async Task SaveDailyInfo(Time sunrise, Time sunset)
        {
            var today = LastMidnight();
            var keyTimes = new List<KeyTime>() {
                new KeyTime() {
                    Date = today,
                    Time = new Time() {
                                TimeType = TimeType.Dawn,
                                Hour = sunrise.Hour - 1,
                                Minute = sunrise.Minute
                    }
                },
                new KeyTime() {
                    Date = today,                     
                    Time = sunrise
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
                    Time = sunset
                }
            };
            await this._context.KeyTimes.AddRangeAsync(keyTimes);
            await this._context.SaveChangesAsync();
        }

        private double MsToNextMidnight()
        {
            var dt = DateTime.Now;
            var toMidnight = dt.AddDays(1) - dt;
            return toMidnight.TotalMilliseconds;
        }

        private DateTime LastMidnight()
        {
            var dt = DateTime.Now;
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }
    }

}