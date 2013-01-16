using System.Collections.Generic;

namespace FinalProject.Simulator
{
    public class Entrance
    {
        public Queue<Box> EntranceQueue;

        public Entrance()
        {
            EntranceQueue = new Queue<Box>();
        }

        public int GetQueueLen
        {
            get { return EntranceQueue.Count; }
        }

        public string Name { get; set; }

        public void AddNewBox(Box box)
        {
            EntranceQueue.Enqueue(box);
        }

        public Box Dequeue()
        {
            Box item = EntranceQueue.Dequeue();
            return item;
        }
    }
}