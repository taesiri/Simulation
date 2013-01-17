using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using FinalProject.Plot;
using FinalProject.SimulationWorld;
using FinalProject.Simulator;
using FinalProject.Simulator.Report;

namespace FinalProject
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnAutoSClick(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Today + TimeSpan.FromHours(8);

            var results = new List<SimulationResult>();
            for (int i = 0; i < 10; i++)
            {
                var simulator = new AutoSimulator(start, start.Add(TimeSpan.FromHours(8)),
                                                  start.Add(TimeSpan.FromHours(1)));

                simulator.StartSimulation();
                results.Add(simulator.GetResult());
            }


            var reportWindow = new ReportWindow(results);
            reportWindow.Show();
        }

        private void BtnRealTimeSimulation(object sender, RoutedEventArgs e)
        {
            var vw = new World();

            vw.Show();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnGraphClick(object sender, RoutedEventArgs e)
        {
            var plotterDialog = new PlotterDialog();
            plotterDialog.ShowDialog();
        }

        private void BtnSimulationChartsClick(object sender, RoutedEventArgs e)
        {
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Process.Start(appStartPath + @"\Charts\FinalProject.htm");
        }
    }
}