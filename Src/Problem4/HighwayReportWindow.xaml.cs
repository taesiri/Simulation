using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Xps.Packaging;
using CodeReason.Reports;
using Problem4.Generator;
using Problem4.Highway;

namespace Problem4
{
    /// <summary>
    /// Interaction logic for HighwayReportWindow.xaml
    /// </summary>
    public partial class HighwayReportWindow : Window
    {
        private readonly List<CarDetailsRow> _data;
        private bool _firstActivated = true;

        public HighwayReportWindow(IEnumerable<CarDetailsRow> data)
        {
            InitializeComponent();
            _data = new List<CarDetailsRow>(data);
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            if (!_firstActivated) return;

            _firstActivated = false;

            try
            {
                var reportDocument = new ReportDocument();

                var reader =
                    new StreamReader(new FileStream(@"Templates\HighwayReportTemplate.xaml", FileMode.Open,
                                                    FileAccess.Read));
                reportDocument.XamlData = reader.ReadToEnd();
                reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, @"Templates\");
                reader.Close();

                var data = new ReportData();
                data.ReportDocumentValues.Add("PrintDate", DateTime.Now); // print date is now

                //TABLE Goes Here

                var table = new DataTable("HighwayTable");
                table.Columns.Add("no", typeof (string));
                table.Columns.Add("carModel", typeof(string));
                table.Columns.Add("tripStartTime", typeof (string));
                table.Columns.Add("tripDuration", typeof (string));
                table.Columns.Add("tripEndTime", typeof (string));
                table.Columns.Add("carCapacity", typeof (string));

                int counter = 1;

                foreach (CarDetailsRow element in _data)
                {
                    // Add Element To the Table
                    table.Rows.Add(new object[]
                                       {
                                           counter, element.CarName, element.TripStartTime.ToString("T"), element.TripDuration +" Min",
                                           element.TripEndTime.ToString("T"),
                                           Helper.CarCapacity(element.CarType).ToString(CultureInfo.InvariantCulture)
                                       });
                    counter++;
                }
                table.Rows.Add(new object[] {"Total", "---", "---", "---", "---", "---"});

                data.DataTables.Add(table);


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