using System;
using System.IO;

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
                writer.WriteLine(config);
            }
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
