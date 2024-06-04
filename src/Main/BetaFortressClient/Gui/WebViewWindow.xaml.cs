using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BetaFortressTeam.BetaFortressClient.Gui
{
    /// <summary>
    /// Interaction logic for WebViewWindow.xaml
    /// </summary>
    public partial class WebViewWindow : Window
    {
        public WebViewWindow()
        {
            InitializeComponent();
        }

        public WebViewWindow(string url)
        {
            InitializeComponent();

            this.webview.Source = new Uri(url);
        }
    }
}
