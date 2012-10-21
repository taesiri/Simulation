using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using AurelienRibon.Ui.SyntaxHighlightBox;

namespace Problem1.ScriptEditor
{
    /// <summary>
    /// Interaction logic for ScriptEditorWindow.xaml
    /// </summary>
    public partial class ScriptEditorWindow
    {
        private readonly string _firstScript;
        private bool _isSaved;

        public ScriptEditorWindow()
        {
            InitializeComponent();
            shbox.CurrentHighlighter = HighlighterManager.Instance.Highlighters["VHDL"];
            try
            {
                if (!File.Exists(@"Scripts\Script.py"))
                {
                    MessageBox.Show("Could not find \"Script.py\"!", "[warning!]", MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    File.Create(@"Scripts\Script.py");
                }

                var reader = new StreamReader(new FileStream(@"Scripts\Script.py", FileMode.Open, FileAccess.Read));
                _firstScript = reader.ReadToEnd();
                reader.Close();

                shbox.Text = _firstScript;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Something went wrong !\n" + exp.Message, "[error!]");
                return;
            }
        }

        private bool IsCodeChanged()
        {
            if (_firstScript != shbox.Text)
                if (!_isSaved)
                    return true;

            return false;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (IsCodeChanged())
            {
                MessageBoxResult dialogResult = MessageBox.Show("Do you want to save changes you made to Script?",
                                                                "Script Editor", MessageBoxButton.YesNoCancel);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    DoSave();
                }
                else if (dialogResult == MessageBoxResult.No)
                {
                    return;
                }
                else // Cancel Button Clicked!
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void DoSave()
        {
            try
            {
                var writer = new StreamWriter(new FileStream(@"Scripts\Script.py", FileMode.Truncate, FileAccess.Write));
                writer.Write(shbox.Text);
                writer.Close();

                _isSaved = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Could not save to \"Script.py\"!\n" + exp.Message, "[warning!]", MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private bool DoTest()
        {
            //throw new NotImplementedException();
            return true;
        }

        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            DoSave();
            Close();
        }

        private void ShboxTextChanged(object sender, TextChangedEventArgs e)
        {
            _isSaved = false;
        }
    }
}