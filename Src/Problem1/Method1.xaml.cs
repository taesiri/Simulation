using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Xps.Packaging;
using CodeReason.Reports;
using Problem1.TableRows;

namespace Problem1
{
    /// <summary>
    /// Interaction logic for Method1.xaml
    /// </summary>
    public partial class Method1
    {
        private readonly List<ReportTableRowList> _eventList;
        private bool _firstActivated = true;

        public Method1(ReportTableRowList dataList1, ReportTableRowList dataList2, ReportTableRowList dataList3)
        {
            InitializeComponent();
            _eventList = new List<ReportTableRowList> {dataList1, dataList2, dataList3};
        }

        private void FillDataTable(ref DataTable table, int eventIndex)
        {
            table.Columns.Add("no", typeof (int));
            table.Columns.Add("RandomNumber1", typeof (int));
            table.Columns.Add("LifeTime", typeof (int));
            table.Columns.Add("CumulativeLifetime", typeof (int));
            table.Columns.Add("RandomNumber2", typeof (int));
            table.Columns.Add("SuspensionTime", typeof (int));

            List<ReportTableRowClass> tempList = _eventList[eventIndex].ReturnData();
            int counter = 1;
            foreach (ReportTableRowClass elemet in tempList)
            {
                // Add Element To the Table
                table.Rows.Add(new object[]
                                   {
                                       counter, elemet.Column1, elemet.Column2, elemet.Column3, elemet.Column4,
                                       elemet.Column5
                                   });
                counter++;
            }
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

                var table = new DataTable("Bearing1Table");
                FillDataTable(ref table, 0);
                var table2 = new DataTable("Bearing2Table");
                FillDataTable(ref table2, 1);
                var table3 = new DataTable("Bearing3Table");
                FillDataTable(ref table3, 2);

                data.DataTables.Add(table);
                data.DataTables.Add(table2);
                data.DataTables.Add(table3);

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