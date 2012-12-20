using System;
using System.Collections.Generic;
using Problem4.Generator;

namespace Problem4.BOX
{
    public class BoxSolver
    {
        private readonly List<BoxItem> _boxStates;
        private TimeSpan AwaitingBoxMoreThan1;
        private TimeSpan AwaitingBoxMoreThan2;

        public FutureEventList FutureEventList;
        private bool _isSolved;
        private List<QueueState> _queueStates;
        private TimeSpan _totalTime;

        public BoxSolver(TimeSpan totalTime)
        {
            _boxStates = new List<BoxItem>();
            _totalTime = totalTime;
        }

        public void GettingThingsReady()
        {
            FutureEventList = new FutureEventList();
            var currentArrival = new DateTime(2012, 12, 19, 8, 0, 0, 0);

            // Future Event List! All Arrivals!
            // First Box


            TimeSpan firstServiceTime = TimeSpan.FromMinutes((int) SimpleRng.GetNormal(16, 3));
            var firstbox = new BoxItem(currentArrival, Priority.Normal, firstServiceTime);


            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            firstbox.Identifier = guidString;

            FutureEventList.PushEvent(new EventListItem(EventType.Arrival, firstbox, currentArrival));


            // Special Boxes
            // Special Boxes Arrives every hour
            double allSpecialBoxes = _totalTime.TotalMinutes/60;
            int counter = 1;
            while (counter <= allSpecialBoxes)
            {
                TimeSpan arrivalTime = TimeSpan.FromMinutes(counter*60);

                TimeSpan serviceTime = TimeSpan.FromMinutes((int) SimpleRng.GetNormal(16, 3));


                var currentBox = new BoxItem(currentArrival + arrivalTime, Priority.High, serviceTime);


                g = Guid.NewGuid();
                guidString = Convert.ToBase64String(g.ToByteArray());
                guidString = guidString.Replace("=", "");
                guidString = guidString.Replace("+", "");
                currentBox.Identifier = guidString;

                FutureEventList.PushEvent(new EventListItem(EventType.Arrival, currentBox, currentArrival + arrivalTime));
                _totalTime = _totalTime.Subtract(serviceTime);
                counter++;
            }
            // Normal Priority Boxes!  |  All Arrivals!
            while (_totalTime.TotalMinutes > 0)
            {
                var arriveTime = (int) SimpleRng.GetNormal(14, 4);
                TimeSpan serviceTime = TimeSpan.FromMinutes((int) SimpleRng.GetNormal(9, 2));

                currentArrival += TimeSpan.FromMinutes(arriveTime);
                var currentBox = new BoxItem(currentArrival, Priority.Normal, serviceTime);

                g = Guid.NewGuid();
                guidString = Convert.ToBase64String(g.ToByteArray());
                guidString = guidString.Replace("=", "");
                guidString = guidString.Replace("+", "");
                currentBox.Identifier = guidString;


                FutureEventList.PushEvent(new EventListItem(EventType.Arrival, currentBox, currentArrival));

                _totalTime = _totalTime.Subtract(serviceTime);
                //counter = counter.Add(serviceTime);
            }
        }


        public void TrySolve()
        {
            GettingThingsReady();
            _queueStates = new List<QueueState>();
            var allQueue = new List<QueueState>();

            bool isIdle = true;

            var boxQueue = new Queue<BoxItem>();
            var resumeQueue = new Queue<BoxItem>();

            BoxItem currentBox = null;

            var lLock = new Queue<BoxItem>();

            while (FutureEventList.GetLen > 0)
            {
                EventListItem currentEvent = FutureEventList.PopEvent();

                if (boxQueue.Count > 0)
                {
                    if (lLock.Count != boxQueue.Count)
                    {
                        _queueStates.Add(new QueueState(currentEvent.Time, boxQueue));
                        lLock = new Queue<BoxItem>(boxQueue);
                    }
                }

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
                    _boxStates.Add(solvedBox);

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

                allQueue.Add(new QueueState(currentEvent.Time, boxQueue));
            }

            _queueStates.BubbleSort();
            allQueue.BubbleSort();

            var timeSpan = new TimeSpan();

            for (int i = 0; i < allQueue.Count; i++)
            {
                if (allQueue[i].InQueues.Count >= 2)
                {
                    timeSpan += allQueue[i + 1].Time - allQueue[i].Time;
                }
            }
            AwaitingBoxMoreThan2 = timeSpan;

            timeSpan = new TimeSpan();

            for (int i = 0; i < allQueue.Count; i++)
            {
                if (allQueue[i].InQueues.Count >= 1)
                {
                    timeSpan += allQueue[i + 1].Time - allQueue[i].Time;
                }
            }
            AwaitingBoxMoreThan1 = timeSpan;

            _isSolved = true;
        }

        public SolvedData GetSolution()
        {
            if (!_isSolved)
                throw new Exception("The Case isn't Solved!");

            return new SolvedData(_boxStates, _queueStates)
                       {AwaitingBoxMoreThan1 = AwaitingBoxMoreThan1, AwaitingBoxMoreThan2 = AwaitingBoxMoreThan2};
        }
    }
}