namespace FinalProject.SimulationElements.Enums
{
    public enum Events
    {
        BeginingOfSimulation,
        Arrival,
        LoadingBoxStartedOnStationA,
        LoadingBoxEndedOnStationA,
        LoadingBoxStartedOnStationB,
        LoadingBoxEndedOnStationB,
        LoadingBoxStartedOnStationC,
        LoadingBoxEndedOnStationC,
        ServiceStartedOnStationA,
        ServiceEndedOnStationA,
        ServiceStartedOnStationB,
        ServiceEndedOnStationB,
        ServiceStartedOnStationC,
        ServiceEndedOnStationC,
        RobotJobFinished,
        BoxArrivedToInspector,
        InspectorWorker1JobDone,
        InspectorWorker2JobDone,
        Departure,
        EndOfSimulation
    }

    public enum RobotStatus
    {
        Idle = 0,
        Busy = 1
    }

    public enum StationStatus
    {
        Empty = 0,
        OnService = 1,
        ServiceDone = 2,
        Loading = 3,
        Loaded = 4,
        Blocked = 5,
        WaitingforRobot = 6,
        BlockedAndWaitingforRobot = 7,
    }

    public enum WorkerStatus
    {
        Idle = 0,
        Busy = 1
    }
}