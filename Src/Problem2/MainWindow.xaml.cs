using System.Windows;
using Problem2.ScriptEditor;
using Problem2.Solvers;

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
            var engine = new SolverEngine(SolverEngine.ReaderEngine.BuiltIn, SolverEngine.SolvingMethod.Method1, 10);
            engine.SolveIt();

        }

        private void BtnSolve2Click(object sender, RoutedEventArgs e)
        {
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