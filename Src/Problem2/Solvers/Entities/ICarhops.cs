namespace Problem2.Solvers.Entities
{
    public interface ICarhops 
    {
        bool IsIdle { get; set; }
        ICustomer CurrentCustomer { get; set; }
    }
}
