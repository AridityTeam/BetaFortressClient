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
using System.Threading.Tasks;

namespace BetaFortressTeam.BetaFortressClient.Util
{
    public static class ModManager
    {
        static string ModPath = Steam.GetSourceModsPath + "/bf";

        public static string GetModPath
        {
            get
            {
                return ModPath;
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

        public static async void InstallMod(string modPath)
        {
            var gitProgress = new ProgressHandler((serverProgressOutput) =>
            {
                // Print output to console
                Console.Write(serverProgressOutput);
                return true;
            });

            if (!Directory.Exists(Steam.GetSourceModsPath + "/bf"))
            {
                CloneOptions cloneOptions = new CloneOptions();
                cloneOptions.FetchOptions.OnTransferProgress = TransferProgress;
                cloneOptions.FetchOptions.Depth = 1;
                cloneOptions.FetchOptions.OnProgress = gitProgress;

                var x = await Task.Run(() => Repository.Clone("https://github.com/Beta-Fortress-2-Team/bf.git", Steam.GetSourceModsPath + "/bf", cloneOptions));

                if (SetupManager.HasMissingModFiles())
                {
                    if (Gui.MessageYesNo("Beta Fortress Client has detected that your current installation has missing files.\n" +
                        "Do you want to reinstall?"))
                    {
                        Directory.Delete(Steam.GetSourceModsPath + "/bf", true);
                    }
                }
            }
        }

        public static bool TransferProgress(TransferProgress progress)
        {
            Console.WriteLine($"Objects: {progress.ReceivedObjects} of {progress.TotalObjects}, Bytes: {progress.ReceivedBytes}");
            return true;
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
