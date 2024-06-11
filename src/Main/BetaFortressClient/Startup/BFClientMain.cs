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
<<<<<<< Updated upstream
#if HAS_SETUP
using Microsoft.Win32;
#endif
//using System.Net.Http;
//using System.Threading.Tasks;
=======
using System.Reflection;
#if WINDOWS
using System.Security.Principal;
#endif
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
=======
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

>>>>>>> Stashed changes
        [STAThread()]
        public static void Main(string[] args)
        {
            Console.WriteLine("[ BFCLIENT ] Starting up...");
            commandLineArgs = args;

<<<<<<< Updated upstream
=======
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

>>>>>>> Stashed changes
            if (/* better workaround */ Steam.IsSteamInstalled /*Directory.Exists(Steam.GetSteamPath)*/)
            {
                Console.WriteLine("[ BFCLIENT ] Steam is already installed");
            }
            else
            {
<<<<<<< Updated upstream
                Console.WriteLine("[ BFCLIENT ] Could not find Steam! Assuming that Steam is not installed.");
                Console.WriteLine("[ BFCLIENT ] Quitting.");
                return;
            }

            try
            {
                Run();
=======
                Console.WriteLine("[ BFCLIENT ] Could not find Steam! Assuming Steam is not installed.");
                return;
            }

#if HAS_SETUP
            // wacky workaround
            if(!File.Exists("./configuration/BFClientConfig.cfg") || Properties.BFClientAppSettings.Default.FirstRun == true)
            {
                SetupManager.HasCompletedSetup = false;
            }
            else
            {
                SetupManager.HasCompletedSetup = false;
            }
#endif

            try
            {
                if (!commandLineArgs.Contains("/console"))
                {
                    Run();
                }
                else
                {
                    if (commandLineArgs.Contains("/help"))
                    {
                        Console.WriteLine("Beta Fortress Client v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n" +
                                          "Usage: BetaFortressClient.exe /console [args]\n" +
                                          "/help - show this help text\n" +
                                          "/doNotAllocConsole - do not alloc the console" +
                                          "/initializeGui - init the gui even with the console" +
                                          "/uninstall - uninstalls the mod");
                        Console.ReadKey();
                    }
                }
>>>>>>> Stashed changes
            }
            catch (Exception e)
            {
                HandleExceptionWithMessage(e, true);
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
<<<<<<< Updated upstream

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

=======

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

>>>>>>> Stashed changes
        static void HandleExceptionWithMessage(Exception ex, bool showMessage)
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
            }
        }

        static void Run()
        {
<<<<<<< Updated upstream
            try
            {
                InitInteractive();
            }
            finally
            {
                Console.WriteLine("[ BFCLIENT ] Leaving function: Run(string[] args) ");
            }
        }

        static void InitInteractive()
        {
            Console.WriteLine("******* WELCOME TO BETA FORTRESS CLIENT ******");
            Console.WriteLine("************* SELECT YOUR OPTION *************");
            Console.WriteLine("[ 1 ] Install / Update\n" +
                              "[ 2 ] Configure for server hosting\n" +
                              "[ 3 ] Uninstall\n" +
                              "[ 4 ] Exit");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                if(ModManager.IsModInstalled)
                {
                    Console.WriteLine("Mod is already installed, updating...");
                }
                else
                {
                    Console.WriteLine("Installing Beta Fortress...");
                    
                }
            }
            else
            {
                Console.WriteLine("Please enter 1, 2, 3 or 4.");
                Console.WriteLine("Press any key to continue . . .");
                Console.ReadKey();

                InitInteractive();
=======

            try
            {

                if (!commandLineArgs.Contains("/disableHolidayManager"))
                {
                    // do an action
                    HolidayManager.DoHolidayAction();
                }

                RunInteractive();
            }
            finally
            {
                Console.WriteLine("[ BFCLIENT ] Leaving function: Run()");
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
>>>>>>> Stashed changes
            }
        }
    }
}
