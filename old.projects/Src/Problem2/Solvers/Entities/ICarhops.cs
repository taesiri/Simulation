namespace Problem2.Solvers.Entities
{
    public abstract class Carhops
    {
        private ICustomer _customer;
        public int UtilizationTime;
        public int TotalNumberOfCustomer;
        public virtual bool IsIdle { get; set; }

        public virtual ICustomer CurrentCustomer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                if (value != null)
                {
                    UtilizationTime += value.ServiceTime;
                    TotalNumberOfCustomer++;
                }
            }
        }
    }
}