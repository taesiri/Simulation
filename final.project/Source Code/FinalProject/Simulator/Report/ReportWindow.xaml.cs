﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Xps.Packaging;
using CodeReason.Reports;

namespace FinalProject.Simulator.Report
{
    /// <summary>
    ///     Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow
    {
        private readonly List<SimulationResult> _data;
        private bool _firstActivated = true;

        public ReportWindow(IEnumerable<SimulationResult> data)
        {
            InitializeComponent();
            _data = new List<SimulationResult>(data);
        }


        private void WindowActivated(object sender, EventArgs e)
        {
            if (!_firstActivated) return;

            _firstActivated = false;

            try
            {
                var reportDocument = new ReportDocument();

                String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

                var reader =
                    new StreamReader(new FileStream(appStartPath + @"\Simulator\Report\Template\Template.xaml",
                                                    FileMode.Open,
                                                    FileAccess.Read));
                reportDocument.XamlData = reader.ReadToEnd();
                reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory,
                                                            appStartPath + @"\Simulator\Report\Template\");
                reader.Close();

                var data = new ReportData();
                data.ReportDocumentValues.Add("PrintDate", DateTime.Now); // print date is now


                //TABLE Goes Here

                var table = new DataTable("TableZero");
                table.Columns.Add("no", typeof (string));
                table.Columns.Add("sDate", typeof (string));
                table.Columns.Add("eDate", typeof (string));
                table.Columns.Add("simulDuration", typeof (string));
                table.Columns.Add("totalBoxes", typeof (string));
                table.Columns.Add("avrageBoxTime", typeof (string));

                int counter = 1;

                var totalAverage = new TimeSpan(0);
                int totalBoxes = 0;

                foreach (SimulationResult element in _data)
                {
                    // Add Element To the Table
                    table.Rows.Add(new object[]
                        {
                            counter, element.SimulationStartDate.ToString(CultureInfo.InvariantCulture),
                            element.LastBoxDeparture.ToString(CultureInfo.InvariantCulture),
                            (element.LastBoxDeparture - element.SimulationStartDate).ToString(),
                            element.BoxResult.Count.ToString(CultureInfo.InvariantCulture),
                            Math.Round(element.GetAverageBoxInSystemTime.TotalMinutes) + " - Minutes"
                        });
                    counter++;
                    totalAverage += element.GetAverageBoxInSystemTime;
                    totalBoxes += element.BoxResult.Count;
                }
                table.Rows.Add(new object[]
                    {
                        "Total", "---", "---", "---", totalBoxes.ToString(),
                        Math.Round(totalAverage.TotalMinutes/_data.Count) + " - Minutes Per Box"
                    });


                data.DataTables.Add(table);


                // Table 1

                table = new DataTable("TableOne");
                table.Columns.Add("no", typeof (string));
                table.Columns.Add("ATotalService", typeof (string));
                table.Columns.Add("BTotalService", typeof (string));
                table.Columns.Add("CTotalService", typeof (string));
                table.Columns.Add("InspectorTotalService", typeof (string));
                table.Columns.Add("TotalServiceTime", typeof (string));

                counter = 1;


                var averageA = new TimeSpan(0);
                var averageB = new TimeSpan(0);
                var averageC = new TimeSpan(0);
                var averageInspector = new TimeSpan(0);
                var averagetotal = new TimeSpan(0);

                var totalServiceEachRun = new List<TimeSpan>();


                foreach (SimulationResult element in _data)
                {
                    // Add Element To the Table
                    table.Rows.Add(new object[]
                        {
                            counter, element.StationATotalService, element.StationBTotalService,
                            element.StationCTotalService,
                            element.Inspector1TotalService + element.Inspector2TotalService,
                            element.TotalService
                        });
                    counter++;
                    averageA += element.StationATotalService;
                    averageB += element.StationBTotalService;
                    averageC += element.StationCTotalService;
                    averageInspector += element.Inspector1TotalService + element.Inspector2TotalService;
                    averagetotal += element.TotalService;

                    totalServiceEachRun.Add(element.TotalService);
                }
                table.Rows.Add(new object[]
                    {
                        "Average", Math.Round(averageA.TotalMinutes/_data.Count) + " - Minutes",
                        Math.Round(averageB.TotalMinutes/_data.Count) + " - Minutes",
                        Math.Round(averageC.TotalMinutes/_data.Count) + " - Minutes",
                        Math.Round(averageInspector.TotalMinutes/_data.Count) + " - Minutes",
                        TimeSpan.FromMinutes(Math.Round(averagetotal.TotalMinutes/_data.Count))
                    });

                data.DataTables.Add(table);

                // Table 2


                table = new DataTable("TableTwo");
                table.Columns.Add("NoRunBox", typeof (string));
                table.Columns.Add("ArDate", typeof (string));
                table.Columns.Add("DeDate", typeof (string));
                table.Columns.Add("SSSA", typeof (string));
                table.Columns.Add("SESA", typeof (string));
                table.Columns.Add("SSSB", typeof (string));
                table.Columns.Add("SESB", typeof (string));
                table.Columns.Add("SSSC", typeof (string));
                table.Columns.Add("SESC", typeof (string));
                table.Columns.Add("TotalServiceTime", typeof (string));

                counter = 1;

                foreach (SimulationResult element in _data)
                {
                    int boxCounter = 1;
                    foreach (Box box in element.BoxResult)
                    {
                        table.Rows.Add(new object[]
                            {
                                counter + "." + boxCounter,
                                box.ArrivalTime.TimeOfDay.ToString(),
                                box.DepartureTime.TimeOfDay.ToString(),
                                box.StationAServiceStartTime.TimeOfDay.ToString(),
                                box.StationAServiceEndTime.TimeOfDay.ToString(),
                                box.StationBServiceStartTime.TimeOfDay.ToString(),
                                box.StationBServiceEndTime.TimeOfDay.ToString(),
                                box.StationCServiceStartTime.TimeOfDay.ToString(),
                                box.StationCServiceEndTime.TimeOfDay.ToString(),
                                box.GetTotalServiceTime(true).ToString()
                            });

                        boxCounter++;
                    }

                    if (counter != 10)
                    {
                        table.Rows.Add(new object[]
                            {
                                "--", "--", "--", "--", "--", "--", "--", "--", "--", "--"
                            });
                        table.Rows.Add(new object[]
                            {
                                "", ">>", "", "R", "U", "N", "", (counter + 1).ToString()
                            });
                        table.Rows.Add(new object[]
                            {
                                "--", "--", "--", "--", "--", "--", "--", "--", "--", "--"
                            });
                    }

                    counter++;
                }

                data.DataTables.Add(table);


                // CHART One
                var chartTable = new DataTable("ChartOne");
                chartTable.Columns.Add("Run", typeof (string));
                chartTable.Columns.Add("TotalBoxes", typeof (int));

                counter = 1;
                foreach (SimulationResult element in _data)
                {
                    chartTable.Rows.Add(new object[] {counter, element.BoxResult.Count});
                    counter++;
                }

                data.DataTables.Add(chartTable);


                // CHART Two
                chartTable = new DataTable("ChartTwo");
                chartTable.Columns.Add("Run", typeof (string));
                chartTable.Columns.Add("AverageServicetime", typeof (double));

                counter = 1;
                foreach (TimeSpan element in totalServiceEachRun)
                {
                    chartTable.Rows.Add(new object[] {counter, element.TotalMinutes});
                    counter++;
                }

                data.DataTables.Add(chartTable);


                DateTime dateTimeStart = DateTime.Now; // start time measure here

                XpsDocument xps = reportDocument.CreateXpsDocument(data);
                DocumentViewer.Document = xps.GetFixedDocumentSequence();

                // show the elapsed time in window title
                Title += string.Format(" - Generated in {0}ms", (DateTime.Now - dateTimeStart).TotalMilliseconds);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);
            }
        }
    }
}