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

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace BetaFortressTeam.BetaFortressClient.Util
{
    /// <summary>
    /// Beta Fortress Team's own implementation of the Steam class like from TF2CLauncher
    /// DO NOT CHANGE UNLESS YOU KNOW WHAT YOUR DOING
    /// </summary>
    public static class Steam
    {
        /// <summary>
        /// Returns a value where the Steam client was installed
        /// </summary>
        public static string GetSteamPath
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");
                if (key != null)
                {
                    // make sure the path actually exists before returning the value
                    if (Directory.Exists(key.GetValue("SteamPath").ToString()))
                    {
                        return key.GetValue("SteamPath").ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Returns a value where the "sourcemods" directory is
        /// </summary>
        public static string GetSourceModsPath
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");
                if (key != null)
                {
                    // make sure the path actually exists before returning the value
                    if (Directory.Exists(key.GetValue("SourceModInstallPath").ToString()))
                    {
                        return key.GetValue("SourceModInstallPath").ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Returns the "Steam/steamapps/common" directory if it exists
        /// </summary>
        public static string GetSteamAppsPath
        {
            get
            {
                if (Directory.Exists(GetSteamPath + "/steamapps/common"))
                {
                    return GetSteamPath + "/steamapps/common";
                }
                return null;
            }
        }

        /// <summary>
        /// Checks if Steam is installed by checking if the registry keys for Steam exists or checking if the Steam installation directory exists
        /// </summary>
        public static bool IsSteamInstalled
        {
            get
            {
                if (Directory.Exists(GetSteamPath))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// **WARNING!!!**
        /// not implemented in linux
        /// Checks if the specific app ID is installed
        /// (ex.: 220 is HL2, 240 is CS:S, 440 is TF2)
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static bool IsAppInstalled(int appId)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\Apps\" + appId);
            if (key != null)
            {
                // make sure the path actually exists before returning the value
                if (key.GetValue("Installed").ToString() == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// **WARNING!!!**
        /// not implemented in linux
        /// Checks if the specific app ID is updating
        /// Only useful in some cases
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static bool IsAppUpdating(int appId)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\Apps\" + appId);
            if (key != null)
            {
                // make sure the path actually exists before returning the value
                if (key.GetValue("Updating").ToString() == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // note -- for source engine games: use a mutex instead but this is another workaround
        /// <summary>
        /// **WARNING!!!**
        /// not implemented in linux
        /// Checks if the specific app ID is running
        /// NOTE: If the game/software has a mutex, you can use the Mutex class as an better alternative.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static bool IsAppRunning(int appId)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam" + appId);
            if (key != null)
            {
                if (key.GetValue("RunningAppID").ToString() == $"{appId}")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Runs a specific app ID
        /// </summary>
        /// <param name="appId"></param>
        public static void RunApp(int appId)
        {
            Process p = new Process();
#if WINDOWS
            p.StartInfo.FileName = Steam.GetSteamPath + "/steam.exe";
#elif POSIX
            p.StartInfo.FileName = "/usr/bin/steam";
#endif
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = "-applaunch " + appId;
            p.Start();
        }

        /// <summary>
        /// Runs a specific app ID with extra launch options
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="args"></param>
        public static void RunApp(int appId, string args)
        {
            Process p = new Process();
#if WINDOWS
            p.StartInfo.FileName = Steam.GetSteamPath + "/steam.exe";
#elif POSIX
            p.StartInfo.FileName = "/usr/bin/steam";
#endif
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = "-applaunch " + appId + " " + args;
            p.Start();
        }
    }
}
