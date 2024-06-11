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
