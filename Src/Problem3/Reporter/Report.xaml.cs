using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Xps.Packaging;
using CodeReason.Reports;
using Problem3.Helper;
using Problem3.Tests;

namespace Problem3.Reporter
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report
    {
        private readonly List<Tuple<int, int>> _analysisOverIncrement;
        private readonly List<Tuple<int, int>> _analysisOverMultiplier;
        private readonly List<Tuple<int, int>> _analysisOverSeed;
        private readonly ReportList _reportList;
        private bool _firstActivated = true;

        public Report(ReportList data, List<Tuple<int, int>> analysisMultiplier, List<Tuple<int, int>> analysisIncrement,
                      List<Tuple<int, int>> analysisSeed)
        {
            InitializeComponent();
            _reportList = data;
            _analysisOverMultiplier = analysisMultiplier;
            _analysisOverIncrement = analysisIncrement;
            _analysisOverSeed = analysisSeed;
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            if (!_firstActivated) return;

            _firstActivated = false;

            try
            {
                var reportDocument = new ReportDocument();

                var reader = new StreamReader(new FileStream(@"Templates\ReportTemplate.xaml", FileMode.Open,
                                                             FileAccess.Read));
                reportDocument.XamlData = reader.ReadToEnd();

                reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, @"Templates\");
                reader.Close();

                var data = new ReportData();

                data.ReportDocumentValues.Add("PrintDate", DateTime.Now);

                data.ReportDocumentValues.Add("ReportSummary", _reportList.GetReportSummery);

                var table = new DataTable("Table1");
                table.Columns.Add("c1", typeof (string));
                table.Columns.Add("c2", typeof (string));
                table.Columns.Add("c3", typeof (string));

                table.Rows.Add(new object[]
                                   {
                                       _reportList.GetInformation,
                                       _reportList.GetLength.ToString(CultureInfo.InvariantCulture),
                                       _reportList.InnerData.ConvertToString()
                                   });

                data.DataTables.Add(table);

                // CHI TEST
                List<Tuple<int, float, int, float>> testTable = UniformityTests.ChiSquare(10, _reportList);

                table = new DataTable("Table2");
                table.Columns.Add("c1", typeof (string));
                table.Columns.Add("c2", typeof (string));
                table.Columns.Add("c3", typeof (string));
                table.Columns.Add("c4", typeof (string));
                float sum = 0f;
                foreach (var tuple in testTable)
                {
                    table.Rows.Add(new object[]
                                       {
                                           tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4
                                       });
                    sum += tuple.Item4;
                }

                table.Rows.Add(new object[] {"Total", "--", "--", sum.ToString(CultureInfo.InvariantCulture)});

                data.DataTables.Add(table);
                string result = UniformityTests.IsUniformWithChiSquare(testTable)
                                    ? "There isn't enough evidence to rejecting the hypothesis!"
                                    : "The hypothesis is rejected!";
                data.ReportDocumentValues.Add("ChiSquareResult", String.Format("Chi-Square Test Result : {0}", result));

                //END CHI TEST

                // KS TEST

                List<Tuple<int, float, float, float, float, float>> kStestTable = UniformityTests.KSTest(_reportList);

                table = new DataTable("Table3");
                table.Columns.Add("c1", typeof (string));
                table.Columns.Add("c2", typeof (string));
                table.Columns.Add("c3", typeof (string));
                table.Columns.Add("c4", typeof (string));
                table.Columns.Add("c5", typeof (string));
                table.Columns.Add("c6", typeof (string));
                float maxDPlus = 0f;
                float maxDNegetive = 0f;
                foreach (var tuple in kStestTable)
                {
                    if (tuple.Item5 > maxDPlus)
                        maxDPlus = tuple.Item5;
                    if (tuple.Item6 > maxDNegetive)
                        maxDNegetive = tuple.Item6;
                    table.Rows.Add(new object[]
                                       {
                                           tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4,
                                           tuple.Item5, tuple.Item6
                                       });
                }

                table.Rows.Add(new object[] { "Max", "--", "--", "--", maxDPlus, maxDNegetive });

                data.DataTables.Add(table);


                result = UniformityTests.IsUniformWithKS(kStestTable)
                                   ? "There isn't enough evidence to rejecting the hypothesis!"
                                   : "The hypothesis is rejected!";
                data.ReportDocumentValues.Add("KSResult",
                                              String.Format("KS Test Result : Max[D+,D-] = {0}\n{1}",
                                                            Math.Max(maxDPlus, maxDNegetive), result));

                //END TEST

                var mtable = new DataTable("Chart1");
                mtable.Columns.Add("Multiplier", typeof (string));
                mtable.Columns.Add("Length of Sequence", typeof (int));
                foreach (var item in _analysisOverMultiplier)
                {
                    mtable.Rows.Add(new object[] {item.Item1, item.Item2});
                }

                data.DataTables.Add(mtable);


                mtable = new DataTable("Chart2");
                mtable.Columns.Add("Increment", typeof (string));
                mtable.Columns.Add("Length of Sequence", typeof (int));
                foreach (var item in _analysisOverIncrement)
                {
                    mtable.Rows.Add(new object[] {item.Item1, item.Item2});
                }

                data.DataTables.Add(mtable);


                mtable = new DataTable("Chart3");
                mtable.Columns.Add("Seed", typeof (string));
                mtable.Columns.Add("Length of Sequence", typeof (int));
                foreach (var item in _analysisOverSeed)
                {
                    mtable.Rows.Add(new object[] {item.Item1, item.Item2});
                }

                data.DataTables.Add(mtable);


                DateTime dateTimeStart = DateTime.Now;

                XpsDocument xps = reportDocument.CreateXpsDocument(data);
                documentViewer.Document = xps.GetFixedDocumentSequence();

                Title += string.Format(" - Generated in {0}ms", (DateTime.Now - dateTimeStart).TotalMilliseconds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}\r\n\r\n{1}\r\n{2}", ex.Message, ex.GetType(), ex.StackTrace),
                                ex.GetType().ToString(),
                                MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
    }
}