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
        private TimeSpan _time;
        public Method1(ReportTableRowList dataList1, ReportTableRowList dataList2, ReportTableRowList dataList3, TimeSpan time)
        {
            InitializeComponent();
            _eventList = new List<ReportTableRowList> {dataList1, dataList2, dataList3};
            _time = time;
        }

        private void FillDataTable(ref DataTable table, int eventIndex, out int tLife,out int tWaste)
        {
            table.Columns.Add("no", typeof (string));
            table.Columns.Add("RandomNumber1", typeof (string));
            table.Columns.Add("LifeTime", typeof(string));
            table.Columns.Add("CumulativeLifetime", typeof(string));
            table.Columns.Add("RandomNumber2", typeof(string));
            table.Columns.Add("SuspensionTime", typeof(string));

            List<ReportTableRowClass> tempList = _eventList[eventIndex].ReturnData();
            int counter = 1;
            int totalLife = 0;
            int totalWaste = 0;
            foreach (ReportTableRowClass element in tempList)
            {
                // Add Element To the Table
                table.Rows.Add(new object[]
                                   {
                                       counter, element.Column1, element.Column2,
                                       element.Column3, element.Column4, element.Column5
                                   });
                counter++;
                totalLife += element.Column2;
                totalWaste += element.Column5;
            }
            table.Rows.Add(new object[] {"Total", "---","---", totalLife, "---", totalWaste});
            tLife = totalLife;
            tWaste = totalWaste;
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

                int bearing1Life = 0;
                int bearing2Life = 0;
                int bearing3Life = 0;
                int bearing1Waste = 0;
                int bearing2Waste = 0;
                int bearing3Waste = 0;

                var table = new DataTable("Bearing1Table");
                FillDataTable(ref table, 0, out bearing1Life, out bearing1Waste);
                var table2 = new DataTable("Bearing2Table");
                FillDataTable(ref table2, 1, out bearing2Life, out bearing2Waste);
                var table3 = new DataTable("Bearing3Table");
                FillDataTable(ref table3, 2, out bearing3Life, out bearing3Waste);

                data.DataTables.Add(table);
                data.DataTables.Add(table2);
                data.DataTables.Add(table3);



                var mtable = new DataTable("BearingsLife3DChart");
                mtable.Columns.Add("Bearing", typeof(string));
                mtable.Columns.Add("Total LifeSpan", typeof(int));
                mtable.Rows.Add(new object[] { "Bearing 1", bearing1Life });
                mtable.Rows.Add(new object[] { "Bearing 2", bearing2Life });
                mtable.Rows.Add(new object[] { "Bearing 3", bearing3Life });
                data.DataTables.Add(mtable);

                mtable = new DataTable("BearingsWastes3DChart");
                mtable.Columns.Add("Bearing", typeof(string));
                mtable.Columns.Add("Waste Time", typeof(int));
                mtable.Rows.Add(new object[] { "Bearing 1", bearing1Waste });
                mtable.Rows.Add(new object[] { "Bearing 2", bearing2Waste });
                mtable.Rows.Add(new object[] { "Bearing 3", bearing3Waste });
                data.DataTables.Add(mtable);


                DateTime dateTimeStart = DateTime.Now; // start time measure here

                XpsDocument xps = reportDocument.CreateXpsDocument(data);
                documentViewer.Document = xps.GetFixedDocumentSequence();

                // show the elapsed time in window title
                Title += string.Format(" - Generated in {0}ms", (_time + (DateTime.Now - dateTimeStart)).TotalMilliseconds);
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