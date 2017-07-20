using System.Linq;
using RoboHome.Models;
namespace RoboHome.Data
{
    public static class RoboContextExtension
    {
        public static void EnsureSeedData(this RoboContext context) {
            if (context.Switches.Any()) 
                return;
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
    }
}