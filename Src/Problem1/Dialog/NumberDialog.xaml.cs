using System.Windows;

namespace Problem1.Dialog
{
    /// <summary>
    /// Interaction logic for NumberDialog.xaml
    /// </summary>
    public partial class NumberDialog
    {
        public NumberDialog()
        {
            InitializeComponent();
        }

        public string EnterdText
        {
            get { return txtNumber.Text; }
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCancelalick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}