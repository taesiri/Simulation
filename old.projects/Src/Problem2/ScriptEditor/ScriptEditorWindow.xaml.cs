using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Problem2.ScriptEditor
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

                textEditor.Text = _firstScript;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Something went wrong !\n" + exp.Message, "[error!]");
                return;
            }
        }

        private bool IsCodeChanged()
        {
            if (_firstScript != textEditor.Text)
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
                writer.Write(textEditor.Text);
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

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var fileStream = new FileStream(@"Highlighter\Python.xshd", FileMode.Open, FileAccess.Read);
            using (var reader = new XmlTextReader(fileStream))
            {
                textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
            fileStream.Close();
        }

        private void TextEditorTextChanged(object sender, EventArgs e)
        {
            _isSaved = false;
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}