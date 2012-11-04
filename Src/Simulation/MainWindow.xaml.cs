using System.Diagnostics;
using System.Windows;

namespace Simulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void BtnProblem1Click(object sender, RoutedEventArgs e)
        {

            Process.Start(@"Problem1\");
        }

        private void BtnProblem2Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"Problem2\");
        }

      

      
    }
}
