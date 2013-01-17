using System;
using System.Windows;
using System.Windows.Media;

namespace FinalProject.Plot
{
    /// <summary>
    ///     Interaction logic for PlotterDialog.xaml
    /// </summary>
    public partial class PlotterDialog : Window
    {
        public PlotterDialog()
        {
            InitializeComponent();
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnDrawClick(object sender, RoutedEventArgs e)
        {
            Color selectedColor = ColorPickerPlotterColor.SelectedColor;
            double mean = Convert.ToDouble(TxtMeanValue.Text);


            var plotwindow = new PlotterWindow(1/mean, selectedColor);
            plotwindow.Show();
            Close();
        }
    }
}