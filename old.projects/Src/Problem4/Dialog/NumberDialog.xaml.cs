using System;
using System.Windows;

namespace Problem4.Dialog
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

        public int SelectedInteger
        {
            get
            {
                return Convert.ToInt32(textBox1.Text);
            }
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