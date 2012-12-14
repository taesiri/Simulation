using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private readonly List<int> _accidentdata;
        private readonly List<CarDetailsRow> _data;
        private bool _firstActivated = true;

        public HighwayReportWindow(IEnumerable<CarDetailsRow> data, IEnumerable<int> accidentdata)
        {
            InitializeComponent();
            _data = new List<CarDetailsRow>(data);
            _accidentdata = new List<int>(accidentdata);
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

                int numbOfCars = _data.Count - 1;
                int passengers = _data.Sum(carDetailsRow => carDetailsRow.GetPassengers);
                int casualties = _accidentdata.Sum(i => _data[i].GetPassengers);


                string reportSum = string.Format(
                    "There were {0} cars in the road with total of {1} Passengers.\nUnfortunately there was {2} accident in the road with {3} casualties",
                    numbOfCars, passengers, _accidentdata.Count - 1, casualties) + Environment.NewLine;

                string accident = "";


                foreach (int i in _accidentdata)
                {
                    accident +=
                        string.Format("Car : [{0}] - Passengers : {1}", _data[i].CarName, _data[i].GetPassengers) +
                        Environment.NewLine;
                }

                data.ReportDocumentValues.Add("ReportSummary", reportSum);
                data.ReportDocumentValues.Add("Accidents", accident);


                //TABLE Goes Here

                var table = new DataTable("HighwayTable");
                table.Columns.Add("no", typeof (string));
                table.Columns.Add("carModel", typeof (string));
                table.Columns.Add("tripStartTime", typeof (string));
                table.Columns.Add("tripDuration", typeof (string));
                table.Columns.Add("tripEndTime", typeof (string));
                table.Columns.Add("tripStatus", typeof (string));
                table.Columns.Add("carCapacity", typeof (string));

                int counter = 1;

                foreach (CarDetailsRow element in _data)
                {
                    // Add Element To the Table
                    table.Rows.Add(new object[]
                                       {
                                           counter, element.CarName, element.TripStartTime.ToString("T"),
                                           element.TripDuration + " Min",
                                           element.TripEndTime.ToString("T"), element.TripStatus,
                                           Helper.CarCapacity(element.CarType).ToString(CultureInfo.InvariantCulture)
                                       });
                    counter++;
                }
                table.Rows.Add(new object[] {"Total", "---", "---", "---", "---", "---", "---"});

                data.DataTables.Add(table);


                // Chart - Car by Capacity

                int totalC1 = 0;
                int totalC2 = 0;
                int totalC3 = 0;
                int totalC4 = 0;
                int totalC40 = 0;

                foreach (CarDetailsRow detailsRow in _data)
                {
                    if (detailsRow.CarType == CarType.C1)
                        totalC1++;
                    else if (detailsRow.CarType == CarType.C2)
                        totalC2++;
                    else if (detailsRow.CarType == CarType.C3)
                        totalC3++;

                    else if (detailsRow.CarType == CarType.C4)
                        totalC4++;

                    else if (detailsRow.CarType == CarType.C40)
                        totalC40++;
                }


                var mtable = new DataTable("CarsChart");
                mtable.Columns.Add("Number of Cars", typeof (string));
                mtable.Columns.Add("Passengers", typeof (int));
                mtable.Rows.Add(new object[] {"1 Passenger", totalC1});
                mtable.Rows.Add(new object[] {"2 Passengers", totalC2});
                mtable.Rows.Add(new object[] {"3 Passengers", totalC3});
                mtable.Rows.Add(new object[] {"4 Passengers", totalC4});
                mtable.Rows.Add(new object[] {"40 Passengers", totalC40});
                data.DataTables.Add(mtable);


                //


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