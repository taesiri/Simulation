using System;
using System.Collections.Generic;
using FinalProject.SimulationElements.Enums;

namespace FinalProject.SimulationElements.FutureEventList
{
    public class FutureEventList
    {
        private DateTime _lastEnter = DateTime.Now;
        public List<FutureEvent> EventList { get; set; }

        public FutureEvent GetImminentEvent()
        {
            if (EventList.Count != 0)
            {
                EventList.BubbleSort();
                return EventList[0];
            }


            return new FutureEvent(Events.EndOfSimulation, new DateTime());
        }

        public void GenerateNextEntrance()
        {
            TimeSpan timeBetweenTwoEnter = TimeSpan.FromSeconds(1); // Const

            _lastEnter = _lastEnter.Add(timeBetweenTwoEnter);
            EventList.Add(new FutureEvent(Events.Arrival, _lastEnter));

            throw new NotImplementedException();
        }
    }
}