using System;
using System.Windows;
using FinalProject.SimulationWorld;
using FinalProject.Simulator;

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
            var simulator = new AutoSimulator(start, start.Add(TimeSpan.FromHours(8)));

            simulator.StartSimulation();
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
    }
}