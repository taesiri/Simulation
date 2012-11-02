using System;
using System.Collections.Generic;
using System.Windows;
using Problem2.Dialog;
using Problem2.GraphicalOutput;
using Problem2.ScriptEditor;
using Problem2.Solvers;
using Problem2.Solvers.Entities;

namespace Problem2
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
                int numb = numberDialog.SelectedInteger;

                try
                {
                    MessageBox.Show("Please wait while we processing your request!", "[wait]");
                    var engine = new SolverEngine(SolverEngine.ReaderEngine.BuiltIn, SolverEngine.SolvingMethod.Method1,
                                                  numb);
                    engine.SolveIt();
                    List<ICustomer> solvedData = engine.GetAnswer;
                    List<Carhops> carhopses = engine.GetCarhops;
                    //var globalList = engine.GetGlobalList;

                    if (solvedData != null && carhopses != null)
                    {
                        if (numberDialog.IsTimelineSelected)
                        {
                            var timeline = new Timeline(solvedData);
                            timeline.Show();
                        }
                        if (numberDialog.IsPrintableOutputSelected)
                        {
                            var output = new Method1Report(solvedData, carhopses);
                            output.Show();
                        }
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Error! \n" + exp.Message + "\n" + exp.StackTrace, "Error !", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }

        private void BtnSolve2Click(object sender, RoutedEventArgs e)
        {
            var numberDialog = new NumberDialog();
            numberDialog.ShowDialog();

            if (numberDialog.DialogResult.HasValue && numberDialog.DialogResult.Value)
            {
                int numb = numberDialog.SelectedInteger;


                var engine = new SolverEngine(SolverEngine.ReaderEngine.BuiltIn, SolverEngine.SolvingMethod.Method2,
                                              numb);
                engine.SolveIt();
                List<ICustomer> solvedData = engine.GetAnswer;
                List<Carhops> carhopses = engine.GetCarhops;
                if (solvedData != null && carhopses != null)
                {
                    if (numberDialog.IsTimelineSelected)
                    {
                        var timeline = new Timeline(solvedData);
                        timeline.Show();
                    }
                    if (numberDialog.IsPrintableOutputSelected)
                    {
                        var output = new Method2Report(solvedData, carhopses);
                        output.Show();
                    }
                }
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

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            rBtnBuiltInCSharp.IsChecked = true;
        }
    }
}