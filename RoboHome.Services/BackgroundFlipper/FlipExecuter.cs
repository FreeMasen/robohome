using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoboHome.Data;
using RoboHome.Models;

namespace RoboHome.Services
{
    public class FlipExecuter: IHostedService
    {
        private Timer timer;
        private readonly RoboContext _context;
        private readonly Messenger _messenger;

        public FlipExecuter(RoboContext context, IMqClient messenger)
        {
            Console.WriteLine("new FlipExecuter");
            this._context = context;
            this._messenger = (Messenger)messenger;
        }

        public void TimerCb(object state)
        {
            try {
                var hour = DateTime.Now.Hour;
                var min = DateTime.Now.Minute;
                var tod = TimeOfDay.AM;
                if (hour > 12) {
                    hour -= 12;
                    tod = TimeOfDay.PM;
                }
                if (hour >= 12)
                {
                    tod = TimeOfDay.PM;
                }
                var flips = this.GetFlips(hour, min, tod);
                foreach (var flip in flips)
                {
                    var remote = this._context.Remotes
                                        .Where(r => r.Switches.Any(s => s.Id == flip.SwitchId))
                                        .FirstOrDefault();
                    var sw = this._context.Switches.SingleOrDefault(s => s.Id == flip.SwitchId);
                    var msg = new {switch_id = sw.Number, direction = flip.Direction};
                    this._messenger.SendMessage(remote.Id, msg);
                    if (sw != null) {
                        sw.State = flip.Direction;
                        this._context.SaveChanges();
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("Error flipping in background {0}", ex.Message);
            }
        }

        public List<Flip> GetFlips(int hour, int min, TimeOfDay tod)
        {
            try {
                return this._context
                            .Flips
                            .Where(f => f.Time.Hour == hour &&
                                    f.Time.Minute == min &&
                                    f.Time.TimeOfDay == tod &&
                                    (f.Time.DayOfWeek & this.TodayDoW()) > 0)
                            .ToList();
            } catch (Exception ex) {
                Console.WriteLine("Error getting flips {0}", ex);
                throw ex;
            }
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken != null && !cancellationToken.IsCancellationRequested)
            {
                if (this.timer == null)
                {
                    this.timer = new Timer(this.TimerCb, null, 0, 1000 * 60);
                } else {
                    this.timer.Change(0, 1000 * 60);
                }
            }
            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken != null && !cancellationToken.IsCancellationRequested) {
                this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            return Task.CompletedTask;
        }

        private WeekDay TodayDoW() {
            switch (DateTime.Today.DayOfWeek) {
                case DayOfWeek.Sunday:
                    return WeekDay.Sunday;
                case DayOfWeek.Monday:
                    return WeekDay.Monday;
                case DayOfWeek.Tuesday:
                    return WeekDay.Tuesday;
                case DayOfWeek.Wednesday:
                    return WeekDay.Wednesday;
                case DayOfWeek.Thursday:
                    return WeekDay.Thursday;
                case DayOfWeek.Friday:
                    return WeekDay.Friday;
                case DayOfWeek.Saturday:
                    return WeekDay.Saturday;
                default:
                    throw new Exception("Unknown Day of the week");
            }
        }
    }

}