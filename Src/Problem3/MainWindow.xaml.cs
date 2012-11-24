using System.Collections.Generic;
using System.Windows;
using Problem3.RandomGenerator;

namespace Problem3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var home = new Home();
            home.ShowDialog();
            List<float> test = CongruentialMethod.MixedCongruential(100, 27, 4, 6);
            test.ToString();
        }
    }
}