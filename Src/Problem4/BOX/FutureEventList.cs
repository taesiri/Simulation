using System.Collections.Generic;

namespace Problem4.BOX
{
    public class FutureEventList
    {
        public FutureEventList()
        {
            DataList = new LinkedList<EventListItem>();
        }

        public FutureEventList(IEnumerable<EventListItem> data)
        {
            DataList = new LinkedList<EventListItem>(data);
        }

        public LinkedList<EventListItem> DataList { get; set; }


        public EventListItem GetNearFutureEvent
        {
            get { return DataList.First.Value; }
        }
    }
}