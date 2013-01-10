using System;
using System.Collections.Generic;
using System.Linq;

namespace Problem2.Solvers.Entities
{
    public class FutureEventList
    {
        private readonly List<FutureEventListRow> _listOfEvents;

        public FutureEventList()
        {
            _listOfEvents = new List<FutureEventListRow>();

        }

        public int Length
        {
            get { return _listOfEvents.Count; }
        }

        public void SortList()
        {
            _listOfEvents.BubbleSort();
        }

        public void PushEventRow(FutureEventListRow row)
        {
            _listOfEvents.Add(row);
        }

        public FutureEventListRow GetNearestEvent()
        {
            SortList();

            return _listOfEvents[0];
        }

        public IEnumerable<FutureEventListRow> GetItem(int time)
        {
            var returnList = _listOfEvents.Where(item => item.Time == time).ToList();

            if (returnList.Count == 0)
                return null;

            return returnList;
        }

        public FutureEventListRow GetNearestEvent(int time)
        {
            throw new NotImplementedException();
            //SortList();

            //foreach (var item in _listOfEvents)
            //{
            //   // if (GetItem)
            //}

            //return _listOfEvents[0];
        }

        public void RemoveFirstElement()
        {
            _listOfEvents.RemoveAt(0);
        }

        public void RemoveElementAt(int id)
        {
            _listOfEvents.RemoveAt(id);
        }

        public void PushEventRow(FutureEventListRow row, bool reSort)
        {
            _listOfEvents.Add(row);
            if (reSort) SortList();
        }
        public FutureEventListRow GetElementAt(int id)
        {
            return _listOfEvents[id];
        }

        public IEnumerable<FutureEventListRow> GetElementByTime(int time)
        {
            return _listOfEvents.Where(element => element.Time == time).ToList();
        }
    }

    public class FutureEventListRow : IComparable
    {
        public EventType EventType { get; set; }
        public ICustomer Customer { get; set; }
        public int Time { get; set; }

        public int CompareTo(object obj)
        {
            var otherObject = obj as FutureEventListRow;

            if (otherObject == null)
                throw new ArgumentException("Object is not FutureEventListRow");

            return Time.CompareTo(otherObject.Time);
        }
    }
}