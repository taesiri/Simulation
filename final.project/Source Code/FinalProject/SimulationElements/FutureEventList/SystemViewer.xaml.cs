using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace FinalProject.SimulationElements.FutureEventList
{
    /// <summary>
    ///     Interaction logic for SystemViewer.xaml
    /// </summary>
    public partial class SystemViewer : Window
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