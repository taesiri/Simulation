using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Problem4.BOX;
using Problem4.Dialog;
using Problem4.Highway;

namespace Problem4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Cs1Details = "A Simple and Handy Question about Chapter 9";
        private const string Cs2Details = "A Question Related to \"Direct Transformation for the Normal Distribution\"";
        private const string Cs3Details = "Hardest Question! but not that HARD! :]";

        private const string TextDefaults = "\\\\ Question Preview";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnsMouseEnter(object sender, MouseEventArgs e)
        {
            var btnender = sender as Button;
            if (btnender == null) return;

            switch (btnender.Name)
            {
                case "BtnCS1":
                    txtQuestionPreview.Text = Cs1Details;
                    break;
                case "BtnCS2":
                    txtQuestionPreview.Text = Cs2Details;
                    break;
                case "BtnCS3":
                    txtQuestionPreview.Text = Cs3Details;
                    break;
            }
        }

        private void BtnsMouseLeave(object sender, MouseEventArgs e)
        {
            txtQuestionPreview.Text = TextDefaults;
        }


        private void BtnCs1Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Problem Solved with empty Hand! Please refer to Documentation for the Answer");
        }


        private void BtnCs2Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NumberDialog();


            dialog.ShowDialog();

            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                int numb = dialog.SelectedInteger;

                var sl = new HighwaySolver(numb);
                sl.SolveIt();
                List<CarDetailsRow> solvedData = sl.SolvedData;

                var accidents = (int) (0.012*solvedData.Count);
                // Make sure we have 1 Accident at Least!
                if (accidents == 0) accidents = 1;

                var randomAccidentsList = new List<int>();
                var rnd = new Random(DateTime.Now.Millisecond);

                while (accidents > 0)
                {
                    int randomNumber = rnd.Next(1, solvedData.Count);
                    if (solvedData[randomNumber].CarType != CarType.C40)
                    {
                        randomAccidentsList.Add(randomNumber);
                        accidents--;
                    }
                }

                foreach (int randomAccident in randomAccidentsList)
                {
                    solvedData[randomAccident].TripStatus = "Encountered Road Accident";
                    solvedData[randomAccident].TripDuration = rnd.Next(1, solvedData[randomAccident].TripDuration - 1);
                }

                var highway = new HighwayReportWindow(solvedData, randomAccidentsList);
                highway.Show();
            }
        }


        private void BtnCs3Click(object sender, RoutedEventArgs e)
        {
            var solver = new BoxSolver(TimeSpan.FromHours(40));
            solver.TrySolve();
            List<BoxItem> answer = solver.GetAnswer();
        }

        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}