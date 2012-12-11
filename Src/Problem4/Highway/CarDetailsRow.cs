using System.ComponentModel;

namespace Problem4.Highway
{
    public class CarDetailsRow
    {
        public CarDetailsRow(string name, int start, int duration, int interArrivalTime, CarType type)
        {
            CarName = name;
            TripStartTime = start;
            TripDuration = duration;
            InterArrivalTime = interArrivalTime;

            CarType = type;
        }

        public string CarName { get; set; }
        public int TripStartTime { get; set; }
        public int InterArrivalTime { get; set; } // the Difference time between this Trip and last Trip
        public int TripDuration { get; set; }

        public int TripEndTime
        {
            get { return TripStartTime + TripDuration; }
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