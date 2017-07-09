using System.Collections.Generic;

namespace RoboHome.Models
{
    public class Light
    {
        public int Id { get; set; }
        public int PinId { get; set; }
        public LightState State { get; set; }
        public List<Flip> Flips { get; set; }
        public Light() {}
    }
}