using System;
using FinalProject.SimulationElements.Enums;

namespace FinalProject.SimulationElements.FutureEventList
{
    public class FutureEvent : IComparable
    {
        public FutureEvent()
        {
        }

        public FutureEvent(Events futureEvent, DateTime time)
        {
            Event = futureEvent;
            Time = time;
        }

        public Events Event { get; set; }
        public DateTime Time { get; set; }

        public int CompareTo(object obj)
        {
            var otherObject = obj as FutureEvent;
            if (otherObject == null)
                throw new Exception("The Other Object is not Type of FutureEvent");

            return Time.CompareTo(otherObject.Time);
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", Event.ToString(), Time.ToString("h:mm:ss"));
        }
    }
}