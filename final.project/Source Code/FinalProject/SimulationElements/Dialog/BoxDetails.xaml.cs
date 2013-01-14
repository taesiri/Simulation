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

        public BoxDetails(string name, DateTime time)
        {
            InitializeComponent();

            LblBoxEntranceDate.Content = time.ToString("h:mm:ss");
            LblBoxName.Content = name;
        }


        private void CloseBtnClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}