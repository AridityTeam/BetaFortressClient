using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BetaFortressTeam.BetaFortressClient.Installer.Gui;

#nullable enable
namespace BetaFortressTeam.BetaFortressClient.Installer.Startup
{
    static class BFClientInstallerMain
    {
        static string[]? commandLineArgs;

        public static string[] CommandLineArgs
        {
            get
            {
                return commandLineArgs;
            }
        }

        static App? app;

        [STAThread()]
        public static void Main(string[] args)
        {
            args = commandLineArgs;

            app = new App();

            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.EnableVisualStyles();

            try
            {
                Run();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }
        }

        static void Run()
        {
            try
            {
                MainWindow window = new MainWindow();
                app.Run(window); // why tf the program cant call this shit?
                                 // why tf is the window isnt showing??
                                 // oh im just an idiot
            }
            finally
            {
                Console.WriteLine("Leaving function: Run()");
            }
        }
    }
}
