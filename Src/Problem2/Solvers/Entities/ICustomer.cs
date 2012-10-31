namespace Problem2.Solvers.Entities
{
    public interface ICustomer
    {
        int ArrivalTime { get; set; }
        int ServiceTime { get; set; }
        int DepartureTime { get; set; }
        int WaitTime { get; set; }
        ICarhops ServiceProvider { get; set; }
    }
}