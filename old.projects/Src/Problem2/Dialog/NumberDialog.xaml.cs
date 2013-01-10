using System;
using System.Windows;

namespace Problem2.Dialog
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
            get { return Convert.ToInt32(textBox1.Text); }
        }

        public bool IsTimelineSelected
        {
            get { return chkTimeline.IsChecked != null && chkTimeline.IsChecked.Value; }
        }

        public bool IsPrintableOutputSelected
        {
            get { return chkNormalO.IsChecked != null && chkNormalO.IsChecked.Value; }
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            if (chkNormalO.IsChecked != null &&
                (chkTimeline.IsChecked != null && !(chkTimeline.IsChecked.Value || chkNormalO.IsChecked.Value)))
            {
                MessageBox.Show("Please Select Output Format", "Message!");
                return;
            }
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