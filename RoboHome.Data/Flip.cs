using System;
namespace RoboHome.Models
{
    public class Flip
    {
        public int Id { get; set; }
        public SwitchState Direction { get; set; }
        public Time Time { get; set; }
        public int SwitchId { get; set; }
        public Flip() {}
        public Flip(int id, SwitchState direction, int hour, int minute, TimeOfDay tod) {
            this.Id = id;
            this.Direction = direction;
            this.Time = new Time() {
                Hour = hour,
                Minute = minute,
                TimeOfDay = tod
            };
        }
    }
}