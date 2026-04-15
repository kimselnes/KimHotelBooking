using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Navigation.BookingCRUDs;
using KimHotelBooking.Navigation.CheckInOut;
using KimHotelBooking.Navigation.CustomerCRUDs;
using KimHotelBooking.Navigation.InvoiceCRUDs;
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
            var mainPrompt = "Välkommen till Kim's Hotel";

            List<string> mainOptions =
            [
                "Gäster",
                "Admin",                
                "Kontrollera fakturor",
                "Avsluta",
                "TEST" // TA BORT TEST NÄR APPEN ÄR KLAR!!!
            ];

            Menu mainMenu = new Menu(mainPrompt, mainOptions);
            int mainSelectedIndex = mainMenu.Run();


            switch (mainSelectedIndex)
            {
                case 0:
                    GuestMenu();
                    break;

                case 1:
                    AdminMenu();
                    break;

                case 2:
                    InvoicePastDueDate.Run();
                    Run();
                    break;

                case 3:
                    Environment.Exit(0);
                    break;

                case 4: // [TEST] - TA BORT EFTER ATT APPEN ÄR KLAR!!! 
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

        public static void GuestMenu()
        {
            var guestPrompt = "\u001b[1mGÄST MENY\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vad vill du göra?";

            List<string> guestOptions =
            [
                "Boka rum",
                "Avboka rum/Ändra bokning",
                "Betala faktura",
                "Gå tillbaka"
            ];

            Menu guestMenu = new Menu(guestPrompt, guestOptions);
            int guestSelectedIndex = guestMenu.Run();

            switch (guestSelectedIndex)
            {
                case 0:
                    NewBookingMenu.Run();
                    break;

                case 1:
                    BookingMenu.Run();
                    break;

                case 2:
                    InvoiceMenu.Run();
                    break;

                case -1:
                    Run();
                    break;
            }
        }

        public static void AdminMenu()
        {
            var adminPrompt = "\u001b[1mADMIN MENY\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vad vill du göra?";

            List<string> adminOptions =
            [
                "Checka in/ut",
                "Hantera rum",
                "Hantera kunder",
                "Hantera bokningar",
                "Gå tillbaka"
            ];

            Menu adminMenu = new Menu(adminPrompt, adminOptions);
            int adminSelectedIndex = adminMenu.Run();

            switch (adminSelectedIndex)
            {
                case 0:
                    CheckInOutMenu.Run();
                    break;

                case 1:
                    RoomMenu.Run();
                    break;

                case 2:
                    CustomerMenu.Run();
                    break;

                case 3: 

                    break;

                case -1:
                    Run();
                    break;
            }
        }
    }
}
