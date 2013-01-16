using System.Collections.Generic;
using FinalProject.SimulationElements.Enums;

namespace FinalProject.Simulator
{
    public class Inspector
    {
        public Queue<Box> InspectorQueue;

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

        public void PushBoxToQueue(Box box)
        {
            InspectorQueue.Enqueue(box);
        }
    }
}