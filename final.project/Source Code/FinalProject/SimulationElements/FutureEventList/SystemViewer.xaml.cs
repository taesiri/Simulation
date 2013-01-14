using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FinalProject.SimulationElements.FutureEventList
{
    /// <summary>
    ///     Interaction logic for SystemViewer.xaml
    /// </summary>
    public partial class SystemViewer
    {
        public ObservableCollection<SystemImage> Data = new ObservableCollection<SystemImage>();

        public SystemViewer()
        {
            InitializeComponent();
        }

        public ObservableCollection<SystemImage> GetData
        {
            get { return Data; }
        }

        public void SetData(List<SystemImage> images)
        {
            foreach (SystemImage systemImage in images)
            {
                Data.Add(systemImage);
            }
        }
    }
}