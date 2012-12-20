using System;
using System.Collections.Generic;

namespace Problem4.BOX
{
    public class BoxItem : IComparable
    {
        public BoxItem(DateTime arrivalDate, Priority priority, TimeSpan totalServiceTime)
        {
            ArrivalDate = arrivalDate;
            Priority = priority;
            TotalServiceTime = totalServiceTime;
        }

        public DateTime ArrivalDate { get; set; }
        public DateTime ServiceStartDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public Priority Priority { get; set; }

        public DateTime ServiceInterruptTime { get; set; }

        public TimeSpan TotalServiceTime { get; set; }
        public List<TimeRange> ServiceTimeScale { get; set; }

        public int OnArrivalQueueCount { get; set; }
        public int OnDepartureQueueCount { get; set; }

        public TimeSpan InQueuetime
        {
            get
            {
                TimeSpan time = DepartureTime.Subtract(ArrivalDate);
                return time - TotalServiceTime;
            }
        }

        public string Identifier { get; set; }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var otherObj = obj as BoxItem;
            if (otherObj == null)
            {
                throw new Exception("the Other Object is not BoxItem");
            }
            return ArrivalDate.CompareTo(otherObj.ArrivalDate);
        }

        #endregion
    }
}