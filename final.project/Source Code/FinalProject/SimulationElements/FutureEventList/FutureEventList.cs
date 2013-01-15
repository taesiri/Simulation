using System;
using System.Collections.Generic;
using FinalProject.SimulationElements.Enums;
using FinalProject.SimulationElements.RandomGenerator;

namespace FinalProject.SimulationElements.FutureEventList
{
    public class FutureEventList
    {
        public FutureEventList()
        {
            EventList = new List<FutureEvent>();
        }

        public List<FutureEvent> EventList { get; set; }

        public FutureEvent GetImminentEvent()
        {
            if (EventList.Count != 0)
            {
                EventList.BubbleSort();

                FutureEvent eL = EventList[0];
                EventList.RemoveAt(0);
                return eL;
            }


            return new FutureEvent(Events.EndOfSimulation, new DateTime());
        }

        public void GenerateNextEntrance(DateTime currentTime)
        {
            TimeSpan timeBetweenTwoEnter = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(12))); // Const

            while (timeBetweenTwoEnter.TotalMinutes > 30)
            {
                timeBetweenTwoEnter = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(12)));
            }

            EventList.Add(new FutureEvent(Events.Arrival, currentTime.Add(timeBetweenTwoEnter)));
        }

        public void AddNewEvent(FutureEvent newEvent)
        {
            EventList.Add(newEvent);
            EventList.BubbleSort();
        }

        public List<FutureEvent> GetEventsAt(DateTime dateTime)
        {
            var returnList = new List<FutureEvent>();

            foreach (FutureEvent futureEvent in EventList)
            {
                if (futureEvent.Time == dateTime)
                {
                    returnList.Add(futureEvent);
                }
            }

            if (returnList.Count != 0)
            {
                foreach (FutureEvent futureEvent in returnList)
                {
                    EventList.Remove(futureEvent);
                }
            }

            return returnList;
        }
    }
}