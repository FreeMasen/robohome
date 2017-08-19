using System;
namespace RoboHome.Models
{
    public class Flip
    {
        public int Id { get; set; }
        public SwitchState Direction { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public Time Time { get; set; }

        //For EF
        public Flip(int id, SwitchState direction, int hour, int minute, TimeOfDay tod) {
            this.Id = id;
            this.Direction = direction;
            this.Hour = hour;
            this.Minute = minute;
            this.TimeOfDay = tod;
        }
    }
}