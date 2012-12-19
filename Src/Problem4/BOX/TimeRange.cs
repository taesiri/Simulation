using System;

namespace Problem4.BOX
{
    public class TimeRange
    {
        public TimeRange()
        {
            Min = TimeSpan.FromMinutes(1);
            Max = TimeSpan.FromMinutes(60);
        }

        public TimeRange(int min, int max)
        {
            Min = TimeSpan.FromMinutes(min);
            Max = TimeSpan.FromMinutes(max);
        }

        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
    }
}