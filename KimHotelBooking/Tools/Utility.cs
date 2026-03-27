using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Tools
{
    public static class Utility
    {
        public static void WaitForKeyPress()
        {
            Console.WriteLine("Tryck valfri knapp för att fortsätta.");
            Console.ReadKey(true);
        }
    }
}
