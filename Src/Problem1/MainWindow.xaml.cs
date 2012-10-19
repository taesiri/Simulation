using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Problem1.Dialog;

namespace Problem1
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

        private void BtnSolve1Click(object sender, RoutedEventArgs e)
        {
            var numberDialog = new NumberDialog();
            numberDialog.ShowDialog();

            if (numberDialog.DialogResult.HasValue && numberDialog.DialogResult.Value)
            {
                var numb = Convert.ToInt32(numberDialog.EnterdText);

                MessageBox.Show("Please wait while we processing your request!", "[wait]");
                var method = new Method1(Solver1.SolveIt(numb), Solver1.SolveIt(numb), Solver1.SolveIt(numb));
                method.Show();

            }
            else
            {
                return;
            }
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
