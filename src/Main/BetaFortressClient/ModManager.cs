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

using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetaFortressTeam.BetaFortressClient.Util
{
    public static class ModManager
    {
        #if _WINDOWS
        static string ModPath = Steam.GetSourceModsPath + "/bf";

        public static string GetModPath
        {
            get
            {
                if(Directory.Exists(ModPath))
                { 
                    return ModPath;
                }
                return null;
            }
        }

        public static bool IsModInstalled
        {
            get
            {
                if (Directory.Exists(ModPath))
                {
                    return true;
                }
                return false;
            }
        }

        public static string GetModVersion
        {
            get
            {
                using (StreamReader reader = new StreamReader(ModPath + "/version.txt"))
                {
                    return reader.ReadLine();
                }
            }
        }
        #endif

        public static string GetModName
        {
            get
            {
                return "Beta Fortress";
            }
        }

        public static void SetConfigs(Configuration.ConVars config)
        {
            using (StreamWriter writer = new StreamWriter(GetModPath + "/cfg/config.cfg"))
            {
                writer.Write(config);
            }
        }

        public static async Task InstallMod(string modPath)
        {
            await Task.Run(() => Console.Clear());

            var gitProgress = new ProgressHandler((serverProgressOutput) =>
            {
                // Print output to console
                Console.Write(serverProgressOutput);
                return true;
            });

            if (!Directory.Exists(modPath))
            {
                CloneOptions cloneOptions = new CloneOptions();
                cloneOptions.FetchOptions.OnTransferProgress = TransferProgress;
                cloneOptions.FetchOptions.Depth = 1;
                cloneOptions.FetchOptions.OnProgress = gitProgress;

                var x = await Task.Run(() => Repository.Clone("https://github.com/Beta-Fortress-2-Team/bf.git", modPath + "/bf", cloneOptions));

                #if _WINDOWS
                if (SetupManager.HasMissingModFiles())
                {
                    if (Gui.MessageYesNo("Beta Fortress Client has detected that your current installation has missing files.\n" +
                        "Do you want to reinstall?"))
                    {
                        Directory.Delete(ModPath, true);
                    }
                }
                #endif
            }
        }

        public static bool CheckForUpdates(string currentModPath)
        {
            using (var repo = new Repository(currentModPath))
            {
                var options = new FetchOptions();
                    options.Prune = true;
                    options.TagFetchMode = TagFetchMode.Auto;
                    options.Depth = 1;

                var remote = repo.Network.Remotes["origin"];
                var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                repo.Network.Fetch(remote.Name, refSpecs, options, "Checking for updates...");
            }
            return false;
        }

        public static async Task UpdateMod(string currentModPath)
        {
            var gitProgress = new ProgressHandler((serverProgressOutput) =>
            {
                // Print output to console
                Console.Write(serverProgressOutput);
                return true;
            });

            using (var repo = new Repository(currentModPath))
            {
                #if _WINDOWS
                if (SetupManager.HasMissingModFiles())
                {
                    if (Gui.MessageYesNo("Beta Fortress Client has detected that your current installation has missing files.\n" +
                        "Do you want to reinstall?"))
                    {
                        await Task.Run(() => Directory.Delete(ModPath, true));
                        await Task.Run(() => InstallMod(ModPath));
                    }
                }
                else
                {
                    var trackedBranch = repo.Head.TrackedBranch;

                    var options = new FetchOptions();
                    options.Prune = true;
                    options.TagFetchMode = TagFetchMode.Auto;
                    options.Depth = 1;

                    var remote = repo.Network.Remotes["origin"];
                    var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                    await Task.Run(() => repo.Network.Fetch(remote.Name, refSpecs, options, "Fetching remote..."));

                    Commit originHeadCommit = repo.ObjectDatabase.FindMergeBase(repo.Branches[trackedBranch.FriendlyName].Tip, repo.Head.Tip);
                    //CheckoutOptions checkoutOptions = new CheckoutOptions();
                    var pullOptions = new PullOptions();
                    pullOptions.FetchOptions = new FetchOptions();
                    pullOptions.FetchOptions.Prune = true;
                    pullOptions.FetchOptions.Depth = 1;
                    pullOptions.FetchOptions.OnProgress = gitProgress;
                    pullOptions.FetchOptions.OnTransferProgress = TransferProgress;
                    //await Task.Run(() => repo.Reset(ResetMode.Hard, originHeadCommit, checkoutOptions));
                    var id = new Identity("Beta Fortress Client user", "aridityteam@gmail.com");
                    var sig = new Signature(id, new DateTimeOffset(DateTime.Today));
                    var result = await Task.Run(() => Commands.Pull(repo, sig, pullOptions));
                    if (result.Status == MergeStatus.Conflicts)
                    {
                        Gui.Message("Conflict detected!!!", 0);
                        await Task.Delay(500);
                        return;
                    }
                    if (result.Status == MergeStatus.UpToDate)
                    {
                        Gui.Message("Mod is up-to-date.", 0);
                        await Task.Delay(500);
                        return;
                    }
                }
                #else
                var trackedBranch = repo.Head.TrackedBranch;

                var options = new FetchOptions();
                options.Prune = true;
                options.TagFetchMode = TagFetchMode.Auto;
                options.Depth = 1;

                var remote = repo.Network.Remotes["origin"];
                var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                await Task.Run(() => repo.Network.Fetch(remote.Name, refSpecs, options, "Fetching remote..."));

                Commit originHeadCommit = repo.ObjectDatabase.FindMergeBase(repo.Branches[trackedBranch.FriendlyName].Tip, repo.Head.Tip);
                //CheckoutOptions checkoutOptions = new CheckoutOptions();
                var pullOptions = new PullOptions();
                pullOptions.FetchOptions = new FetchOptions();
                pullOptions.FetchOptions.Prune = true;
                pullOptions.FetchOptions.Depth = 1;
                pullOptions.FetchOptions.OnProgress = gitProgress;
                pullOptions.FetchOptions.OnTransferProgress = TransferProgress;
                //await Task.Run(() => repo.Reset(ResetMode.Hard, originHeadCommit, checkoutOptions));
                var id = new Identity("Beta Fortress Client user", "aridityteam@gmail.com");
                var sig = new Signature(id, new DateTimeOffset(DateTime.Today));
                var result = await Task.Run(() => Commands.Pull(repo, sig, pullOptions));
                if (result.Status == MergeStatus.Conflicts)
                {
                    Gui.Message("Conflict detected!!!", 0);
                    await Task.Delay(500);
                    return;
                }
                if (result.Status == MergeStatus.UpToDate)
                {
                    Gui.Message("Mod is up-to-date.", 0);
                    await Task.Delay(500);
                    return;
                }
                #endif
            }
        }


        public static bool TransferProgress(TransferProgress progress)
        {
            Console.Write($"Objects: {progress.ReceivedObjects} of {progress.TotalObjects}, Bytes: {progress.ReceivedBytes}\n");
            return true;
        }

        // thanks YourLocalMoon!
        // even though it doesnt work you fucking swine
        public static (string Name, string Type, int NoModels, int NoHiModel, int NoCrosshair, string Developer, string DeveloperUrl, string Manual, int SteamAppId) ExtractGameInfo(string gameInfoContent)
        {
            string name = null;
            string type = null;
            int noModels = 0;
            int noHiModel = 0;
            int noCrosshair = 0;
            string developer = "Unknown";
            string developerUrl = "None";
            string manual = "None";
            int steamAppId = 0;

            var nameMatch = Regex.Match(gameInfoContent, @"\bgame\s+(.+)", RegexOptions.IgnoreCase);
            if (nameMatch.Success && nameMatch.Groups.Count > 1)
            {
                name = nameMatch.Groups[1].Value.Replace("\"", "").Trim();
            }

            var typeMatch = Regex.Match(gameInfoContent, @"\btype\s+(.+)", RegexOptions.IgnoreCase);
            if (typeMatch.Success && typeMatch.Groups.Count > 1)
            {
                type = typeMatch.Groups[1].Value.Trim();
            }

            var noModelsMatch = Regex.Match(gameInfoContent, @"\bnomodels\s+(\d+)", RegexOptions.IgnoreCase);
            if (noModelsMatch.Success && noModelsMatch.Groups.Count > 1)
            {
                noModels = int.Parse(noModelsMatch.Groups[1].Value.Trim());
            }

            var noHiModelMatch = Regex.Match(gameInfoContent, @"\bnohimodel\s+(\d+)", RegexOptions.IgnoreCase);
            if (noHiModelMatch.Success && noHiModelMatch.Groups.Count > 1)
            {
                noHiModel = int.Parse(noHiModelMatch.Groups[1].Value.Trim());
            }

            var noCrosshairMatch = Regex.Match(gameInfoContent, @"\bnocrosshair\s+(\d+)", RegexOptions.IgnoreCase);
            if (noCrosshairMatch.Success && noCrosshairMatch.Groups.Count > 1)
            {
                noCrosshair = int.Parse(noCrosshairMatch.Groups[1].Value.Trim());
            }

            var developerMatch = Regex.Match(gameInfoContent, @"\bdeveloper\s+""(.+)""", RegexOptions.IgnoreCase);
            if (developerMatch.Success && developerMatch.Groups.Count > 1)
            {
                developer = developerMatch.Groups[1].Value.Trim();
            }

            var developerUrlMatch = Regex.Match(gameInfoContent, @"\bdeveloper_url\s+""(.+)""", RegexOptions.IgnoreCase);
            if (developerUrlMatch.Success && developerUrlMatch.Groups.Count > 1)
            {
                developerUrl = developerUrlMatch.Groups[1].Value.Trim();
            }

            var manualMatch = Regex.Match(gameInfoContent, @"\bmanual\s+""(.+)""", RegexOptions.IgnoreCase);
            if (manualMatch.Success && manualMatch.Groups.Count > 1)
            {
                manual = manualMatch.Groups[1].Value.Trim();
            }

            var appIdMatch = Regex.Match(gameInfoContent, @"\bSteamAppId\s+(\d+)", RegexOptions.IgnoreCase);
            if (appIdMatch.Success && manualMatch.Groups.Count > 1)
            {
                steamAppId = int.Parse(manualMatch.Groups[1].Value.Trim());
            }

            return (name, type, noModels, noHiModel, noCrosshair, developer, developerUrl, manual, steamAppId);
        }
    }

    public class Configuration
    {
        public struct ConVars
        {
            public static string conCommand;
            public static string value;

            public ConVars(string conVar, string conValue)
            {
                conCommand = conVar;
                value = conValue;
            }
        }


    }
}
