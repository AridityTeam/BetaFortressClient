﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Squirrel;

#if DEBUG
#pragma warning disable 1998 // this will disable the lack of await operators warning
#endif

namespace BetaFortressTeam.BetaFortressClient.Updater.Util
{
    public static class SquirrelManager
    {
        public static bool HasUpdates;
        public static string Message;

        public static async Task CheckForUpdates()
        {
            using(var mgr = UpdateManager.GitHubUpdateManager("https://github.com/Beta-Fortress-2-Team/bf"))
            {
                #if !DEBUG
                try
                {
                    var result = await mgr.Result.CheckForUpdate();

                    if(result.ReleasesToApply.Any())
                    {
                        var versionCount = result.ReleasesToApply.Count;
                        Console.WriteLine($"{versionCount} update(s) found.");

                        HasUpdates = true;  

                        var versionWord = versionCount > 1 ? "versions" : "version";
                        Message = new StringBuilder().AppendLine($"Beta Fortress Client is {versionCount} {versionWord} behind.").
                                                    AppendLine("If you choose to update, changes wont take affect until Beta Fortress Client is restarted.").
                                                    AppendLine("Would you like to download and install them?").
                                                    ToString();

                        DialogResult result1 = MessageBox.Show(SquirrelManager.Message, "Beta Fortress Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                
                        if(result1 == DialogResult.Yes)
                        {
                            DialogResult anothaResult = MessageBox.Show("To update your client's version\n" +
                            "Beta Fortress Client needs elevated/admin privileges. Continue?", "Beta Fortress Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            if(anothaResult == DialogResult.Yes)
                            {
                                var updateResult = await mgr.Result.UpdateApp();
                                DialogResult BruhResult = MessageBox.Show("Download complete!\n" +
                                                                        "Do you want to restart now?", 
                                                                        "Beta Fortress Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                                if(BruhResult == DialogResult.Yes)
                                {
                                    Application.Restart();

                                    // make sure the dumbass exited
                                    Application.Exit();
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    #if DEBUG
                    if(Debugger.IsAttached)
                    {
                        throw new InvalidOperationException("An error occured while checking for updates", e);
                    }
                    else
                    {
                        Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Exception has occured!!");
                        Console.WriteLine(e);
                        Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Writing log...");

                        using(StreamWriter writer = new StreamWriter("./bfclient.BetaFortressTeam.exception.log"))
                        {
                            OperatingSystem os = Environment.OSVersion;
                            Version ver = os.Version;

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

                            Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Successfully writted the log file.");
                        }

                        MessageBox.Show("An error occured while checking for updates!!!\n" +
                            "Please contact the developers!",
                            "Beta Fortress Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    #else
                    Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Exception has occured!!");
                    Console.WriteLine(e);
                    Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Writing log...");

                    using(StreamWriter writer = new StreamWriter("./bfclient.BetaFortressTeam.exception.log"))
                    {
                        OperatingSystem os = Environment.OSVersion;
                        Version ver = os.Version;

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

                        Console.WriteLine("[ BFCLIENT EXCEPTION HANDLER ] Successfully writted the log file.");
                    }

                    MessageBox.Show("An error occured while checking for updates!!!\n" +
                        "Please contact the developers!",
                        "Beta Fortress Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    #endif
                }
                #else
                Console.WriteLine("[ BFCLIENT UPDATE MANAGER ] Skipping Task function: CheckForUpdates()");
                #endif
            }
        }
    }
}
