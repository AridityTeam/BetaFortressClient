//#define HAS_SETUP // startup setup process is still buggy
//#define RELEASE_BUILD
#define BETA_BUILD
//#define CONFIDENTIAL_BUILD

using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
#if HAS_SETUP
using Microsoft.Win32;
#endif
using System.Reflection;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

// BF Client-specific namespaces
using BetaFortressTeam.BetaFortressClient.Util;
using BetaFortressTeam.BetaFortressClient.Gui;
using BetaFortressTeam.BetaFortressClient.Updater.Util;

namespace BetaFortressTeam.BetaFortressClient.Startup
{
    static class BFClientMain
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        static string[] commandLineArgs = null;

        public static string[] CommandLineArgs
        {
            get { return commandLineArgs; }
        }

        private static bool IsElevated()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        [STAThread()]
        public static void Main(string[] args)
        {
            Console.WriteLine("[ BFCLIENT ] Starting up...");
            commandLineArgs = args;
            #if HAS_SETUP
            if(!SetupManager.FirstRun)
            {
                SetupManager.FirstRun = true;  
                
                if(SetupManager.FirstRun && !SetupManager.HasCompletedSetup && !IsElevated())
                {
                    var path = Assembly.GetExecutingAssembly().Location;
                    using (var process = Process.Start(new ProcessStartInfo(path)
                    {
                        Verb = "runas"
                    }))
                    {
                        Application.Exit();
                    }
                }

                try
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\PracticeMedicine\\BFClient");

                    RegistryKey bfClientKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PracticeMedicine\\BFClient");
                    if(bfClientKey != null)
                    {
                        try
                        {
                            bfClientKey.GetValue("FirstRun");
                            bfClientKey.SetValue("FirstRun", "0");
                            SetupManager.FirstRun = false;
                        }
                        catch(Exception) // happens if the registry was null
                        {
                            bfClientKey.SetValue("FirstRun", "1");
                            SetupManager.FirstRun = true; 
                        }
                    }
                }
                catch (Exception ex)
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\PracticeMedicine\\BFClient");
                    if(key == null)
                    {
                        key.CreateSubKey("Properties");
                    }
                }

