using BetaFortressTeam.BetaFortressClient.Util;
using System.Diagnostics;

namespace BetaFortressTeam.BetaFortressClient
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("[BFCLIENT] Starting up the program...");

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
                Gui.Message(ex.ToString(), 0);
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
                    string dir2 = Gui.MessageInput("Please enter where your Steam's sourcemods directory:");
                    if(Directory.Exists(dir2) && Directory.Exists(dir2 + "/bf"))
                    {
                        dir2 = dir2 + "/bf";
                        
                        if(File.Exists(dir2 + "/cfg/server.cfg"))
                        {
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
                                Task.Run(async() => await p.WaitForExitAsync());
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
                                Task.Run(async() => await p.WaitForExitAsync());
                            }
                        }
                    }
                    else
                    {
                        Gui.Message("Beta Fortress is not installed.\n", 0);
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
