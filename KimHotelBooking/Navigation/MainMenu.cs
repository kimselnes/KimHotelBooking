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
            Utility.FullClearConsole();
            var mainPrompt = "Välkommen till Kim's Hotel";

            List<string> mainOptions =
            [
                "Gäster",
                "Admin",                
                "Kontrollera fakturor",
                "Avsluta"
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
            }
        }

        public static void GuestMenu()
        {
            Utility.FullClearConsole();
            var guestPrompt = "\u001b[1mGÄST MENY\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vad vill du göra?";

            List<string> guestOptions =
            [
                "Boka rum",
                "Avboka rum/Ändra bokning",
                "Betala faktura"
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
            Utility.FullClearConsole();
            var adminPrompt = "\u001b[1mADMIN MENY\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vad vill du göra?";

            List<string> adminOptions =
            [
                "Checka in/ut",
                "Visa alla bokningar",
                "Hantera rum",
                "Hantera kunder"
            ];

            Menu adminMenu = new Menu(adminPrompt, adminOptions);
            int adminSelectedIndex = adminMenu.Run();

            switch (adminSelectedIndex)
            {
                case 0:
                    CheckInOutMenu.Run();
                    break;

                case 1: 
                    BookingReadMenu.Run();
                    break;

                case 2:
                    RoomMenu.Run();
                    break;

                case 3:
                    CustomerMenu.Run();
                    break;

                case -1:
                    Run();
                    break;
            }
        }
    }
}
