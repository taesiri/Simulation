using System;
using System.Collections.Generic;
namespace Problem1
{
    public class EventList
    {
        private readonly List<Tuple<int,int,int,int,int>> _mainList;

        public EventList()
        {
            _mainList = new List<Tuple<int, int, int, int, int>>();
        }

        public void PushEvent(Tuple<int,int,int,int,int> item)
        {
            _mainList.Add(item);
        }
        public List<Tuple<int, int, int, int, int>> ReturnData()
        {
            return _mainList;
        }
    }
}
