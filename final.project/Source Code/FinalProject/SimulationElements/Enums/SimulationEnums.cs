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
        BlockedOrAwaiting = 2
    }

    public enum WorkerStatus
    {
        Idle = 0,
        Busy = 1
    }
}