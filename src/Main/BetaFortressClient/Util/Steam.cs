using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

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
                if(key != null)
                {
                    // make sure the path actually exists before returning the value
                    if(Directory.Exists(key.GetValue("SteamPath").ToString()))
                    { 
                        return key.GetValue("SteamPath").ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
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
                if(key != null)
                {
                    // make sure the path actually exists before returning the value
                    if(Directory.Exists(key.GetValue("SourceModInstallPath").ToString()))
                    { 
                        return key.GetValue("SourceModInstallPath").ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Checks if Steam is installed by checking if the registry keys for Steam exists or checking if the
        /// Steam installation directory exists
        /// </summary>
        public static bool IsSteamInstalled
        {
            get
            {
                // use the registry keys
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");
                if(key != null)
                {
                    return true;
                }

                // or check if the installation path exists
                if(Directory.Exists(GetSteamPath))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Checks if the specific app ID is installed
        /// (ex.: 220 is HL2, 240 is CS:S, 440 is TF2)
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static bool IsAppInstalled( int appId )
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\Apps\" + appId);
            if(key != null)
            {
                // make sure the path actually exists before returning the value
                if(key.GetValue("Installed").ToString() == "1")
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
        /// Checks if the specific app ID is updating
        /// Only useful in some cases
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static bool IsAppUpdating( int appId )
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\Apps\" + appId);
            if(key != null)
            {
                // make sure the path actually exists before returning the value
                if(key.GetValue("Updating").ToString() == "1")
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
        /// Checks if the specific app ID is running
        /// NOTE: If the game/software has a mutex, you can use the Mutex class as an better alternative.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static bool IsAppRunning( int appId )
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam" + appId);
            if(key != null)
            {
                // make sure the path actually exists before returning the value
                if(key.GetValue("RunningAppID").ToString() == $"{appId}")
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
        public static void RunApp( int appId )
        {
            Process p = new Process();
            p.StartInfo.FileName = GetSteamPath + "\\steam.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = "-applaunch " + appId;
            p.Start();
        }

        /// <summary>
        /// Runs a specific app ID with extra launch options
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="args"></param>
        public static void RunApp( int appId, string args )
        {
            Process p = new Process();
            p.StartInfo.FileName = GetSteamPath + "\\steam.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = "-applaunch " + appId + " " + args;
            p.Start();
        }
    }
}
