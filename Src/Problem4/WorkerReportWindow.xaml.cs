using System;
using System.Data;
using System.IO;
using System.Windows.Xps.Packaging;
using CodeReason.Reports;
using Problem4.BOX;

namespace Problem4
{
    /// <summary>
    /// Interaction logic for WorkerReportWindow.xaml
    /// </summary>
    public partial class WorkerReportWindow
    {
        private readonly SolvedData _data;
        private bool _firstActivated = true;

        public WorkerReportWindow(SolvedData data)
        {
            InitializeComponent();
            _data = data;
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            if (!_firstActivated) return;

            _firstActivated = false;

            //Sort Data!
            _data.Sort();

            try
            {
                var reportDocument = new ReportDocument();

                var reader =
                    new StreamReader(new FileStream(@"Templates\WorkerReportTemplate.xaml", FileMode.Open,
                                                    FileAccess.Read));
                reportDocument.XamlData = reader.ReadToEnd();
                reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, @"Templates\");
                reader.Close();

                var data = new ReportData();
                data.ReportDocumentValues.Add("PrintDate", DateTime.Now); // print date is now

                //TABLE Goes Here

                var table = new DataTable("BoxesTable");
                table.Columns.Add("no", typeof (string));
                table.Columns.Add("Arrive", typeof (string));
                table.Columns.Add("ServiceStart", typeof (string));
                table.Columns.Add("ServiceDuration", typeof (string));
                table.Columns.Add("WaitingTime", typeof (string));
                table.Columns.Add("Priority", typeof (string));
                table.Columns.Add("Departure", typeof (string));

                int counter = 1;
                var totalServices = new TimeSpan();

                foreach (BoxItem element in _data.BoxItems)
                {
                    // Add Element To the Table
                    table.Rows.Add(new object[]
                                       {
                                           counter, element.ArrivalDate.ToString("T"),
                                           element.ServiceStartDate.ToString("T"),
                                           element.TotalServiceTime, 0.ToString(), element.Priority,
                                           element.DepartureTime.ToString("T")
                                       });
                    counter++;
                    totalServices = totalServices.Add(element.TotalServiceTime);
                }
                table.Rows.Add(new object[]
                                   {
                                       "Total", "---", "---", totalServices.TotalHours.ToString() + " Hours", "---",
                                       "---",
                                       "---"
                                   });

                data.DataTables.Add(table);

                // CHART
                var mtable = new DataTable("QueueStateTable");
                mtable.Columns.Add("DateTime", typeof (string));
                mtable.Columns.Add("InQueueBoxes", typeof (int));

                foreach (QueueState item in _data.QueueStates)
                {
                    mtable.Rows.Add(new object[] {item.Time.ToString("T"), item.InQueues.Count});

                }
                data.DataTables.Add(mtable);


                DateTime dateTimeStart = DateTime.Now; // start time measure here

                XpsDocument xps = reportDocument.CreateXpsDocument(data);
                documentViewer.Document = xps.GetFixedDocumentSequence();

                // show the elapsed time in window title
                Title += string.Format(" - Generated in {0}ms", (DateTime.Now - dateTimeStart).TotalMilliseconds);
            }
            catch (Exception exp)
            {
            }
        }
    }
}