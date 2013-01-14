using System.Collections.Generic;
using System.Collections.ObjectModel;
using FinalProject.SimulationElements.FutureEventList;

namespace FinalProject.SimulationElements.Dialog
{
    /// <summary>
    ///     Interaction logic for FutureEventListViewer.xaml
    /// </summary>
    public partial class FutureEventListViewer
    {
        public ObservableCollection<FutureEvent> Data = new ObservableCollection<FutureEvent>();

        public FutureEventListViewer()
        {
            InitializeComponent();
        }

        public ObservableCollection<FutureEvent> GetData
        {
            get { return Data; }
        }

        public void SetData(List<FutureEvent> images)
        {
            images.BubbleSort();
            Data.Clear();
            foreach (FutureEvent futureEvent in images)
            {
                Data.Add(futureEvent);
            }
        }
    }
}