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
using System.Diagnostics;
using System.Windows.Forms;
using BetaFortressTeam.BetaFortressClient.Util;

namespace BetaFortressTeam.BetaFortressClient.Gui
{
    public partial class SDKLauncherForm : Form
    {
        public SDKLauncherForm()
        {
            InitializeComponent();
        }

        private void btnHammer_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = Steam.GetSteamAppsPath + "Source SDK Base 2013 Multiplayer/bin/hammer.exe";
            p.StartInfo.Arguments = "-game " + ModManager.GetModPath;
            p.Start();
        }
    }
}
