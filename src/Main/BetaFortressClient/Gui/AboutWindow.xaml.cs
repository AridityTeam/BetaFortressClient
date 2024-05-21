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
            this.lblClientName.Content += " arch any cpu x86 version" + Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
