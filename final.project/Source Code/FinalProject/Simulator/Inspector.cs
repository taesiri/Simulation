using System;
using System.Collections.Generic;
using FinalProject.SimulationElements.Enums;

namespace FinalProject.Simulator
{
    public class Inspector
    {
        public Queue<Box> InspectorQueue;
        private TimeSpan _totalServiceTimeInspector1;
        private TimeSpan _totalServiceTimeInspector2;

        public Inspector()
        {
            InspectorQueue = new Queue<Box>();
        }

        public WorkerStatus Inspector1Status { get; set; }
        public WorkerStatus Inspector2Status { get; set; }


        public int GetQueueLen
        {
            get { return InspectorQueue.Count; }
        }


        public string ServiceProviderName { get; set; }
        public Box Inspector1Box { get; set; }
        public Box Inspector2Box { get; set; }


        public TimeSpan GetTotalServiceTime
        {
            get { return _totalServiceTimeInspector1 + _totalServiceTimeInspector2; }
        }

        public TimeSpan GetTotalServiceTimeInspector1
        {
            get { return _totalServiceTimeInspector1; }
        }

        public TimeSpan GetTotalServiceTimeInspector2
        {
            get { return _totalServiceTimeInspector2; }
        }

        public void AddServiceTimeInspector1(TimeSpan time)
        {
            _totalServiceTimeInspector1 += time;
        }

        public void AddServiceTimeInspector2(TimeSpan time)
        {
            _totalServiceTimeInspector2 += time;
        }

        public void PushBoxToQueue(Box box)
        {
            InspectorQueue.Enqueue(box);
        }
    }
}