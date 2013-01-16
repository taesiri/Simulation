using System;
using System.Windows;

namespace FinalProject.SimulationElements.Dialog
{
    /// <summary>
    ///     Interaction logic for BoxDetails.xaml
    /// </summary>
    public partial class BoxDetails
    {
        public BoxDetails()
        {
            InitializeComponent();
        }

        public BoxDetails(BoxInformation data)
        {
            InitializeComponent();

            TxtBoxName.Text = data.Name;

            TxtArrivalDate.Text = data.ArrivalTime.ToString("h:mm:ss");
            TxtDepartureDate.Text = data.DepartureTime.ToString("h:mm:ss");

            TxtSsa.Text = data.StationAServiceStartTime.ToString("h:mm:ss");
            TxtSea.Text = data.StationAServiceEndTime.ToString("h:mm:ss");

            TxtSsb.Text = data.StationBServiceStartTime.ToString("h:mm:ss");
            TxtSeb.Text = data.StationBServiceEndTime.ToString("h:mm:ss");

            TxtSsc.Text = data.StationCServiceStartTime.ToString("h:mm:ss");
            TxtSec.Text = data.StationCServiceEndTime.ToString("h:mm:ss");

            TimeSpan totalService = data.GetServiceDurationA + data.GetServiceDurationB + data.GetServiceDurationC;

            TxtTotalServiceTime.Text = totalService.ToString();

            TxtWastedTime.Text = (data.DepartureTime - data.ArrivalTime - totalService).ToString();
        }


        private void CloseBtnClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}