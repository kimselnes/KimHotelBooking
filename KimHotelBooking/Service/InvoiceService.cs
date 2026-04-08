using KimHotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace KimHotelBooking.Service
{
    public class InvoiceService
    {
        public static Invoice Invoice { get; set; } = new Invoice();

        public static Invoice SetDatesPaidBooking(Booking booking)
        {
            Invoice.IssueDate = DateTime.Now;
            Invoice.DueDate = Invoice.IssueDate.AddDays(7);
            Invoice.isPaid = false;
            Invoice.Booking = booking;

            return Invoice;
        }

        public static decimal DetermineFullInvoicePrice(Room room, int amountOfDays)
        {
            return room.PricePerNight * amountOfDays;
        }
    }
}
