using System;
using System.Collections.Generic;

namespace Problem4.BOX
{
    public class BoxItem
    {
        public BoxItem(DateTime arrivalDate, Priority priority, TimeSpan totalServiceTime)
        {
            ArrivalDate = arrivalDate;
            Priority = priority;
            TotalServiceTime = totalServiceTime;
        }

        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public Priority Priority { get; set; }

        public TimeSpan TotalServiceTime { get; set; }
        public List<TimeRange> ServiceTimeScale { get; set; }
    }
}