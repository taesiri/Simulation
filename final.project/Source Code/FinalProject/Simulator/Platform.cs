using System;
using FinalProject.SimulationElements;
using FinalProject.SimulationElements.Enums;

namespace FinalProject.Simulator
{
    public class Platform
    {
        private TimeSpan _totalServiceTime;
        public string Name { get; set; }
        public ServiceBoxElement InnerElement { get; set; }
        public StationStatus Status { get; set; }

        public bool IsEmpty
        {
            get
            {
                if (ServiceBox != null)
                    return false;
                return true;
                //if (Status == StationStatus.Empty) return true;
                //return false;
            }
        }

        public bool IsBlockedOrAwaiting
        {
            get
            {
                if (Status == StationStatus.WaitingforRobot)
                    return true;
                if (Status == StationStatus.Blocked)
                    return true;
                if (Status == StationStatus.BlockedAndWaitingforRobot)
                    return true;
                return false;
            }
        }

        public TimeSpan GetTotalServiceTime
        {
            get { return _totalServiceTime; }
        }

        public Box ServiceBox { get; set; }

        public void AddServiceTime(TimeSpan time)
        {
            _totalServiceTime += time;
        }
    }
}