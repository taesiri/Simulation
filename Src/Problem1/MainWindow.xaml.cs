using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Problem1.Dialog;
using Problem1.ScriptEditor;

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

                try
                {
                    MessageBox.Show("Please wait while we processing your request!", "[wait]");

                    //Try to Solving the Case
                    var bearing1 = Solver1.SolveIt(numb);
                    var bearing2 = Solver1.SolveIt(numb);
                    var bearing3 = Solver1.SolveIt(numb);

                    var method = new Method1(bearing1, bearing2, bearing3);
                    method.Show();
                }
                catch (Exception exp)
                {
                    MessageBox.Show("The Following error has occurred :\n" + exp.Message, "[error!]");
                }
            }
            else
            {
                return;
            }
    
        }

        private void BtnEditScriptClick(object sender, RoutedEventArgs e)
        {
            var scriptEditorWindow = new ScriptEditorWindow();
            scriptEditorWindow.ShowDialog();
        }

        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
