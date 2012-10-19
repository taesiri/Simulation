using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Problem1.Dialog
{
    /// <summary>
    /// Interaction logic for NumberDialog.xaml
    /// </summary>
    public partial class NumberDialog
    {
        public string EnterdText
        {
            get { return txtNumber.Text; }
        }

        public NumberDialog()
        {
            InitializeComponent();
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
