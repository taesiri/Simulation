using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Problem4.Highway;

namespace Problem4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Cs1Details = "Case Study 1";
        private const string Cs2Details = "Case Study 2";
        private const string Cs3Details = "Case Study 3";

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

        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void BtnCs2Click(object sender, RoutedEventArgs e)
        {
            var sl = new Solver(2000);
            sl.SolveIt();

            var highway = new HighwayReportWindow(sl.SolvedData);
            highway.Show();
        }
    }
}