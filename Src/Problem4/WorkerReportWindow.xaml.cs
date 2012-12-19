using System;
using System.Collections.Generic;
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
        private readonly List<BoxItem> _data;
        private bool _firstActivated = true;

        public WorkerReportWindow(IEnumerable<BoxItem> data)
        {
            InitializeComponent();
            _data = new List<BoxItem>(data);
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            if (!_firstActivated) return;

            _firstActivated = false;

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