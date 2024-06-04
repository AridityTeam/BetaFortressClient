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

using System;
using System.IO;
using System.Runtime.CompilerServices;
using NLua;
using NLua.Exceptions;

using BetaFortressTeam.BetaFortressClient.Gui;
#if SQUIRREL_UPDATER
using BetaFortressTeam.BetaFortressClient.Updater.Util;
#endif

#if WINDOWS
#pragma warning disable 1416
#endif

namespace BetaFortressTeam.BetaFortressClient.Util
{
    public static class LuaManager
    {
        static string scriptDir = System.Windows.Forms.Application.StartupPath + "/addons";
        static Lua lua;

        private static string errorString = null;

        public static string ErrorText
        {
            get
            {
                return "An error occured loading an addon script:\n" + errorString;
            }
        }
        
        static void LoadDependencies()
        {
            lua = new Lua();
                
            lua.LoadCLRPackage();
        }

        public static void LoadLuaScripts()
        {
            Console.WriteLine("[BFCLIENT LUA MANAGER] Loading Beta Fortress Client Lua scripts...");

            LoadDependencies();

            try
            {
                lua.DoFile(string.Format("{0}/scripts/main/init.lua", System.Windows.Forms.Application.StartupPath));
            }
            catch(LuaException e)
            {
                Console.WriteLine("[BFCLIENT LUA MANAGER] An error occured loading a Lua file\n" + e.Message);
            }
        }

        public static void LoadAddonScripts()
        {
            try
            {
                LoadDependencies();

                string[] files = Directory.GetFiles(scriptDir, "*.lua", SearchOption.AllDirectories);
            
                if(Directory.Exists(scriptDir))
                {
                    foreach (string file in files) 
                    {
                        lua.DoFile(file);
                    }
                }
            }
            catch(LuaException e)
            {
                Console.WriteLine("[BFCLIENT LUA MANAGER] An error occured loading an addon script\n" + e.Message);
                errorString = "[BFCLIENT LUA MANAGER] An error occured loading an addon script\n" + e.Message;
            }
        }
    }
}
