using System;
using System.ComponentModel;

namespace Problem4.Highway
{
    public class CarDetailsRow
    {
        public CarDetailsRow(string name, DateTime start, int duration, int interArrivalTime, CarType type)
        {
            CarName = name;
            TripStartTime = start;
            TripDuration = duration;
            InterArrivalTime = interArrivalTime;

            CarType = type;
        }

        public string CarName { get; set; }
        public DateTime TripStartTime { get; set; }
        public int InterArrivalTime { get; set; } // the Difference time between this Trip and last Trip
        public int TripDuration { get; set; }

        public DateTime TripEndTime
        {
            get { return TripStartTime.AddMinutes(TripDuration); }
        }

        public CarType CarType { get; set; }

        
    }

    public enum CarType
    {
        [Description("Capacity : 1")] C1,
        [Description("Capacity : 2")] C2,
        [Description("Capacity : 3")] C3,
        [Description("Capacity : 4")] C4,
        [Description("Capacity : 40")] C40
    }
}