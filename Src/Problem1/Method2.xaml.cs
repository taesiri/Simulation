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
    /// Interaction logic for Method2.xaml
    /// </summary>
    public partial class Method2
    {

        private bool _firstActivated = true;
        private readonly List<ReportTableRowList> _eventList; 

        public Method2(ReportTableRowList data)
        {
            InitializeComponent();
            _eventList = new List<ReportTableRowList> { data };
        }

        private void FillDataTable(ref DataTable table, int eventIndex)
        {
            table.Columns.Add("no", typeof (int));
            table.Columns.Add("Bearing1", typeof (int));
            table.Columns.Add("Bearing2", typeof (int));
            table.Columns.Add("Bearing3", typeof (int));
            table.Columns.Add("FirstFail", typeof (int));
            table.Columns.Add("CumulativeLifetime", typeof (int));
            table.Columns.Add("RandomNumber", typeof (int));
            table.Columns.Add("SuspensionTime", typeof (int));

            var tempList = _eventList[eventIndex].ReturnData();
            var counter = 1;
            foreach (var elemet in tempList)
            {
                // Add Element To the Table
                table.Rows.Add(new object[]
                                   {
                                       counter, elemet.Column1, elemet.Column2, elemet.Column3, elemet.Column4, elemet.Column5,
                                       elemet.Column6, elemet.Column7
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

                var reader = new StreamReader(new FileStream(@"Templates\ReportTemplate-Method2.xaml", FileMode.Open, FileAccess.Read));
                reportDocument.XamlData = reader.ReadToEnd();
                reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, @"Templates\");
                reader.Close();

                var data = new ReportData();
                data.ReportDocumentValues.Add("PrintDate", DateTime.Now); // print date is now

                var table = new DataTable("Bearing1Table");
                FillDataTable(ref table,0);
         
                data.DataTables.Add(table);

                DateTime dateTimeStart = DateTime.Now; // start time measure here

                XpsDocument xps = reportDocument.CreateXpsDocument(data);
                documentViewer.Document = xps.GetFixedDocumentSequence();

                // show the elapsed time in window title
                Title += string.Format(" - Generated in {0}ms", (DateTime.Now - dateTimeStart).TotalMilliseconds);
            }
            catch (Exception ex)
            {
                // show exception
                MessageBox.Show(string.Format("{0}\r\n\r\n{1}\r\n{2}", ex.Message, ex.GetType(), ex.StackTrace), ex.GetType().ToString(),
                                MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

    }
}
