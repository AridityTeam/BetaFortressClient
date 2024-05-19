using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BetaFortressTeam.BetaFortressClient.Updater.Util;

namespace BetaFortressTeam.BetaFortressClient.Updater.Startup
{
    static class BFClientUpdaterMain
    {
        static string[] commandLineArgs = null;

        public static string[] CommandLineArgs
        {
            get
            {
                return commandLineArgs;
            }
        }

        public static void Main(string[] args)
        {
            commandLineArgs = args;

            try
            {
                RunService(args);
            }
            catch (Exception ex)
            {

            }
        }

        static async void RunService(string[] args)
        {
            if(commandLineArgs != null)
            {
                if(commandLineArgs.Contains("/checkforupdates"))
                {
                    await SquirrelManager.CheckForUpdates();
                }

                if(commandLineArgs.Contains("/update"))
                {
                    await SquirrelManager.Update();
                }
            }
        }
    }
}
