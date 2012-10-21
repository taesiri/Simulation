using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Problem1.Dialog;
using Problem1.ScriptEditor;
using Problem1.Sovlers;

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
                    DateTime timer = DateTime.Now;

                    MessageBox.Show("Please wait while we processing your request!", "[wait]");

                    var solverEngine = new SolverEngine(SolverEngine.ReaderEngine.Python,SolverEngine.SolvingMethod.Method1, numb);
                    var bearing1 = solverEngine.SolveIt();
                    var bearing2 = solverEngine.SolveIt();
                    var bearing3 = solverEngine.SolveIt();

                    MessageBox.Show("Please wait while we Generating your Report", "[done processing] - " + DateTime.Now.Subtract(timer).ToString());
                    
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
        private void BtnSolve2Click(object sender, RoutedEventArgs e)
        {
            var numberDialog = new NumberDialog();
            numberDialog.ShowDialog();

            if (numberDialog.DialogResult.HasValue && numberDialog.DialogResult.Value)
            {
                var numb = Convert.ToInt32(numberDialog.EnterdText);
                try
                {
                    DateTime timer = DateTime.Now;
                    MessageBox.Show("Please wait while we processing your request!", "[wait]");

                    var solverEngine = new SolverEngine(SolverEngine.ReaderEngine.Python,SolverEngine.SolvingMethod.Method2, numb);
                    var data = solverEngine.SolveIt();

                    MessageBox.Show("Please wait while we Generating your Report", "[done processing] - " + DateTime.Now.Subtract(timer).ToString());

                    var method = new Method2(data);
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
