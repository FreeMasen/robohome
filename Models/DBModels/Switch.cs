using System.Collections.Generic;

namespace RoboHome.Models
{
    public class Switch
    {
        public int Id { get; set; }
        public int OnPin { get; set; }
        public int OffPin { get; set; }
        public string Name { get; set; }
        public SwitchState State { get; set; }
        public List<Flip> Flips { get; set; }
        public Switch() {}
    }
}