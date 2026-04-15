using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.InvoiceCRUDs
{
    public class InvoicePastDueDate
    {
        public static void Run()
        {
            Console.Clear();
            var invoicesPastDue = InvoiceService.GetAllPastDue();

            if (invoicesPastDue.Count == 0)
            {
                Console.WriteLine("Inga förfallna fakturor hittades.");
                Utility.WaitForKeyPress();
                MainMenu.Run();
            }
            else
            {
                foreach (var invoice in invoicesPastDue)
                {
                    InvoiceService.Delete(invoice);

                    var booking = BookingService.GetByBookingId(invoice.BookingId);
                    BookingService.SetNotActive(booking.Id);
                    Console.WriteLine($"\nFaktura {invoice.Id} och bokningen {booking.Id} " +
                        $"har tagits bort eftersom fakturans förfallodatum har passerat.");
                }
            }

            Utility.WaitForKeyPress();
        }
    }
}
