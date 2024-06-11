/* 
    Copyright (C) 2024 The Aridity Team, All rights reserved

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

//#define HAS_SETUP // startup setup process is still buggy
//#define RELEASE_BUILD
#define BETA_BUILD
//#define CONFIDENTIAL_BUILD
//#define SQUIRREL_UPDATER

using System;
using System.Linq;
using System.IO;
using System.Reflection;
#if WINDOWS
using System.Security.Principal;
#endif

// BF Client-specific namespaces
using BetaFortressTeam.BetaFortressClient.Util;

namespace BetaFortressTeam.BetaFortressClient.Startup
{
    static class BFClientMain
    {

        static string[] commandLineArgs = null;

        static string SteamSetupPath = null;
        static string SteamSetupExe = null;

        public static string[] CommandLineArgs
        {
            get { return commandLineArgs; }
        }

#if WINDOWS
        private static bool IsElevated()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
#endif

        [STAThread()]
        public static void Main(string[] args)
        {
            Console.WriteLine("[ BFCLIENT ] Starting up...");
            commandLineArgs = args;

#if WINDOWS
            Console.WriteLine("[ BFCLIENT ] Checking if Steam is installed...");
            if (!Steam.IsSteamInstalled)
            {
                SteamSetupPath = Environment.ProcessPath + "/temp/" + Path.GetRandomFileName();
                SteamSetupExe = SteamSetupPath + "/SteamSetup.exe";

                if (!Directory.Exists(SteamSetupPath))
                {
                    Directory.CreateDirectory(SteamSetupPath);
                }
            }
#endif

            if (/* better workaround */ Steam.IsSteamInstalled /*Directory.Exists(Steam.GetSteamPath)*/)
            {
                Console.WriteLine("[ BFCLIENT ] Steam is already installed");
            }
            else
            {
                Console.WriteLine("[ BFCLIENT ] Could not find Steam! Assuming that Steam is not installed.");
                Console.WriteLine("[ BFCLIENT ] Quitting.");
                return;
            }

            try
            {
                Run();
                Console.WriteLine("[ BFCLIENT ] Could not find Steam! Assuming Steam is not installed.");
                return;
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        static void HandleException(Exception ex)
        {
            Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Exception has occured!!");
            Console.WriteLine(ex);
            Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Writing log...");

            using (StreamWriter writer = new StreamWriter("./bfclient.BetaFortressTeam" + ".exception.log"))
            {
                OperatingSystem os = Environment.OSVersion;
                Version ver = os.Version;

                if (commandLineArgs.Contains("/console"))
                {
                    writer.WriteLine("EXCEPTIONS OCCURED AS OF " + DateTime.Now);
                    writer.WriteLine("NOTE!!!!!: INSTANCE IS IN CONSOLE MODE AS OF " + DateTime.Now);
                    writer.WriteLine("=================================== OS VERSION DETAILS ===================================");
                    writer.WriteLine("Version: " + os.Version.ToString());
                    writer.WriteLine("  Major version: " + ver.Major);
                    writer.WriteLine("  Major revision: " + ver.MajorRevision);
                    writer.WriteLine("  Minor version: " + ver.Minor);
                    writer.WriteLine("  Minor revision: " + ver.MinorRevision);
                    writer.WriteLine("  Build: " + ver.Build);
                    writer.WriteLine("Platform: " + os.Platform.ToString());
                    writer.WriteLine("SP: " + os.ServicePack.ToString());
                    writer.WriteLine("Version String: " + os.VersionString.ToString());
                    writer.WriteLine("==========================================================================================");
                    writer.WriteLine("==================================== EXCEPTION DETAILS ====================================");
                    writer.WriteLine(ex);
                    writer.WriteLine("===========================================================================================");
                    writer.Close();
                }
                else
                {
                    writer.WriteLine("EXCEPTIONS OCCURED AS OF " + DateTime.Now);
                    writer.WriteLine("=================================== OS VERSION DETAILS ===================================");
                    writer.WriteLine("Version: " + os.Version.ToString());
                    writer.WriteLine("  Major version: " + ver.Major);
                    writer.WriteLine("  Major revision: " + ver.MajorRevision);
                    writer.WriteLine("  Minor version: " + ver.Minor);
                    writer.WriteLine("  Minor revision: " + ver.MinorRevision);
                    writer.WriteLine("  Build: " + ver.Build);
                    writer.WriteLine("Platform: " + os.Platform.ToString());
                    writer.WriteLine("SP: " + os.ServicePack.ToString());
                    writer.WriteLine("Version String: " + os.VersionString.ToString());
                    writer.WriteLine("==========================================================================================");
                    writer.WriteLine("==================================== EXCEPTION DETAILS ====================================");
                    writer.WriteLine(ex);
                    writer.WriteLine("===========================================================================================");
                    writer.Close();
                }

                Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Successfully writted the log file.");
                Console.WriteLine("[ BFCLIENT ] Closing...");
                return;
            }
        }

        static void Run()
        {
            try
            {
                RunInteractive();
            }
            finally
            {
                Console.WriteLine("[ BFCLIENT ] Leaving function: Run(string[] args) ");
            }
        }

        static void RunInteractive()
        {
            Gui.Message("Welcome to Beta Fortress Client aka a knockoff of TF2CDownloader in .NET!", 0);
            Gui.Message("You are in version" + Assembly.GetExecutingAssembly().GetName().Version, 0);
            Gui.Message("[ 1 ] Install / Update Beta Fortress\n" +
                        "[ 2 ] Configure for server hosting\n" +
                        "[ 3 ] Uninstall Beta Fortress\n" +
                        "[ 4 ] Quit\n", 0);
            string input = Gui.MessageInput("Choose your option: ");
            if(input != null) 
            {
                if(input == "1")
                {
                    if(ModManager.IsModInstalled)
                    {
                        // update
                    }
                    else
                    {
                        ModManager.InstallMod(Steam.GetSourceModsPath + "/bf");
                    }
                }
                else if(input == "2")
                {
                    Gui.Message("Coming soon!", 0);
                    Console.Clear();
                    RunInteractive();
                }
                else if(input == "3")
                {
                    if (Gui.MessageYesNo("This will remove your current configurations and current custom mods/addons for Beta Fortress. Are you sure?"))
                    {
                        Directory.Delete(Steam.GetSourceModsPath + "/bf", true);
                    }
                }
                else if(input == "4")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Gui.MessageEnd("Please enter 1, 2, 3 or 4.", 1);
                }
            }
        }
    }
}
