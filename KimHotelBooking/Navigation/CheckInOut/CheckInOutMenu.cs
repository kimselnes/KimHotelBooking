using KimHotelBooking.Models;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.CheckInOut
{
    public class CheckInOutMenu
    {
        public static void Run()
        {
            var prompt = "\u001b[1mCHECK IN/OUT\u001b[0m\n" +
            "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
            "Vad vill du göra?";

            List<string> options =
            [
                "Checka in kund",
                "Checka ut kund"
            ];

            Menu menu = new Menu(prompt, options);
            int selectedIndex = menu.Run();

            switch (selectedIndex)
            {
                case 0:
                    // Check in.
                    CheckIn();
                    break;

                case 1:
                    // Check out.
                    CheckOut();
                    break;

                case -1:
                    // Back to main menu.
                    MainMenu.AdminMenu();
                    break;
            }
        }

        public static void CheckIn()
        {
            var currentDate = DateTime.Now.Date;

            var bookings = BookingService.GetAllBookingsList();
            var bookingsToCheckIn = new List<Booking>();
            foreach (var booking in bookings)
            {
                if (booking.StartDate == currentDate ||
                    booking.EndDate == currentDate ||
                    booking.StartDate < currentDate &&
                    booking.EndDate > currentDate)
                {
                    if (booking.IsCheckedIn == false && booking.IsActive == true)
                    {
                        bookingsToCheckIn.Add(booking);
                    }
                }
            }

            if (bookingsToCheckIn.Count == 0)
            {
                Console.WriteLine("\nInga bokningar att checka in.");
                Utility.WaitForKeyPress();
                Run();
            }
            else
            {
                var checkInOptions = BookingService.BookingListToStringsOnlyCheckedOut(bookingsToCheckIn);

                var checkInPrompt = "\u001b[1mCHECKA IN\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n" +
                $"Rum som går att checka in:";

                Menu checkInMenu = new Menu(checkInPrompt, checkInOptions);
                int checkInSelectedIndex = checkInMenu.Run();

                if (checkInSelectedIndex == -1)
                {
                    Run();
                }
                else
                {
                    var correctId = bookingsToCheckIn[checkInSelectedIndex].Id;
                    BookingService.CheckInCustomer(correctId);

                    Console.WriteLine("\n\u001b[32mKunden är incheckad!\u001b[0m");
                    Utility.WaitForKeyPress();
                    Run();
                }
            }
        }

        public static void CheckOut()
        {
            var currentDate = DateTime.Now.Date;

            var checkedInBookings = BookingService.GetAllIsCheckedIn();
            var bookingsToCheckOut = new List<Booking>();

            foreach (var booking in checkedInBookings)
            {
                bookingsToCheckOut.Add(booking);
            }

            if (bookingsToCheckOut.Count == 0)
            {
                Console.WriteLine("\nInga bokningar att checka ut.");
                Utility.WaitForKeyPress();
                Run();
            }
            else
            {
                var checkOutOptions = BookingService.BookingsWithListToString(bookingsToCheckOut);

                var checkOutPrompt = "\u001b[1mCHECKA UT\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n" +
                $"Rum som går att checka ut:";

                Menu checkOutMenu = new Menu(checkOutPrompt, checkOutOptions);
                int checkOutSelectedIndex = checkOutMenu.Run();

                if (checkOutSelectedIndex == -1)
                {
                    Run();
                }
                else
                {
                    var correctBookingId = bookingsToCheckOut[checkOutSelectedIndex].Id;
                    BookingService.CheckOutCustomer(correctBookingId);
                    BookingService.SetNotActive(correctBookingId);

                    InvoiceService.SetIsActiveFalseByBookingId(correctBookingId);

                    Console.WriteLine("\n\u001b[32mKunden är utcheckad!\u001b[0m");
                    Utility.WaitForKeyPress();
                    Run();
                }
            }
        }
    }
}
