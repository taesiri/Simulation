using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Xps.Packaging;
using CodeReason.Reports;
using Problem2.Solvers.Entities;

namespace Problem2
{
    /// <summary>
    /// Interaction logic for Method1Report.xaml
    /// </summary>
    public partial class Method1Report
    {
        private readonly List<ICustomer> _customerData;
        private readonly List<Carhops> _carhopses;

        private bool _firstActivated = true;


        public Method1Report(IEnumerable<ICustomer> data, IEnumerable<Carhops> carhopses )
        {
            InitializeComponent();
            _customerData = new List<ICustomer>(data);
            _carhopses = new List<Carhops>(carhopses);
            _customerData.BubbleSort();
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            if (!_firstActivated) return;

            _firstActivated = false;

            try
            {
                var reportDocument = new ReportDocument();

                var reader =
                    new StreamReader(new FileStream(@"Templates\ReportTemplate-Method1.xaml", FileMode.Open,
                                                    FileAccess.Read));
                reportDocument.XamlData = reader.ReadToEnd();
                reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, @"Templates\");
                reader.Close();

                var data = new ReportData();
                data.ReportDocumentValues.Add("PrintDate", DateTime.Now); // print date is now


                var table = new DataTable("CustomersTable");
                table.Columns.Add("no", typeof (string));
                table.Columns.Add("ArrivalTime", typeof (string));
                table.Columns.Add("rArrivalNumber", typeof (string));
                table.Columns.Add("ServiceTime", typeof (string));
                table.Columns.Add("WaitTime", typeof (string));
                table.Columns.Add("ServiceProvider", typeof (string));
                table.Columns.Add("DepartureTime", typeof (string));
                table.Columns.Add("AState", typeof(string));
                table.Columns.Add("DState", typeof(string));
                int counter = 1;
                foreach (ICustomer element in _customerData)
                {
                    // Add Element To the Table
                    table.Rows.Add(new object[]
                                       {
                                           counter, element.ArrivalTime, element.ArrivalTimeNumber, element.ServiceTime,
                                           element.WaitTime,
                                           element.ServiceProvider.ToString(), element.DepartureTime,
                                           element.OnArrivalSystemState.ToString(), element.AfterDepartureState
                                       });
                    counter++;
                }
                table.Rows.Add(new object[] {"Total", "---", "---", "---", "---", "---"});

                data.DataTables.Add(table);


                table = new DataTable("CustomersTableNL");
                table.Columns.Add("no", typeof(string));
                table.Columns.Add("FirstName", typeof(string));
                table.Columns.Add("LastName", typeof(string));
                table.Columns.Add("ArrivalTime", typeof(string));
                table.Columns.Add("ServiceTime", typeof(string));
                table.Columns.Add("WaitTime", typeof(string));
                table.Columns.Add("DepartureTime", typeof(string));
                table.Columns.Add("ServiceProvider", typeof(string));
                counter = 1;
                foreach (ICustomer element in _customerData)
                {
                    // Add Element To the Table
                    table.Rows.Add(new object[]
                                       {
                                           counter, element.FirstName, element.LastName, element.ArrivalTime,
                                           element.ServiceTime,
                                           element.WaitTime, element.DepartureTime,
                                           element.ServiceProvider.ToString()
                                       });
                    counter++;
                }
                table.Rows.Add(new object[] { "Total", "---", "---", "---", "---", "---" });

                data.DataTables.Add(table);


                table = new DataTable("CarhopsTable");
                table.Columns.Add("objective", typeof(string));
                table.Columns.Add("able", typeof(string));
                table.Columns.Add("baker", typeof(string));
                table.Columns.Add("charlie", typeof(string));


                table.Rows.Add(new object[] { "---", "---", "---", "---"});


                table.Rows.Add(new object[]
                                   {
                                       "Total Number of Customers",
                                       _carhopses[0].TotalNumberOfCustomer,
                                       _carhopses[1].TotalNumberOfCustomer,
                                       _carhopses[2].TotalNumberOfCustomer
                                   });

                table.Rows.Add(new object[]
                                   {
                                       "Total Minutes of Work",
                                       _carhopses[0].UtilizationTime,
                                       _carhopses[1].UtilizationTime,
                                       _carhopses[2].UtilizationTime
                                   });

              

                data.DataTables.Add(table);
                //Drawing Charts

                var mtable = new DataTable("CUtilizationTableP");
                mtable.Columns.Add("Carhop", typeof(string));
                mtable.Columns.Add("Utilization Time", typeof(int));
                mtable.Rows.Add(new object[] { _carhopses[0].ToString(), _carhopses[0].UtilizationTime });
                mtable.Rows.Add(new object[] { _carhopses[1].ToString(), _carhopses[1].UtilizationTime });
                mtable.Rows.Add(new object[] { _carhopses[2].ToString(), _carhopses[2].UtilizationTime });
                data.DataTables.Add(mtable);


                mtable = new DataTable("CUtilizationTable");
                mtable.Columns.Add("Carhop", typeof(string));
                mtable.Columns.Add("Utilization Time", typeof(int));
                mtable.Rows.Add(new object[] { _carhopses[0].ToString(), _carhopses[0].UtilizationTime });
                mtable.Rows.Add(new object[] { _carhopses[1].ToString(), _carhopses[1].UtilizationTime });
                mtable.Rows.Add(new object[] { _carhopses[2].ToString(), _carhopses[2].UtilizationTime });
                data.DataTables.Add(mtable);



                mtable = new DataTable("CUtilizationCNTable");
                mtable.Columns.Add("Carhop", typeof(string));
                mtable.Columns.Add("Total Number of Customers", typeof(int));
                mtable.Rows.Add(new object[] { _carhopses[0].ToString(), _carhopses[0].TotalNumberOfCustomer });
                mtable.Rows.Add(new object[] { _carhopses[1].ToString(), _carhopses[1].TotalNumberOfCustomer });
                mtable.Rows.Add(new object[] { _carhopses[2].ToString(), _carhopses[2].TotalNumberOfCustomer });
                data.DataTables.Add(mtable);


                mtable = new DataTable("CUtilizationCNTableP");
                mtable.Columns.Add("Carhop", typeof(string));
                mtable.Columns.Add("Total Number of Customers", typeof(int));
                mtable.Rows.Add(new object[] { _carhopses[0].ToString(), _carhopses[0].TotalNumberOfCustomer });
                mtable.Rows.Add(new object[] { _carhopses[1].ToString(), _carhopses[1].TotalNumberOfCustomer });
                mtable.Rows.Add(new object[] { _carhopses[2].ToString(), _carhopses[2].TotalNumberOfCustomer });
                data.DataTables.Add(mtable);

                DateTime dateTimeStart = DateTime.Now; // start time measure here

                XpsDocument xps = reportDocument.CreateXpsDocument(data);
                documentViewer.Document = xps.GetFixedDocumentSequence();

                // show the elapsed time in window title
                Title += string.Format(" - Generated in {0}ms", (DateTime.Now - dateTimeStart).TotalMilliseconds);
            }
            catch (Exception ex)
            {
                // show exception
                MessageBox.Show(string.Format("{0}\r\n\r\n{1}\r\n{2}", ex.Message, ex.GetType(), ex.StackTrace),
                                ex.GetType().ToString(),
                                MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
    }
}