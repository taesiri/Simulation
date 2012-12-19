using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool RemoveEvent(BoxItem box, EventType eventType)
        {
            EventListItem delItem = DataList.FirstOrDefault(eventListItem => eventListItem.Box.Identifier == box.Identifier && eventListItem.EventType == eventType);
            if (delItem != null)
            {
                DataList.Remove(delItem);
                return true;
            }
            return false;
        }
    }
}