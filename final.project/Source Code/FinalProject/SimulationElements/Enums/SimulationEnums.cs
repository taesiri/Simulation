namespace FinalProject.SimulationElements.Enums
{
    public enum Events
    {
        BeginingOfSimulation,
        Arrival,
        MovingToStationA,
        ServiceStartedOnStationA,
        ServiceEndedOnStationA,
        MovingToStationB,
        ServiceStartedOnStationB,
        ServiceEndedOnStationB,
        MovingToStationC,
        ServiceStartedOnStationC,
        ServiceEndedOnStationC,
        RobotJobFinished,
        BoxMovedToInspector,
        BoxLoadingStarted,
        BoxLoadingEnded,
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
        Blocked = 2,
        WaitingforRobot = 3,
        BlockedAndWaitingforRobot = 4
    }

    public enum WorkerStatus
    {
        Idle = 0,
        Busy = 1
    }
}