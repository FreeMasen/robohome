using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoboHome.Models;

namespace RoboHome.Data
{
    public static class RoboContextExtension
    {
        public static IServiceCollection UseRoboContext(this IServiceCollection services, string connectionString) 
        {
            services.AddDbContext<RoboContext>(options => 
                                                options.UseNpgsql(connectionString));
            return services;
        }
        public static void EnsureSeedData(this RoboContext context) {
            if (!context.Switches.Any()) 
                SeedSwitches(context);
            if (!context.Remotes.Any())
                SeedRemotes(context);            
        }

        private static void SeedSwitches(RoboContext context)
        {
            context.Switches.Add(new Switch() {
                OnPin = 4543804,
                OffPin = 4543795,
                Name = "Not In Use",
                State = SwitchState.Off
            });
            context.Switches.Add(new Switch() {
                OnPin = 4543948,
                OffPin = 4543939,
                Name = "Not In Use",
                State = SwitchState.Off
            });
            context.Switches.Add(new Switch() {
                OnPin = 4544268,
                OffPin = 4544359,
                Name = "Not In Use",
                State = SwitchState.Off
            });
            context.Switches.Add(new Switch() {
                OnPin = 4545804,
                OffPin = 4545795,
                Name = "Not In Use",
                State = SwitchState.Off
            });
            context.Switches.Add(new Switch() {
                OnPin = 4551948,
                OffPin = 4551939,
                Name = "Not In Use",
                State = SwitchState.Off
            });
            context.SaveChanges();
        }

        private static void SeedRemotes(RoboContext context)
        {
            context.Remotes.AddRange(
                new List<Remote>() {
                    new Remote() {
                        Location = "Living Room",
                        Switches = context.Switches.ToList()
                    },
                    new Remote() {
                        Location = "Annex",
                        Switches = new List<Switch>()
                    }
                });
            context.SaveChanges();
        }
    }
}