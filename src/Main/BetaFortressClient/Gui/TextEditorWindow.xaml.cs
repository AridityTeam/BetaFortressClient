using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace BetaFortressTeam.BetaFortressClient.Gui
{
    /// <summary>
    /// Interaction logic for TextEditorWindow.xaml
    /// </summary>
    public partial class TextEditorWindow : Window
    {
        string path;

        public TextEditorWindow()
        {
            InitializeComponent();
        }

        public TextEditorWindow(string filePath)
        {
            InitializeComponent();

            path = filePath;
            this.editor.Text = File.ReadAllText(filePath);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (path == null) {
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.DefaultExt = ".cfg";
				if (dlg.ShowDialog() ?? false) {
					path = dlg.FileName;
				} else {
					return;
				}
			}
            this.editor.Save(path);
        }
    }
}
