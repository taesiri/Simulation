using System;
using System.Collections.Generic;
using FinalProject.SimulationElements.Enums;
using FinalProject.SimulationElements.RandomGenerator;
using FinalProject.SimulationWorld;

namespace FinalProject.SimulationElements.FutureEventList
{
    public class FutureEventList
    {
        public DateTime EndTime;

        public FutureEventList()
        {
            EventList = new List<FutureEvent>();
            EndTime = DateTime.Today + TimeSpan.FromHours(16);
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


            if (World.Instance.MnuChkFastEntrance.IsChecked)
            {
                while (timeBetweenTwoEnter.TotalMinutes > 30)
                {
                    timeBetweenTwoEnter = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(12)));
                }
            }


            DateTime entraneTime = currentTime.Add(timeBetweenTwoEnter);
            if (entraneTime > EndTime)
            {
                //NO More Entrance!
            }
            else
            {
                EventList.Add(new FutureEvent(Events.Arrival, currentTime.Add(timeBetweenTwoEnter)));
            }
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