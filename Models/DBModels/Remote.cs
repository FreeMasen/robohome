using System.Collections.Generic;

namespace RoboHome.Models
{
    public class Remote
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public List<Switch> Switches { get; set; }
        
        public Remote(){}
    }
}