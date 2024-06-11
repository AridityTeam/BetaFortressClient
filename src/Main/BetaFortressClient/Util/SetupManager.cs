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
    public static class SetupManager
    {
        public static bool FirstRun = false;
        public static bool HasCompletedSetup = false;
        public static bool IsRunningSetup = false;

        public static bool HasMissingModFiles()
        {
            string modPath = Steam.GetSourceModsPath + "/bf";
            if (!Directory.Exists(modPath + "/bin"))
            {
                return true;
            }
            if (!Directory.Exists(modPath + "/materials"))
            {
                return true;
            }
            if (!Directory.Exists(modPath + "/scripts"))
            {
                return true;
            }
            if (!Directory.Exists(modPath + "/sound"))
            {
                return true;
            }
            if (!File.Exists(modPath + "/gameinfo.txt"))
            {
                return true;
            }
            return false;
        }
    }
}
