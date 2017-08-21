using System;

namespace RoboHome.Models
{
    public class Time
    {
        private int _hour;
        private int _minute;
        public int Hour { 
            get
            {
                return _hour;
            } 
            set
            {
                this._hour = value;
                this.TimeOfDay = TimeOfDay.AM;
                while (this._hour > 12)
                {
                    this._hour -= 12;
                    this.flipTimeOfDay();
                }
                while (this._hour < 0)
                {
                    this._hour -= 12;
                    this.flipTimeOfDay();
                }
            } 
        }
        public int Minute { 
            get 
            {
                return _minute;
            } 
            set
            {
                this._minute = value;
                while (this._minute > 59)
                {
                    this._minute -= 60;
                    this.Hour++;
                }
                while (this._minute < 0)
                {
                    this._minute += 60;
                    this.Hour--;
                }
            }
        }
        public TimeOfDay TimeOfDay { get; set; }
        public TimeType TimeType { get; set; }
        public static Time Noon
        {
            get
            {
                return new Time() {
                    Hour = 12,
                    Minute = 0,
                    TimeOfDay = TimeOfDay.PM,
                    TimeType = TimeType.Noon
                };
            }
        }
        public static Time Midnight
        {
            get
            {
                return new Time() {
                    Hour = 12,
                    Minute = 0,
                    TimeOfDay = TimeOfDay.AM,
                    TimeType = TimeType.Midnight
                };
            }
        }

        private void flipTimeOfDay() 
        {
            if (this.TimeOfDay == TimeOfDay.AM)
            {
                this.TimeOfDay = TimeOfDay.PM;
            } 
            else
            {
                this.TimeOfDay = TimeOfDay.AM;
            }
        }
    }

    public enum TimeType
    {
        Custom,
        Dawn,
        Sunrise,
        Noon,
        Sunset,
        Dusk,
        Midnight
        
    }

}