

namespace RoboHome.Models
{
    public struct KeyTime {
        public int Hours { get; set; }
        public int Minutes { get; set; }
    }
    public class KeyTimes
    {
        public System.DateTime Date { get; set; }
        public KeyTime Dawn { get; set; }
        public KeyTime Light { get; set; }
        public KeyTime Dusk { get; set; }
        public KeyTime Dark { get; set; }

    }



}