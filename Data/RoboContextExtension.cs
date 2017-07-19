using System.Linq;
using RoboHome.Models;
namespace RoboHome.Data
{
    public static class RoboContextExtension
    {
        public static void EnsureSeedData(this RoboContext context) {
            if (context.Lights.Any()) 
                return;
            context.Lights.Add(new Light() {
                    OnPin = 4543804,
                    OffPin = 4543795,
                    Name = "Not In Use",
                    State = LightState.Off
                });
                context.Lights.Add(new Light() {
                    OnPin = 4543948,
                    OffPin = 4543939,
                    Name = "Not In Use",
                    State = LightState.Off
                });
                context.Lights.Add(new Light() {
                    OnPin = 4544268,
                    OffPin = 4544359,
                    Name = "Not In Use",
                    State = LightState.Off
                });
                context.Lights.Add(new Light() {
                    OnPin = 4545804,
                    OffPin = 4545795,
                    Name = "Not In Use",
                    State = LightState.Off
                });
                context.Lights.Add(new Light() {
                    OnPin = 4551948,
                    OffPin = 4551939,
                    Name = "Not In Use",
                    State = LightState.Off
                });
                context.SaveChanges();
            
        }
    }
}