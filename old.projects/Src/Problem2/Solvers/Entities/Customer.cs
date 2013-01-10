using System;
using Problem2.NameGenerator;

namespace Problem2.Solvers.Entities
{
    public class Customer : ICustomer
    {
        public Customer()
        {
            var gen = new GenerateName();
            FirstName = gen.GenerateFirstName();
            LastName = gen.GenerateLastName();
        }

        public Customer(int arrivalTime, int serviceTime, GenerateName gen)
        {
            FirstName = gen.GenerateFirstName();
            LastName = gen.GenerateLastName();

            ArrivalTime = arrivalTime;
            ServiceTime = serviceTime;
            DepartureTime = 0;
            WaitTime = 0;
        }

        public int ArrivalTime { get; set; }
        public int ArrivalTimeNumber { get; set; }
        public int ServiceTime { get; set; }
        public int DepartureTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GetFullName { get { return FirstName + " " + LastName; } }
        public int WaitTime { get;  set; }
        public Carhops ServiceProvider { get; set; }
        public State OnArrivalSystemState { get; set; }
        public State AfterDepartureState { get; set; }

        public override string ToString()
        {
            return
                String.Format(
                    "Customer Name : {0} {1}; Service Provider : {2}; Srevice Duration {3}; Waiting Time :{4}; Departure Time :{5}",
                    FirstName, LastName, ServiceProvider, ServiceTime, WaitTime, DepartureTime);
        }
        public int CompareTo(object obj)
        {
            var otherObject = obj as Customer;

            if (otherObject == null)
                throw new ArgumentException("Object is not FutureEventListRow");

            return ArrivalTime.CompareTo(otherObject.ArrivalTime);
        }
    }
}
