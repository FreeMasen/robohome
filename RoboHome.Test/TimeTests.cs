using System;
using Xunit;
using RoboHome.Models;

namespace RoboHome.Test
{
    public class TimeTests

    {
        [Fact]
        public void TimeHourWrapTest()
        {
            var midnight = Time.Midnight;
            Assert.StrictEqual(midnight.Hour, 12);
            Assert.StrictEqual(midnight.TimeOfDay, TimeOfDay.AM);
            midnight.Hour = 15;
            Assert.StrictEqual(midnight.Hour, 3);
            Assert.StrictEqual(midnight.TimeOfDay, TimeOfDay.PM);
        }

        [Fact]
        public void TimeMinuteWrapTest()
        {
            var noon = Time.Noon;
            Assert.StrictEqual(noon.Hour, 12);
            Assert.StrictEqual(noon.Minute, 0);
            Assert.StrictEqual(noon.TimeOfDay, TimeOfDay.PM);
            noon.Minute = -5;
            Assert.StrictEqual(noon.Hour, 11);
            Assert.StrictEqual(noon.Minute, 55);
            Assert.StrictEqual(noon.TimeOfDay, TimeOfDay.AM);
        }
    }
}
