using System;
using System.Collections.Generic;
using System.Windows;
using Problem3.Helper;
using Problem3.Reporter;

namespace Problem3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //var home = new Home();
            //home.ShowDialog();
        }

        private void BtnStartClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int mod = Convert.ToInt32(txtMod.Text);
                int seed = Convert.ToInt32(txtSeed.Text);
                int mul = Convert.ToInt32(txtMultiplier.Text);
                int inc = Convert.ToInt32(txtIncrement.Text);

                ReportList a = CongruentialMethod.MixedCongruential(mod, seed, mul, inc);

                if (a.GetLength < 20)
                {
                    MessageBox.Show(
                        String.Format(
                            "The minimume Lentgh of Generated Sequence must be 20 .\n Error : {0} is less than 20",
                            a.GetLength), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Begin Analysis over Multiplier
                List<int> lst = NumberPicker.NormalPicker(mod, mul, 20);
                var analysisMultiplier = new List<Tuple<int, int>>();
                foreach (int item in lst)
                {
                    ReportList temp = CongruentialMethod.MixedCongruential(mod, seed, item, inc);
                    analysisMultiplier.Add(new Tuple<int, int>(item, temp.InnerData.Count));
                }
                // End Analysis over Multiplier


                // Begin Analysis over Increment
                lst = NumberPicker.NormalPicker(mod, inc, 20);
                var analysisIncerment = new List<Tuple<int, int>>();
                foreach (int item in lst)
                {
                    ReportList temp = CongruentialMethod.MixedCongruential(mod, seed, mul, item);
                    analysisIncerment.Add(new Tuple<int, int>(item, temp.InnerData.Count));
                }
                // End Analysis over Increment


                // Begin Analysis over Increment
                lst = NumberPicker.RandomPick(mod, 10);
                var analysisSeed = new List<Tuple<int, int>>();
                foreach (int item in lst)
                {
                    ReportList temp = CongruentialMethod.MixedCongruential(mod, item, mul, inc);
                    analysisSeed.Add(new Tuple<int, int>(item, temp.InnerData.Count));
                }
                // End Analysis over Increment


                var rep = new Report(a, analysisMultiplier, analysisIncerment, analysisSeed);
                rep.Show();
            }

            catch (Exception)
            {
                MessageBox.Show("Something went Wrong!!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void BtnExitEClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}