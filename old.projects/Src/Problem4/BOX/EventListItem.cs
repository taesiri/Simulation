using System;

namespace Problem4.BOX
{
    public class EventListItem : IComparable
    {
        public EventListItem()
        {
        }

        public EventListItem(EventType eventType, BoxItem box, DateTime time)
        {
            EventType = eventType;
            Box = box;
            Time = time;
        }

        public EventType EventType { get; set; }
        public BoxItem Box { get; set; }
        public DateTime Time { get; set; }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var otherObj = obj as EventListItem;
            if (otherObj == null)
            {
                throw new Exception("The other Object is not EventListItem");
            }
            return Time.CompareTo(otherObj.Time);
        }

        #endregion
    }

    public enum EventType
    {
        Arrival,
        Resume,
        Departure
    }
}