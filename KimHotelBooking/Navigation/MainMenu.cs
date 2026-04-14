using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Navigation.BookingCRUDs;
using KimHotelBooking.Navigation.CheckInOut;
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
            
            List<string> options = 
            [
                "Boka rum",
                "Avboka rum/Ändra bokning",
                "Checka in/ut",
                "Hantera rum",
                "Hantera kunder",
                "Hantera bokningar",
                "Avsluta",
                "TEST" // TA BORT TEST NÄR APPEN ÄR KLAR!!!
            ]; 

            Menu menu = new Menu(prompt, options);
            int selectedIndex = menu.Run();


            switch (selectedIndex)
            {
                case 0:
                    // Book a room.
                    NewBookingMenu.Run();
                    break;

                case 1: // Cancel booking/Change booking.
                    BookingMenu.Run();
                    break;

                case 2: // Check in/out.
                    CheckInOutMenu.Run();
                    break;

                case 3:
                    // CRUD rooms.
                    RoomMenu.Run();
                    break;

                case 4:
                    // CRUD customers.
                    //var customerMenu = new CustomerMenu();
                    //customerMenu.Run();
                    CustomerMenu.Run();
                    break;

                case 5:
                    // CRUD bookings.
                    break;

                case 6:
                    // Quit app.
                    Environment.Exit(0);
                    break;

                case 7: // [TEST] - TA BORT EFTER ATT APPEN ÄR KLAR!!! 
                    // -----------------------------------------------------------------------
                    //var oldBooking = new Booking() { StartDate = new DateTime(2026, 01, 10), EndDate = new DateTime(2026, 01, 20) };
                    //var newBooking = new Booking() { StartDate = new DateTime(2026, 01, 19), EndDate = new DateTime(2026, 01, 21) };

                    ////var bookingService = new BookingService();
                    //var isAvaiable = BookingService.IsDateOverlap(newBooking, oldBooking);

                    //Console.WriteLine($"{(isAvaiable == true ? "Overlap" : "INTE overlap")}");
                    //Console.ReadLine();
                    //Environment.Exit(0);
                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - --------------------------------------------
                    List<string> bookingsStrings = new List<string>();
                    var bookingsList = BookingService.GetAllBookingsList().Where(b => b.CustomerId == 3 && b.IsActive == true).ToList();
                    var roomsList = RoomService.GetAllRoomsList().Where(r => r.IsActive == true).ToList();
                    var invoiceList = InvoiceService.GetAllInvoices().Where(i => i.IsActive == true).ToList();



                    foreach (var booking in bookingsList)
                    {
                        var room = roomsList.FirstOrDefault(r => r.Id == booking.RoomId);
                        var invoice = invoiceList.FirstOrDefault(i => i.BookingId == booking.Id);

                        var tempString = $"Datum: {booking.StartDate.ToString("yyyy-MM-dd")} - {booking.EndDate.ToString("yyyy-MM-dd")}\n" +
                            $"{room.Type}-rum med {booking.RoomBeds} sängar.\n" +
                            $"Faktura: {invoice.TotalPrice} ({(invoice.IsPaid == true ? "\u001b[32mBetald\u001b[0m" : "\u001b[31mEj betald\u001b[0m")})";

                        bookingsStrings.Add(tempString);
                        Console.WriteLine(tempString);
                    }
                    Utility.WaitForKeyPress();
                    break;
            }

            Utility.WaitForKeyPress();
        }
    }
}
