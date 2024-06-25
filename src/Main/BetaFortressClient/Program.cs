using BetaFortressTeam.BetaFortressClient.Util;
using System.Diagnostics;
using Sentry;
using Sentry.Profiling;

namespace BetaFortressTeam.BetaFortressClient
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("[BFCLIENT] Starting up the program...");
            // sentry must be the intialized first!!!
            // Init the Sentry SDK
            SentrySdk.Init(o =>
            {
                // Tells which project in Sentry to send events to:
                o.Dsn = "dsn-link";
                // When configuring for the first time, to see what the SDK is doing:
                o.Debug = true; // dont set to true pls -medicineaddict
                // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                // We recommend adjusting this value in production.
                o.TracesSampleRate = 1.0;
                // Sample rate for profiling, applied on top of othe TracesSampleRate,
                // e.g. 0.2 means we want to profile 20 % of the captured transactions.
                // We recommend adjusting this value in production.
                o.ProfilesSampleRate = 1.0;

                o.AutoSessionTracking = true;

                // Requires NuGet package: Sentry.Profiling
                // Note: By default, the profiler is initialized asynchronously. This can
                // be tuned by passing a desired initialization timeout to the constructor.
                o.AddIntegration(new ProfilingIntegration(
                    // During startup, wait up to 500ms to profile the app startup code.
                    // This could make launching the app a bit slower so comment it out if you
                    // prefer profiling to start asynchronously
                    TimeSpan.FromMilliseconds(500)
                ));
            });

            try
            {
                Console.WriteLine("[BFCLIENT] Trying to execute a holiday action...");
                HolidayManager.DoHolidayAction();

                Console.WriteLine("[BFCLIENT] Now running the interactive console UI");
                Gui.MessageWaitForKey("WARNING! This program isn't fully completed yet, press any key to continue...");
                RunInteractive();
            }
            catch (Exception ex) 
            {
                SentrySdk.CaptureException(ex);
                Gui.Message(ex.ToString() + "\n", 0);
                Gui.MessageWaitForKey("We're sorry that you've experienced this problem.\n" +
                    "Don't worry! This issue reported and our team is working on it!\n" +
                    "Press any key to exit...");
            }
        }

        static void RunInteractive()
        {
            Console.Clear();
            Gui.Message("Welcome to Beta Fortress Client!\n", 0);
            Gui.Message("[ 1 ] Install/Update Beta Fortress\n" +
                        "[ 2 ] Configure for server hosting\n" +
                        "[ 3 ] Run Beta Fortress normally\n" +
                        "[ 4 ] Run Beta Fortress as a dedicated server\n" +
                        "[ 5 ] Quit\n", 0);
            string choice = Gui.MessageInput("Select your option:");
            switch(choice)
            {
                case "1":
                    string dir1 = Gui.MessageInput("Please enter where your Steam's sourcemods directory:");
                    if(Directory.Exists(dir1)) 
                    {
                        if(!Directory.Exists(dir1 + "/bf"))
                            Task.Run(async() => await ModManager.InstallMod(dir1));
                        else
                            Task.Run(async() => await ModManager.UpdateMod(dir1));
                    }
                    else
                    {
                        Gui.MessageEnd("Incorrect, wrong, missing directory.", 1);
                    }

                    if(Gui.MessageYesNo("Do you want to go back to the menu?"))
                    {
                        RunInteractive();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                    break;
                case "2":
                    #if _WINDOWS
                    if(ModManager.IsModInstalled)
                    {
                        if(File.Exists(ModManager.GetModPath + "/cfg/server.cfg"))
                        {
                            if(Gui.MessageYesNo("The server configuration file exists. Do you want to edit it now?"))
                            {
                                Process p = new Process();
                                p.StartInfo.FileName = "C:\\windows\\notepad.exe";
                                p.StartInfo.Arguments = ModManager.GetModPath + "/cfg/server.cfg";
                                p.Start();
                                p.WaitForExit();
                            }
                        }
                        else
                        {
                            File.CreateText(ModManager.GetModPath + "/cfg/server.cfg");

                            if(Gui.MessageYesNo("Created the server configuration file. Do you want to edit it now?"))
                            {
                                Process p = new Process();
                                p.StartInfo.FileName = "C:\\windows\\notepad.exe";
                                p.StartInfo.Arguments = ModManager.GetModPath + "/cfg/server.cfg";
                                p.Start();
                                p.WaitForExit();
                            }
                        }

                        if(Gui.MessageYesNo("Do you want to go back to the menu?"))
                        {
                            RunInteractive();
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                    #else
                    string dir2 = Gui.MessageInput("Please enter where your Steam's sourcemods directory:");
                    if(Directory.Exists(dir2) && Directory.Exists(dir2 + "/bf"))
                    {
                        dir2 = dir2 + "/bf";
                        
                        if(File.Exists(dir2 + "/cfg/server.cfg"))
                        {
                            if(Gui.MessageYesNo("The server configuration file exists. Do you want to edit it now?"))
                            {
                                Process p = new Process();
                                #if _WINDOWS
                                p.StartInfo.FileName = "C:\\windows\\notepad.exe";
                                #else
                                p.StartInfo.FileName = "/bin/nano";
                                #endif
                                p.StartInfo.Arguments = dir2 + "/cfg/server.cfg";
                                p.Start();
                                p.WaitForExit();
                            }
                        }
                        else
                        {
                            Gui.Message("Creating the server configuration file.", 0);
                            File.Create(dir2 + "/cfg/server.cfg");
                            if(Gui.MessageYesNo("Created the server configuration file. Do you want to edit it now?"))
                            {
                                Process p = new Process();
                                p.StartInfo = new ProcessStartInfo();
                                #if _WINDOWS
                                p.StartInfo.FileName = "C:\\windows\\notepad.exe";
                                #else
                                p.StartInfo.FileName = "/bin/nano";
                                #endif
                                p.StartInfo.Arguments = dir2 + "/cfg/server.cfg";
                                p.Start();
                                p.WaitForExit();
                            }
                        }
                    }
                    else
                    {
                        Gui.Message("Beta Fortress is not installed.\n", 0);
                    }
                    #endif

                    if(Gui.MessageYesNo("Do you want to go back to the menu?"))
                    {
                        RunInteractive();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                    break;
                case "3":
                    Gui.Message("Coming soon!\n", 0);
                    if(Gui.MessageYesNo("Do you want to go back to the menu?"))
                    {
                        RunInteractive();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                    break;
                case "4":
                    Gui.Message("Coming soon!\n", 0);
                    if(Gui.MessageYesNo("Do you want to go back to the menu?"))
                    {
                        RunInteractive();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Gui.Message("Please enter 1, 2, 3, 4 or 5.", 0);
                    if(Gui.MessageYesNo("Do you want to go back to the menu?"))
                    {
                        RunInteractive();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                    break;
            }
        }
    }
}
