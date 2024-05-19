using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Squirrel;

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
                            var path = string.Format("{0}/BFClientUpdater.exe", Application.StartupPath);
                            using (var process = Process.Start(new ProcessStartInfo(path)
                            {
                                Verb = "runas",
                                Arguments = "/update"
                            }))
                            {
                                process.WaitForExit();
                            }
                        }
                    }
                }
            }
        }

        public static async Task Update()
        {
            using(var mgr = UpdateManager.GitHubUpdateManager("https://github.com/Beta-Fortress-2-Team/bf"))
            {
                await mgr.Result.UpdateApp();
            }
        }
    }
}
