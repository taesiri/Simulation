namespace Problem4.BOX
{
    public class EventListItem
    {
        public int ArrivalDate { get; set; }
        public int ServiceTime { get; set; }
        public Proiority Proiority { get; set; }



    }

    public enum Proiority
    {
        High,
        Normal
    }
}