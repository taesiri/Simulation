using System;
using System.Collections.Generic;

namespace Problem4.BOX
{
    public class FutureEventList
    {
        public FutureEventList()
        {
            DataList = new List<EventListItem>();
        }

        public FutureEventList(IEnumerable<EventListItem> data)
        {
            DataList = new List<EventListItem>(data);
        }

        public List<EventListItem> DataList { get; set; }

        public EventListItem GetNearFutureEvent
        {
            get
            {
                try
                {
                    DataList.BubbleSort();
                    return DataList[0];
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public int GetLen
        {
            get { return DataList.Count; }
        }

        public void PushEvent(EventListItem item)
        {
            DataList.Add(item);
            DataList.BubbleSort();
        }

        public EventListItem PopEvent()
        {
            DataList.BubbleSort();
            EventListItem rItem = DataList[0];
            DataList.Remove(rItem);
            return rItem;
        }
    }
}