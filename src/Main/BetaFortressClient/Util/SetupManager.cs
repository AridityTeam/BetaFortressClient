using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibGit2Sharp;

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
            if(!Directory.Exists(modPath + "/bin"))
            {
                return true;
            }
            if(!Directory.Exists(modPath + "/materials"))
            {
                return true;
            }
            if(!Directory.Exists(modPath + "/scripts"))
            {
                return true;
            }
            if(!Directory.Exists(modPath + "/sound"))
            {
                return true;
            }
            return false;
        }
    }
}
