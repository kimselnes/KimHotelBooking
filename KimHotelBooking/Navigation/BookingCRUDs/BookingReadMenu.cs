using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.BookingCRUDs
{
    public class BookingReadMenu
    {
        public static void Run()
        {
            Utility.FullClearConsole();
            Console.WriteLine("\u001b[1mVISA ALLA BOKNINGAR\u001b[0m\n");
            var bookingsGroupedByRoom = BookingService.GetBookingsGroupedDisplay();

            foreach (var line in bookingsGroupedByRoom)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();
            Utility.WaitForKeyPress();
            MainMenu.AdminMenu();
        }
    }
}
