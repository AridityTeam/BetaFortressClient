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
using System.Diagnostics;
using System.Threading.Tasks;

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

            #if WINDOWS
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
            #endif

            try
            {
                Run();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        static void HandleException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
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
                Console.ForegroundColor = ConsoleColor.Gray;
                Gui.MessageEnd("Press any key to exit", 1);
            }
        }

        static void Run()
        {
            try
            {
                Gui.Message("WARNING: You may encounter issues on this version.\n", 0);
                Gui.MessageInput("Press the ENTER key to continue");

                RunInteractive();
            }
            finally
            {
                Console.WriteLine("[ BFCLIENT ] Leaving function: Run(string[] args) ");
            }
        }

        static void RunInteractive()
        {
            Console.Clear();

            Gui.Message("Welcome to Beta Fortress Client aka a knockoff of TF2CDownloader in .NET!\n", 0);
            Gui.Message("You are in version " + Assembly.GetExecutingAssembly().GetName().Version + "\n", 0);
            if (File.Exists(ModManager.GetModPath + "/gameinfo.txt"))
            {
                string gameInfoContent = File.ReadAllText(ModManager.GetModPath + "/gameinfo.txt");
                var gameInfo = ModManager.ExtractGameInfo(gameInfoContent);

                //if (gameInfo.steamAppId != 243750)
                //{
                //    Console.WriteLine("============================================================================\n");
                //    Gui.Message("Log\n", 0);
                //    Gui.Message("Warning: Beta Fortress's Steam App ID value in gameinfo.txt differ from 243750\nCurrent App ID is: " + gameInfo.steamAppId + "\n\n", 0);
                //    Console.WriteLine("============================================================================\n");
                //}

                //if (gameInfo.NoModels != 1)
                //{
                //    Console.WriteLine("============================================================================\n");
                //    Gui.Message("Log\n", 0);
                //    Gui.Message("Warning: Beta Fortress's Steam App ID value in gameinfo.txt differ from 243750\nCurrent App ID is: " + gameInfo.NoModels + "\n\n", 0);
                //    Console.WriteLine("============================================================================\n");
                //}
            }
            Gui.Message("[ 1 ] Install / Update Beta Fortress\n" +
                        "[ 2 ] Configure for server hosting\n" +
                        "[ 3 ] Uninstall Beta Fortress\n" +
                        "[ 4 ] Run Beta Fortress (windows only)\n" +
                        "[ 5 ] Run Beta Fortress as a dedicated server (windows only)\n" +
                        "[ 6 ] Quit\n", 0);
            string input = Gui.MessageInput("Choose your option:");
            if (input != null)
            {
                if (input == "1")
                {
                    #if WINDOWS
                    if (ModManager.IsModInstalled)
                    {
                        Console.Clear();
                        // update
                        Task.Run(async() => await ModManager.UpdateMod(Steam.GetSourceModsPath + "/bf"));

                        if (Gui.MessageYesNo("Do you want to go back to the menu?"))
                        {
                            RunInteractive();
                        }
                        else
                        {
                            Gui.MessageEnd("User selected to exit the utility\n", 0);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Task.Run(async () => await ModManager.InstallMod(Steam.GetSourceModsPath + "/bf"));

                        if (Gui.MessageYesNo("Do you want to go back to the menu?"))
                        {
                            RunInteractive();
                        }
                        else
                        {
                            Gui.MessageEnd("User selected to exit the utility\n", 0);
                        }
                    }
                    #else
                    ModManager.ModPath = Gui.MessageDir("Please enter the directory where Steam is installed:");
                    Gui.Message("Steam is installed at: " + ModManager.ModPath + ". Is this correct?\n", 0);
                    #endif
                }
                else if (input == "2")
                {
                    //Gui.MessageInput("Coming soon!");
                    //Console.Clear();
                    //RunInteractive();
                    Console.Clear();
                    if (!File.Exists(ModManager.GetModPath + "/cfg/server.cfg"))
                    {
                        Gui.Message("Creating the server.cfg file...\n", 0);
                        Task.Run(async () => await File.WriteAllTextAsync(ModManager.GetModPath + "/cfg/server.cfg", ""));
                        if (Gui.MessageYesNo("Created the server configuration file, do you want to edit it now?"))
                        {
                            Process p = new Process();
#if WINDOWS
                            p.StartInfo.FileName = "C:\\windows\\notepad.exe";
#elif POSIX
                            p.StartInfo.FileName = "/bin/nano";
#endif
                            p.StartInfo.Arguments = ModManager.GetModPath + "/cfg/server.cfg";
                            p.Start();
                            Gui.Message("Waiting for the default program for CFG files to exit...\n", 0);
                            p.WaitForExit();

                            if (Gui.MessageYesNo("Do you want to go back to the menu?"))
                            {
                                RunInteractive();
                            }
                            else
                            {
                                Gui.MessageEnd("User selected to exit the utility\n", 0);
                            }
                        }
                    }
                    else
                    {
                        if (Gui.MessageYesNo("The server configuration file exists in your current installation, do you want to edit it now?"))
                        {
                            Process p = new Process();
#if WINDOWS
                            p.StartInfo.FileName = "C:\\windows\\notepad.exe";
#elif POSIX
                            p.StartInfo.FileName = "/bin/nano";
#endif
                            p.StartInfo.Arguments = ModManager.GetModPath + "/cfg/server.cfg";
                            p.Start();
                            Gui.Message("Waiting for the default program for CFG files to exit...\n", 0);
                            p.WaitForExit();

                            if (Gui.MessageYesNo("Do you want to go back to the menu?"))
                            {
                                RunInteractive();
                            }
                            else
                            {
                                Gui.MessageEnd("User selected to exit the utility\n", 0);
                            }
                        }
                    }
                }
                else if (input == "3")
                {
                    ModManager.ModPath = Gui.MessageDir("Please enter the directory where Steam is installed:");
                    Gui.Message("Steam is installed at: " + ModManager.ModPath + ". Is this correct?\n", 0);

                    if (Gui.MessageYesNo("This will remove your current configurations and current custom mods/addons for Beta Fortress. Are you sure?"))
                    {
                        Directory.Delete(ModManager.ModPath, true);
                    }
                }
                #if WINDOWS
                else if (input == "4")
                {
                    if (Gui.MessageYesNo("Do you want to add extra launch options?"))
                    {
                        string bfArgs = Gui.MessageInput("Launch options:");
                        Steam.RunApp(243750, "-game " + ModManager.GetModPath + " " + bfArgs);
                    }
                    else
                    {
                        Steam.RunApp(243750, "-game " + ModManager.GetModPath);
                    }
                    RunInteractive();
                }
                else if (input == "5")
                {
                    if (!File.Exists(ModManager.GetModPath + "/cfg/server.cfg"))
                    {
                        Gui.Message("The server configuration file doesn't exist! Using the recommended configuration by The Aridity Team", 0);
                        Steam.RunApp(244310, "-game " + ModManager.GetModPath + " " + "-console " + "+hostname My Beta Fortress Server " + "+tv_enable 1 " + "+map ctf_2base");
                    }
                    else
                    {
                        Steam.RunApp(244310, "-game " + ModManager.GetModPath + " " + "-console " + "+servercfgfile server.cfg");
                    }
                    RunInteractive();
                }
                #else
                else if (input == "4")
                {
                    Gui.MessageWaitForKey("This feature is available on Windows only!\nPress any key to go back to the menu\n");
                    RunInteractive();
                }
                else if (input == "5")
                {
                    Gui.MessageWaitForKey("This feature is available on Windows only!\nPress any key to go back to the menu\n");
                    RunInteractive();
                }
                #endif
                else if (input == "6")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Gui.Message("Please enter 1, 2, 3, 4 or 5.\n", 1);
                    if(Gui.MessageYesNo("Do you want to go back to the menu?"))
                    {
                        RunInteractive();
                    }
                }
            }
        }
    }
}
