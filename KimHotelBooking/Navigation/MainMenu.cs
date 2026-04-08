using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Navigation.CustomerCRUDs;
using KimHotelBooking.Navigation.NewBooking;
using KimHotelBooking.Navigation.RoomCRUDs;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation
{
    public class MainMenu
    {
        public static void Run()
        {
            var prompt = "Välkommen till Kim's Hotel";
            
            List<string> options = ["Boka rum",
                "Hantera rum",
                "Hantera kunder",
                "Hantera bokningar",
                "Avsluta",
                "TEST"]; // TA BORT TEST NÄR APPEN ÄR KLAR!!!

            Menu menu = new Menu(prompt, options);
            int selectedIndex = menu.Run();


            switch (selectedIndex)
            {
                case 0:
                    // Book a room.
                    NewBookingMenu.Run();
                    break;

                case 1:
                    // CRUD rooms.
                    RoomMenu.Run();
                    break;

                case 2:
                    // CRUD customers.
                    //var customerMenu = new CustomerMenu();
                    //customerMenu.Run();
                    CustomerMenu.Run();
                    break;

                case 3:
                    // CRUD bookings.
                    break;

                case 4:
                    // Quit app.
                    Environment.Exit(0);
                    break;

                case 5: // [TEST] - TA BORT EFTER ATT APPEN ÄR KLAR!!!

                    var oldBooking = new Booking() { StartDate = new DateTime(2026, 01, 10), EndDate = new DateTime(2026, 01, 20) };
                    var newBooking = new Booking() { StartDate = new DateTime(2026, 01, 19), EndDate = new DateTime(2026, 01, 21) };

                    //var bookingService = new BookingService();
                    var isAvaiable = BookingService.IsDateOverlap(newBooking, oldBooking);

                    Console.WriteLine($"{(isAvaiable == true ? "Overlap" : "INTE overlap")}");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;
            }



            Utility.WaitForKeyPress();
        }
    }
}
