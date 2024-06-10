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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
#if HAS_SETUP
using Microsoft.Win32;
#endif
using System.Reflection;
//using System.Net.Http;
//using System.Threading.Tasks;

// BF Client-specific namespaces
using BetaFortressTeam.BetaFortressClient.Util;

namespace BetaFortressTeam.BetaFortressClient.Startup
{
    static class BFClientMain
    {

        static string[] commandLineArgs = null;

        static string SteamSetupPath = null;
        static string SteamSetupExe = null;

        static App app;

        public static string[] CommandLineArgs
        {
            get { return commandLineArgs; }
        }

        [STAThread()]
        public static void Main(string[] args)
        {
            Console.WriteLine("[ BFCLIENT ] Starting up...");
            commandLineArgs = args;

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

            if(/* better workaround */ Steam.IsSteamInstalled /*Directory.Exists(Steam.GetSteamPath)*/)
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
            }
            catch (Exception e)
            {
                HandleExceptionWithMessage(e, true);
            }
        }

        private static void SteamSetupDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(File.Exists(SteamSetupExe))
            {
                Process.Start(SteamSetupExe);
            }
        }

        static void Run()
        {
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
            if(choice == "1")
            {

            }
            else 
            {
                Console.WriteLine("Please enter 1, 2, 3 or 4.");
                Console.WriteLine("Press any key to continue . . .");
                Console.ReadKey();
                
                InitInteractive();
            }
        }
    }
}
