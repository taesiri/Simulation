using System;

namespace Problem2.Solvers.Entities
{
    public interface ICustomer : IComparable
    {
        int ArrivalTime { get; set; }
        int ArrivalTimeNumber { get; set; }
        int ServiceTime { get; set; }
        int DepartureTime { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string GetFullName { get; }
        int WaitTime { get; set; }

        Carhops ServiceProvider { get; set; }

        State OnArrivalSystemState { get; set; }
        State AfterDepartureState { get; set; }

    }
}