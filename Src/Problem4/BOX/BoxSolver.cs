using System;
using System.Collections.Generic;
using Problem4.Generator;

namespace Problem4.BOX
{
    public class BoxSolver
    {
        private readonly RandomGen _randomGen;
        public FutureEventList FutureEventList;

        private bool _isSolved;

        public BoxSolver(TimeSpan totalTime)
        {
            _randomGen = new RandomGen();

            FutureEventList = new FutureEventList();
            var currentArrival = new DateTime(2012, 12, 19, 8, 0, 0, 0);

            // Future Event List! All Arrivals!

            // Special Boxes
            // Special Boxes Arrives every hour
            double allSpecialBoxes = totalTime.TotalMinutes/60;
            int counter = 1;
            while (counter <= allSpecialBoxes)
            {
                TimeSpan arrivalTime = TimeSpan.FromMinutes(counter*60);
                TimeSpan serviceTime = TimeSpan.FromMinutes((int) _randomGen.Pick(16, 3));

                var currentBox = new BoxItem(currentArrival + arrivalTime, Priority.High, serviceTime);

                FutureEventList.PushEvent(new EventListItem(EventType.Arrival, currentBox, currentArrival));
                totalTime = totalTime.Subtract(serviceTime);
                counter++;
            }
            // Normal Priority Boxes!  |  All Arrivals!
            while (totalTime.TotalMinutes > 0)
            {
                var arriveTime = (int) _randomGen.Pick(14, 4);
                TimeSpan serviceTime = TimeSpan.FromMinutes((int) _randomGen.Pick(9, 2));

                currentArrival += TimeSpan.FromMinutes(arriveTime);
                var currentBox = new BoxItem(currentArrival, Priority.Normal, serviceTime);

                FutureEventList.PushEvent(new EventListItem(EventType.Arrival, currentBox, currentArrival));

                totalTime = totalTime.Subtract(serviceTime);
                //counter = counter.Add(serviceTime);
            }
        }


        public void TrySolve()
        {
            _isSolved = true;

            var smartQueue = new Queue<EventListItem>();
            bool unfinishedJob = false;
            EventListItem onHold = null;
            BoxItem currentBox = null;

            while (FutureEventList.GetLen > 0)
            {
                EventListItem currentEvent = FutureEventList.PopEvent();

                if (currentEvent.EventType == EventType.Arrival)
                {
                    if (smartQueue.Count == 0)
                    {
                        // Worker is FREE!

                        DateTime departureDate = currentEvent.Box.ArrivalDate + currentEvent.Box.TotalServiceTime;

                        var departureEvent = new EventListItem(EventType.Departure, currentEvent.Box, departureDate);
                        currentBox = currentEvent.Box;
                        FutureEventList.PushEvent(departureEvent);
                    }
                    else if (smartQueue.Count != 0)
                    {
                        if (currentEvent.Box.Priority == Priority.High)
                        {
                            // Special BOX!



                            unfinishedJob = true;
                            onHold = new EventListItem(EventType.Resume, currentBox, new DateTime());
                        }
                        else if (currentEvent.Box.Priority == Priority.Normal)
                        {
                            // Normal BOX!
                            smartQueue.Enqueue(currentEvent);
                        }
                    }
                }
                else if (currentEvent.EventType == EventType.Departure)
                {
                    if (currentEvent.Box.Priority == Priority.High)
                    {
                        if (unfinishedJob)
                        {
                            //Resume
                        }
                        else
                        {
                        }
                    }
                    else if (currentEvent.Box.Priority == Priority.High)
                    {
                    }
                }
            }
        }

        public string GetAnswer()
        {
            if (!_isSolved)
                throw new Exception("The Case isn't Solved!");

            return "ANSWER!";
        }
    }
}