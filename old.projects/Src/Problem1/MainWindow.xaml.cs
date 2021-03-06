﻿using System;
using System.Windows;
using Problem1.Dialog;
using Problem1.ScriptEditor;
using Problem1.Solvers;
using Problem1.TableRows;

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
                int numb = numberDialog.SelectedInteger;

                try
                {
                    MessageBox.Show("Please wait while we processing your request!", "[wait]");
                    DateTime timer = DateTime.Now;
                    SolverEngine solverEngine;
                    if (rBtnBuiltInCSharp.IsChecked == true)
                    {
                        solverEngine = new SolverEngine(SolverEngine.ReaderEngine.CSharp,
                                                        SolverEngine.SolvingMethod.Method1, numb);
                    }
                    else if (rBtnIronPython.IsChecked == true)
                    {
                        solverEngine = new SolverEngine(SolverEngine.ReaderEngine.Python,
                                                        SolverEngine.SolvingMethod.Method1, numb);
                    }
                    else
                    {
                        //Default Set to CSharp Built-In
                        solverEngine = new SolverEngine(SolverEngine.ReaderEngine.CSharp,
                                                        SolverEngine.SolvingMethod.Method1, numb);
                    }

                    ReportTableRowList bearing1 = solverEngine.SolveIt();
                    ReportTableRowList bearing2 = solverEngine.SolveIt();
                    ReportTableRowList bearing3 = solverEngine.SolveIt();

                    var totalTime = DateTime.Now.Subtract(timer);

                    var method = new Method1(bearing1, bearing2, bearing3, totalTime);
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
                int numb = numberDialog.SelectedInteger;
                try
                {
                    MessageBox.Show("Please wait while we processing your request!", "[wait]");
                    DateTime timer = DateTime.Now;

                    SolverEngine solverEngine;
                    if (rBtnBuiltInCSharp.IsChecked == true)
                    {
                        solverEngine = new SolverEngine(SolverEngine.ReaderEngine.CSharp,
                                                        SolverEngine.SolvingMethod.Method2, numb);
                    }
                    else if (rBtnIronPython.IsChecked == true)
                    {
                        solverEngine = new SolverEngine(SolverEngine.ReaderEngine.Python,
                                                        SolverEngine.SolvingMethod.Method2, numb);
                    }
                    else
                    {
                        //Default Set to CSharp Built-In
                        solverEngine = new SolverEngine(SolverEngine.ReaderEngine.CSharp,
                                                        SolverEngine.SolvingMethod.Method2, numb);
                    }

                    ReportTableRowList data = solverEngine.SolveIt();

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