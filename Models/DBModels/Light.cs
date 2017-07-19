using System.Collections.Generic;

namespace RoboHome.Models
{
    public class Light
    {
        public int Id { get; set; }
        public int OnPin { get; set; }
        public int OffPin { get; set; }
        public string Name { get; set; }
        public LightState State { get; set; }
        public List<Flip> Flips { get; set; }
        public Light() {}
    }
}