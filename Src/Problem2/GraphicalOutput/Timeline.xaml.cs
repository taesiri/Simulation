using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Problem2.Dialog;
using Problem2.Solvers.Entities;

namespace Problem2.GraphicalOutput
{
    /// <summary>
    /// Interaction logic for Timeline.xaml
    /// </summary>
    public partial class Timeline
    {
        private readonly ObservableCollection<TimeBarItem> _listOfData =
            new ObservableCollection<TimeBarItem>();

        private List<ICustomer> _customerData;
        public Timeline(List<ICustomer> customers)
        {
            InitializeComponent();
            customers.BubbleSort();
            _customerData = new List<ICustomer>(customers);

            var simulationLen = customers[customers.Count-1].DepartureTime+10;
            timelineColumn.Width = simulationLen + 10;
            if (simulationLen < 100000)
            {
                timelineColumn.Width = (simulationLen*8) + 10;
            }
            var counter = 1;
            foreach (var customer in customers)
            {
                var len = customer.DepartureTime - customer.ArrivalTime;

                var left = customer.ArrivalTime;
                var right = simulationLen - customer.DepartureTime;

                if (simulationLen < 100000)
                {
                    len *= 8;
                    left *= 8;
                    right *= 8;
                }
               

                _listOfData.Add(new TimeBarItem(customer.GetFullName,customer.OnArrivalSystemState.ToString(), len, left, right, counter));
                counter++;
            }
        }

        public ObservableCollection<TimeBarItem> ListOfData
        {
            get { return _listOfData; }
        }

        private void LstvTimeLineMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstvTimeLine.SelectedIndex != -1)
            {
                var item = lstvTimeLine.SelectedItem as TimeBarItem;
                if (item != null)
                {
                    var customerForm = new CustomerInDetails(_customerData[item.Number-1]);
                    customerForm.ShowDialog();
                }
            }
        }
    }

}