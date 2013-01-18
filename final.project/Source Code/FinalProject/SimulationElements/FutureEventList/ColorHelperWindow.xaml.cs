using System.ComponentModel;

namespace FinalProject.SimulationElements.FutureEventList
{
    /// <summary>
    ///     Interaction logic for ColorHelperWindow.xaml
    /// </summary>
    public partial class ColorHelperWindow
    {
        public ColorHelperWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}