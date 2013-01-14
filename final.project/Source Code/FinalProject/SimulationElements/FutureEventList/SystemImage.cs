using FinalProject.SimulationElements.Enums;

namespace FinalProject.SimulationElements.FutureEventList
{
    public class SystemImage
    {
        public SystemImage()
        {
            EntranceQueueLen = 0;
            InspectorQueueLen = 0;
            Inspector1Status = WorkerStatus.Idle;
            Inspector2Status = WorkerStatus.Idle;
            StationAStatus = StationStatus.Empty;
            StationBStatus = StationStatus.Empty;
            StationCStatus = StationStatus.Empty;
        }

        public int EntranceQueueLen { get; set; }
        public int InspectorQueueLen { get; set; }

        public WorkerStatus Inspector1Status { get; set; }
        public WorkerStatus Inspector2Status { get; set; }

        public StationStatus StationAStatus { get; set; }
        public StationStatus StationBStatus { get; set; }
        public StationStatus StationCStatus { get; set; }

        public FutureEvent CurrentEvent { get; set; }
        public RobotStatus RobotStatus { get; set; }
        public string RobotLastActivity { get; set; }
    }
}