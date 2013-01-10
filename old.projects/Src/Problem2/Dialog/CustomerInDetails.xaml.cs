using System.Windows;
using Problem2.Solvers.Entities;

namespace Problem2.Dialog
{
    /// <summary>
    /// Interaction logic for CustomerInDetails.xaml
    /// </summary>
    public partial class CustomerInDetails
    {
        public CustomerInDetails(ICustomer customer)
        {
            InitializeComponent();

            txt_CustomerName.Text = customer.GetFullName;
            txtArrivalTime.Text = customer.ArrivalTime.ToString();
            txtQueueTime.Text = customer.WaitTime.ToString();
            txtServiceTime.Text = customer.ServiceTime.ToString();
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}