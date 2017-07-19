using System;
namespace RoboHome.Models
{
    public class Flip
    {
        public int Id { get; set; }
        public LightState Direction { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public TimeOfDay TimeOfDay { get; set; }

        //For EF
        private Flip() {}
        public Flip(int id, LightState direction, int hour, int minute, TimeOfDay tod) {
            Id = id;
            Direction = direction;
            Hour = hour;
            Minute = minute;
            TimeOfDay = tod;
        }
    }
}