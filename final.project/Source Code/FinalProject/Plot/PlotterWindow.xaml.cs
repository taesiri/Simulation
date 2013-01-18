using System;
using System.Windows;
using System.Windows.Media;
using FinalProject.SimulationElements.RandomGenerator;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace FinalProject.Plot
{
    /// <summary>
    ///     Interaction logic for PlotterWindow.xaml
    /// </summary>
    public partial class PlotterWindow
    {
        public PlotterWindow(double landa, Color color)
        {
            InitializeComponent();
            Landa = landa;
            PlotterColor = color;
        }

        public double Landa { get; set; }
        public Color PlotterColor { get; set; }

        private void PlotterWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ExponentialDistribution(Landa, PlotterColor);
            CumulativeDistribution(Landa, PlotterColor);
            SampleRandoms(Convert.ToDouble(1/Landa));
        }

        public void SampleRandoms(double mean)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            int n = rand.Next(10, 100);
            for (int i = 0; i < n; i++)
            {
                RandomEngine.GetNormal();
            }

            string text = "Sample Randome Numbers : \n";

            for (int i = 0; i < 50; i++)
            {
                text += RandomEngine.GetExpo(mean) + ",";
            }
            text = text.Remove(text.Length - 1, 1);

            TxtBlockSampleNumber.Text = text;
        }

        public void ExponentialDistribution(double landa, Color color)
        {
            // Prepare data in arrays
            const int n = 200;
            var x = new double[n];
            var y = new double[n];

            for (int i = 0; i < n; i++)
            {
                x[i] = i;
                y[i] = landa*Math.Pow(Math.E, -1*landa*x[i]);
            }

            // Create data sources:
            EnumerableDataSource<double> xDataSource = x.AsXDataSource();
            EnumerableDataSource<double> yDataSource = y.AsYDataSource();

            CompositeDataSource compositeDataSource = xDataSource.Join(yDataSource);
            // adding graph to plotter
            MainPlotter.AddLineGraph(compositeDataSource,
                                     color,
                                     3,
                                     "Exponential Distribution - Landa : " + landa.ToString("0.0000"));

            // Force evertyhing plotted to be visible
            MainPlotter.FitToView();
        }

        public void CumulativeDistribution(double landa, Color color)
        {
            // Prepare data in arrays
            const int n = 200;
            var x = new double[n];
            var y = new double[n];

            for (int i = 0; i < n; i++)
            {
                x[i] = i;
                y[i] = (1 - Math.Pow(Math.E, -1*landa*x[i]));
            }

            // Create data sources:
            EnumerableDataSource<double> xDataSource = x.AsXDataSource();
            EnumerableDataSource<double> yDataSource = y.AsYDataSource();

            CompositeDataSource compositeDataSource = xDataSource.Join(yDataSource);
            // adding graph to plotter
            Color clr = Color.FromRgb((byte) (255 - color.R), (byte) (255 - color.G), (byte) (255 - color.B));

            CdfPlotter.AddLineGraph(compositeDataSource, clr,
                                    3,
                                    "Cumulative Distribution");

            // Force evertyhing plotted to be visible
            CdfPlotter.FitToView();
        }
    }
}