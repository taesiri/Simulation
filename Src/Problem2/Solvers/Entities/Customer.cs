namespace Problem2.Solvers.Entities
{
    public class Customer : ICustomer
    {
        public Customer(int arrivalTime, int serviceTime)
        {
            ArrivalTime = arrivalTime;
            ServiceTime = serviceTime;
            DepartureTime = 0;
            WaitTime = 0;
        }

        public int ArrivalTime { get; set; }
        public int ServiceTime { get; set; }
        public int DepartureTime { get; set; }
        public int WaitTime { get;  set; }
        public ICarhops ServiceProvider { get; set; }
    }
}
