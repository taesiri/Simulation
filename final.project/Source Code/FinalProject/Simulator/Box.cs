using System;

namespace FinalProject.Simulator
{
    public class Box
    {
        public string Name { get; set; }
        public DateTime ArrivalTime { get; set; } // Entered to the System
        public DateTime DepartureTime { get; set; } // Left the System

        public DateTime StationAServiceEndTime { get; set; }
        public DateTime StationAServiceStartTime { get; set; } // Service Start Time on Station A
        public TimeSpan StationAServiceDuration { get; set; } // Service Duration on This Station
        public TimeSpan StationALoadDuration { get; set; } // Service Duration on This Station

        public DateTime StationBServiceEndTime { get; set; }
        public DateTime StationBServiceStartTime { get; set; } // Service Start Time on Station B
        public TimeSpan StationBServiceDuration { get; set; } // Service Duration on This Station
        public TimeSpan StationBLoadDuration { get; set; } // Service Duration on This Station

        public DateTime StationCServiceEndTime { get; set; }
        public DateTime StationCServiceStartTime { get; set; } // Service Start Time on Station C
        public TimeSpan StationCServiceDuration { get; set; } // Service Duration on This Station
        public TimeSpan StationCLoadDuration { get; set; } // Service Duration on This Station


        public DateTime EnteredToInspector { get; set; } // Moved into Inspector
        public DateTime InspectorServiceStartTime { get; set; } // Inspector Start Time
        public DateTime InspectorServiceDoneTime { get; set; } // Inspector Start Time
        public TimeSpan InspectorQueueTime { get; set; } // Inspector Queue Time
        public TimeSpan InspectorDuration { get; set; } //Inspector Service Duration

        public string Identifier { get; set; }

        public TimeSpan GetInSystemTime
        {
            get { return DepartureTime - ArrivalTime; }
        }
    }
}