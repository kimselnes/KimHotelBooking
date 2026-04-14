using KimHotelBooking.Models;
using KimHotelBooking.Repositories;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace KimHotelBooking.Service
{
    public class InvoiceService
    {
        public static Invoice Invoice { get; set; } = new Invoice();

        public static void Delete(Invoice invoice)
        {
            var invoiceRep = new InvoiceRepository();
            invoiceRep.SetNotActive(invoice);
        }

        public static List<Invoice> GetAllInvoices()
        {
            var invoiceRep = new InvoiceRepository();
            var invoices = invoiceRep.GetAll();

            return invoices;
        }

        public static Invoice GetInvoiceByBookingId(int bookingId)
        {
            var invoiceRep = new InvoiceRepository();
            var invoice = invoiceRep.GetInvoiceByBookingId(bookingId);

            return invoice;
        }

        public static Invoice SetInvoiceValues(Booking booking, Room room, Customer customer)
        {
            Invoice.IssueDate = DateTime.Now;
            Invoice.DueDate = Invoice.IssueDate.AddDays(10);
            Invoice.IsPaid = false;
            Invoice.BookingId = booking.Id;
            Invoice.CustomerId = customer.Id;

            Invoice.TotalPrice = DetermineFullInvoicePrice(room, (booking.EndDate - booking.StartDate).Days + 1);

            return Invoice;
        }

        //public static Invoice SetDatesPaidBooking(Booking booking)
        //{
        //    Invoice.IssueDate = DateTime.Now;
        //    Invoice.DueDate = Invoice.IssueDate.AddDays(7);
        //    Invoice.isPaid = false;
        //    Invoice.BookingId = booking.Id;

        //    return Invoice;
        //}

        public static decimal DetermineFullInvoicePrice(Room room, int amountOfDays)
        {
            return room.PricePerNight * amountOfDays;
        }

        //public static decimal DetermineNewTotalPricePrice(Room room int beds, int amountOfDays)
        //{
        //    return room.PricePerNight * amountOfDays;
        //}
    }
}
