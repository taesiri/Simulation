using System;
using System.Collections.Generic;

namespace Problem4.BOX
{
    public class SolvedData
    {
        public SolvedData(IEnumerable<BoxItem> boxeItems, IEnumerable<QueueState> queueStates)
        {
            BoxItems = new List<BoxItem>(boxeItems);
            QueueStates = new List<QueueState>(queueStates);
        }

        public List<BoxItem> BoxItems { get; set; }
        public List<QueueState> QueueStates { get; set; }
        public TimeSpan AwaitingBoxMoreThan1 { get; set; }
        public TimeSpan AwaitingBoxMoreThan2 { get; set; }
        public void Sort()
        {
            BoxItems.BubbleSort();
            QueueStates.BubbleSort();
        }
    }
}