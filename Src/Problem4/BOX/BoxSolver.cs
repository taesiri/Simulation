using System;
using System.Collections.Generic;
using Problem4.Generator;

namespace Problem4.BOX
{
    public class BoxSolver
    {
        private readonly RandomGen _randomGen;


        private readonly List<BoxItem> _solvedData;
        public FutureEventList FutureEventList;
        private bool _isSolved;

        public BoxSolver(TimeSpan totalTime)
        {
            _randomGen = new RandomGen();
            _solvedData = new List<BoxItem>();

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


                Guid g = Guid.NewGuid();
                string guidString = Convert.ToBase64String(g.ToByteArray());
                guidString = guidString.Replace("=", "");
                guidString = guidString.Replace("+", "");
                currentBox.Identifier = guidString;

                FutureEventList.PushEvent(new EventListItem(EventType.Arrival, currentBox, currentArrival + arrivalTime));
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

                Guid g = Guid.NewGuid();
                string guidString = Convert.ToBase64String(g.ToByteArray());
                guidString = guidString.Replace("=", "");
                guidString = guidString.Replace("+", "");
                currentBox.Identifier = guidString;


                FutureEventList.PushEvent(new EventListItem(EventType.Arrival, currentBox, currentArrival));

                totalTime = totalTime.Subtract(serviceTime);
                //counter = counter.Add(serviceTime);
            }
        }


        public void TrySolve()
        {
            _isSolved = true;

            bool isIdle = true;

            var boxQueue = new Queue<BoxItem>();
            var resumeQueue = new Queue<BoxItem>();

            BoxItem currentBox = null;

            while (FutureEventList.GetLen > 0)
            {
                EventListItem currentEvent = FutureEventList.PopEvent();

                if (currentEvent.EventType == EventType.Arrival)
                {
                    if (isIdle)
                    {
                        // Worker is FREE!
                        currentBox = currentEvent.Box;

                        currentBox.ServiceStartDate = currentEvent.Time;
                        DateTime departureDate = currentEvent.Box.ArrivalDate + currentEvent.Box.TotalServiceTime;


                        var departureEvent = new EventListItem(EventType.Departure, currentBox, departureDate);

                        FutureEventList.PushEvent(departureEvent);
                        isIdle = false;
                    }
                    else
                    {
                        if (currentEvent.Box.Priority == Priority.High)
                        {
                            currentEvent.Box.ServiceStartDate = currentEvent.Box.ArrivalDate;
                            DateTime departureDate = currentEvent.Box.ArrivalDate + currentEvent.Box.TotalServiceTime;
                            var departureEvent = new EventListItem(EventType.Departure, currentEvent.Box, departureDate);

                            if (currentBox != null) // ALWAYS TRUE
                            {
                                currentBox.ServiceInterruptTime = currentEvent.Time;

                                DateTime resumeDate = departureDate;
                                var resumeEvent = new EventListItem(EventType.Resume, currentBox, resumeDate);

                                if (!FutureEventList.RemoveEvent(currentBox, EventType.Departure))
                                {
                                    throw new Exception("Error!");
                                }

                                FutureEventList.PushEvent(resumeEvent);
                            }
                            FutureEventList.PushEvent(departureEvent);
                            currentBox = currentEvent.Box;
                        }
                        else if (currentEvent.Box.Priority == Priority.Normal)
                        {
                            // Normal BOX!
                            boxQueue.Enqueue(currentEvent.Box);
                        }
                    }
                }
                else if (currentEvent.EventType == EventType.Departure)
                {
                    BoxItem solvedBox = currentEvent.Box;
                    solvedBox.DepartureTime = currentEvent.Time;
                    _solvedData.Add(solvedBox);

                    if (resumeQueue.Count > 0 && currentEvent.Box.Priority == Priority.High)
                    {
                        //Resume
                        BoxItem b = resumeQueue.Dequeue();
                        DateTime currentDate = currentEvent.Time;

                        TimeSpan remainingWorkTime = b.TotalServiceTime - (b.ServiceInterruptTime - b.ServiceStartDate);

                        DateTime departureDate = currentDate + remainingWorkTime;

                        var departureEvent = new EventListItem(EventType.Departure, b, departureDate);
                        FutureEventList.PushEvent(departureEvent);
                    }
                    else
                    {
                        if (boxQueue.Count > 0)
                        {
                            currentBox = boxQueue.Dequeue();
                            currentBox.ServiceStartDate = currentEvent.Time;
                            DateTime departureDate = currentBox.ServiceStartDate + currentBox.TotalServiceTime;
                            var departureEvent = new EventListItem(EventType.Departure, currentBox, departureDate);
                            FutureEventList.PushEvent(departureEvent);
                        }
                        else
                        {
                            isIdle = true;
                            currentBox = null;
                        }
                    }
                }
                else if (currentEvent.EventType == EventType.Resume)
                {
                    resumeQueue.Enqueue(currentEvent.Box);
                }
            }
        }

        public List<BoxItem> GetAnswer()
        {
            if (!_isSolved)
                throw new Exception("The Case isn't Solved!");

            return _solvedData;
        }
    }
}