                SetupManager.IsRunningSetup = false;
                SetupManager.HasCompletedSetup = false;
            }
            else
            {
                SetupManager.FirstRun = false;       
                SetupManager.IsRunningSetup = false;
                SetupManager.HasCompletedSetup = true;
            }
            #endif

            bool isWindows = System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows);
            if(isWindows)
            {

            }

            #if RELEASE_BUILD // place these here for future purposes
            SquirrelManager.CheckForUpdates();
            #elif BETA_BUILD
            Task.Run(async() => await SquirrelManager.CheckForUpdates());
            #endif

            if(Directory.Exists(Steam.GetSteamPath))
            {
                Console.WriteLine("[ BFCLIENT ] Steam is already installed");
            }
            else
            {
                Console.WriteLine("[ BFCLIENT ] Could not find Steam! Assuming Steam is not installed.");
                DialogResult result = MessageBox.Show("To use this client, you must have Steam installed into your computer and we could not find Steam inside of your machine.\n" +
                    "By clicking on OK, you will be redirected into the Steam download page on your default browser.", 
                    "Beta Fortress Client", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                if(result == DialogResult.OK)
                {
                    Process.Start("https://store.steampowered.com/about");
                }
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
                if(!commandLineArgs.Contains("/console"))
                {
                    Run(commandLineArgs);
                }
                else
                {
                    if(!commandLineArgs.Contains("/doNotAllocConsole"))
                    {
                        AllocConsole();
                    }

                    if(!commandLineArgs.Contains("/initializeGui"))
                    {
                        Run(args);
                    }

                    if(commandLineArgs.Contains("/help"))
                    {
                        Console.WriteLine("Beta Fortress Client v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n" +
                                          "Usage: BetaFortressClient.exe /console [args]\n" +
                                          "/help - show this help text\n" +
                                          "/doNotAllocConsole - do not alloc the console" +
                                          "/initializeGui - init the gui even with the console" /*+
                                          "/uninstall - uninstalls the mod"*/);
                        Console.ReadKey();
                    }

                    //if(commandLineArgs.Contains("/uninstall"))
                    //{
                    //    MainForm.UninstallBetaFortress();
                    //}
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Exception has occured!!");
                Console.WriteLine(e);
                Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Writing log...");

                using( StreamWriter writer = new StreamWriter( "./bfclient.BetaFortressTeam" + ".exception.log" ) )
                {
                    OperatingSystem os = Environment.OSVersion;
                    Version ver = os.Version;

                    if(commandLineArgs.Contains("/console"))
                    {
                        writer.WriteLine( "EXCEPTIONS OCCURED AS OF " + DateTime.Now );
                        writer.WriteLine( "NOTE!!!!!: INSTANCE IS IN CONSOLE MODE AS OF " + DateTime.Now );
                        writer.WriteLine("=================================== OS VERSION DETAILS ===================================");
                        writer.WriteLine("Version: " + os.Version.ToString() );
                        writer.WriteLine("  Major version: " + ver.Major);
                        writer.WriteLine("  Major revision: " + ver.MajorRevision);
                        writer.WriteLine("  Minor version: " + ver.Minor);
                        writer.WriteLine("  Minor revision: " + ver.MinorRevision);
                        writer.WriteLine("  Build: " + ver.Build);
                        writer.WriteLine("Platform: " + os.Platform.ToString() );
                        writer.WriteLine("SP: " + os.ServicePack.ToString() );
                        writer.WriteLine("Version String: " + os.VersionString.ToString() );
                        writer.WriteLine("==========================================================================================");
                        writer.WriteLine("==================================== EXCEPTION DETAILS ====================================");
                        writer.WriteLine( e );
                        writer.WriteLine("===========================================================================================");
                        writer.Close();
                    }
                    else
                    {
                        writer.WriteLine( "EXCEPTIONS OCCURED AS OF " + DateTime.Now );
                        writer.WriteLine("=================================== OS VERSION DETAILS ===================================");
                        writer.WriteLine("Version: " + os.Version.ToString() );
                        writer.WriteLine("  Major version: " + ver.Major);
                        writer.WriteLine("  Major revision: " + ver.MajorRevision);
                        writer.WriteLine("  Minor version: " + ver.Minor);
                        writer.WriteLine("  Minor revision: " + ver.MinorRevision);
                        writer.WriteLine("  Build: " + ver.Build);
                        writer.WriteLine("Platform: " + os.Platform.ToString() );
                        writer.WriteLine("SP: " + os.ServicePack.ToString() );
                        writer.WriteLine("Version String: " + os.VersionString.ToString() );
                        writer.WriteLine("==========================================================================================");
                        writer.WriteLine("==================================== EXCEPTION DETAILS ====================================");
                        writer.WriteLine( e );
                        writer.WriteLine("===========================================================================================");
                        writer.Close();
                    }

                    Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Successfully writted the log file.");
                }

                MessageBox.Show("An error has occured while trying to execute a function inside Beta Fortress Client\nAn exception log file has been created named " 
                    + @"""bfclient.BetaFortressTeam.exception.log""" + "\nPlease upload the mentioned file name to the developers!", 
                    "Beta Fortress Client - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void Run(string[] args)
        {
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                #if HAS_SETUP
                if(SetupManager.HasCompletedSetup == false && SetupManager.IsRunningSetup == false
                    || SetupManager.FirstRun == true)
                {
                    Console.WriteLine("[ BFCLIENT ] Loading SetupForm...");
                    Application.Run(new SetupForm());
                }
                else
                {
                    Console.WriteLine("[ BFCLIENT ] Loading MainForm...");
                    Application.Run(new MainForm());
                }
                #else
                Console.WriteLine("[ BFCLIENT ] Loading UpdateForm...");
                Application.EnableVisualStyles();
                Application.Run(new MainForm());
                #endif
            }
            finally
            {
                Console.WriteLine("[ BFCLIENT ] Leaving function: Run(string[] args) ");

                #if RELEASE_BUILD // place these here for future purposes
                Task.Run(async () => SquirrelManager.CheckForUpdates());
                #elif BETA_BUILD
                Task.Run(async () => await SquirrelManager.CheckForUpdates());
                #endif
            }
        }
    }
}
