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
        private readonly List<ReportTableRowList> _eventList;
        private bool _firstActivated = true;

        public Method2(ReportTableRowList data)
        {
            InitializeComponent();
            _eventList = new List<ReportTableRowList> {data};
        }

        private void FillDataTable(ref DataTable table, int eventIndex,out int tLife, out int tWasted)
        {
            table.Columns.Add("no", typeof(string));
            table.Columns.Add("Bearing1", typeof(string));
            table.Columns.Add("Bearing2", typeof(string));
            table.Columns.Add("Bearing3", typeof(string));
            table.Columns.Add("FirstFail", typeof(string));
            table.Columns.Add("CumulativeLifetime", typeof(int));
            table.Columns.Add("RandomNumber", typeof(string));
            table.Columns.Add("SuspensionTime", typeof(int));

            List<ReportTableRowClass> tempList = _eventList[eventIndex].ReturnData();
            int counter = 1;
            int totalLife = 0;
            int totalWaste = 0;
            foreach (ReportTableRowClass element in tempList)
            {
                // Add Element To the Table
                table.Rows.Add(new object[]
                                   {
                                       counter, element.Column1, element.Column2, element.Column3, element.Column4,
                                       element.Column5,
                                       element.Column6, element.Column7
                                   });
                counter++;
                totalLife += element.Column4;
                totalWaste += element.Column7;
            }
            table.Rows.Add(new object[] { "Total", "---", "---", "---", "---", totalLife, "---", totalWaste });
            tWasted = totalWaste;
            tLife = totalLife;
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            if (!_firstActivated) return;

            _firstActivated = false;

            try
            {
                var reportDocument = new ReportDocument();

                var reader =
                    new StreamReader(new FileStream(@"Templates\ReportTemplate-Method2.xaml", FileMode.Open,
                                                    FileAccess.Read));
                reportDocument.XamlData = reader.ReadToEnd();
                reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, @"Templates\");
                reader.Close();

                var data = new ReportData();
                data.ReportDocumentValues.Add("PrintDate", DateTime.Now); // print date is now

                var table = new DataTable("Bearing1Table");

                int totalLife, totalWasted;
                FillDataTable(ref table, 0, out totalLife, out totalWasted);

                data.DataTables.Add(table);


                var mtable = new DataTable("BearingsLife3DChart");
                mtable.Columns.Add("Bearing", typeof(string));
                mtable.Columns.Add("Total LifeSpan", typeof(int));
                mtable.Rows.Add(new object[] { "Total", totalLife });
                data.DataTables.Add(mtable);

                mtable = new DataTable("BearingsWastes3DChart");
                mtable.Columns.Add("Bearing", typeof(string));
                mtable.Columns.Add("Waste Time", typeof(int));
                mtable.Rows.Add(new object[] { "Total", totalWasted });
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