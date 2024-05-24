using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace BetaFortressTeam.BetaFortressClient.Util
{
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

        public static void RunApp( int appId )
        {
            Process p = new Process();
            p.StartInfo.FileName = GetSteamPath + "\\steam.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = "-applaunch " + appId;
            p.Start();
        }

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
