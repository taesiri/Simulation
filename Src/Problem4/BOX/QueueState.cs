using System;
using System.Collections.Generic;

namespace Problem4.BOX
{
    public class QueueState : IComparable
    {
        public QueueState(DateTime time, IEnumerable<BoxItem> items)
        {
            Time = time;
            InQueues = new Queue<BoxItem>(items);
        }

        public DateTime Time { get; set; }
        public Queue<BoxItem> InQueues { get; set; }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var otherObj = obj as QueueState;
            if (otherObj == null)
                throw new Exception("The Other Object is not QueueState");

            return Time.CompareTo(otherObj.Time);
        }

        #endregion
    }
}