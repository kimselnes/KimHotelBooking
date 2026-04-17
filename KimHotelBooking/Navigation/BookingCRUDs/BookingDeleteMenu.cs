using KimHotelBooking.Models;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.BookingCRUDs
{
    public class BookingDeleteMenu
    {
        public static Invoice Invoice { get; set; } = new Invoice();
        public static void Run(Customer customer)
        {
            var prompt = "\u001b[1mAVBOKA RUM\u001b[0m\n" +
            "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
            "Vilket rum vill du avboka?";

            var bookingsToDelete = BookingService.GetAllIsActiveCheckedOut(customer);
            var bookingsToDeleteOptions = BookingService.BookingsWithListToString(bookingsToDelete);

            if (bookingsToDelete.Count == 0)
            {
                Console.WriteLine("\n\u001b[31mDu har inga bokningar att avboka.\u001b[0m");
                Utility.WaitForKeyPress();
                BookingMenu.Run();
            }

            Menu menu = new Menu(prompt, bookingsToDeleteOptions);
            int selectedIndex = menu.Run();

            var correctId = 0;

            if (selectedIndex == -1)
            {
                BookingMenu.Run();
            }
            else
            {
                correctId = bookingsToDelete[selectedIndex].Id;
                BookingService.SetNotActive(correctId);

                Console.WriteLine("\n\u001b[32mRummet är avbokat!\u001b[0m");
            }
            Invoice = InvoiceService.GetInvoiceByBookingId(correctId);
            InvoiceService.Delete(Invoice);

            if (Invoice.IsPaid == true)
            {
                Console.WriteLine("\u001b[1mDu kommer att få en återbetalning eftersom fakturan redan är betald.\u001b[0m");
                Utility.WaitForKeyPress();
                Run(customer);
            }

            Run(customer);
        }
    }
}
