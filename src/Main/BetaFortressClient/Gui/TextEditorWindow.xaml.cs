/* 
    Copyright (C) 2024 The Beta Fortress Team, All rights reserved

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

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

        private void lblFileName_Initialized(object sender, EventArgs e)
        {
            this.lblFileName.Content = this.editor.Document.FileName;
        }
    }
}
