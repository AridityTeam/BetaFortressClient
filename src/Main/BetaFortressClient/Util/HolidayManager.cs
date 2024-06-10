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

namespace BetaFortressTeam.BetaFortressClient.Util
{
    public static class HolidayManager
    {
        public static bool IsAprilFools()
        {
            DateTime dt = new DateTime();
            if(dt.Day == 1 && dt.Month == 4)
            {
                return true;
            }
            return false;
        }

        public static bool IsChristmas()
        {
            DateTime dt = new DateTime();
            if(dt.Day == 25 && dt.Month == 12)
            {
                return true;
            }
            return false;
        }
        
        public static bool IsHalloween()
        {
            DateTime dt = new DateTime();
            if(dt.Day == 31 && dt.Month == 10)
            {
                return true;
            }
            return false;
        }
        
        public static bool IsPlaysBirthday()
        {
            DateTime dt = new DateTime();
            if(dt.Day == 15 && dt.Month == 12)
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// guess who is the twins here
        /// </summary>
        /// <returns></returns>
        public static bool IsTheMfingTwinsBirthday()
        {
            DateTime dt = new DateTime();
            if(dt.Day == 8 && dt.Month == 10)
            {
                return true;
            }
            return false;
        }

        private static bool IsYourDickSmall()
        {
            return true;
        }

        // todo -- idk when beta fortress was first started its development
        public static bool IsBetaFortressBirthday()
        {
            return false;
        }

        public static void DoHolidayAction()
        {
            if(IsAprilFools())
            {
                Console.WriteLine("[ BFCLIENT HOLIDAY MANAGER ]: Executing holiday action for AprilFools");
            }
            else if ( IsChristmas() )
            {
                Console.WriteLine("[ BFCLIENT HOLIDAY MANAGER ]: Executing holiday action for Christmas");
            }
            else if ( IsHalloween() )
            {
                Console.WriteLine("[ BFCLIENT HOLIDAY MANAGER ]: Executing holiday action for Halloween");
            }
            else if ( IsPlaysBirthday() )
            {
                // happy birthday! but idk what to add in bf client :(  
                Console.WriteLine("[ BFCLIENT HOLIDAY MANAGER ]: Executing holiday action for PlaysBirthday");
            }
        }
    }
}
