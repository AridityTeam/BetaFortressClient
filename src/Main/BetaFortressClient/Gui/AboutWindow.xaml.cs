using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace BetaFortressTeam.BetaFortressClient.Gui
{
    public partial class AboutWindow : Window
    {
        public AboutWindow() 
        {
            InitializeComponent(); 
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.lblClientName.Content += " arch " + Assembly.GetExecutingAssembly().GetName().ProcessorArchitecture + " version " + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
