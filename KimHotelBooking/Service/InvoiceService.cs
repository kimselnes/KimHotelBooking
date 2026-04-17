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

        public static List<Invoice> GetAllInvoicesForCustomer(int customerId)
        {
            var invoiceRep = new InvoiceRepository();
            var invoices = invoiceRep.GetAllInvoicesForCustomer(customerId);

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

        public static void SetIsPaidTrue(Invoice invoice)
        {
            invoice.IsPaid = true;

            var invoiceRep = new InvoiceRepository();
            invoiceRep.Update(invoice);
        }

        public static void SetIsActiveFalseByBookingId(int bookingId)
        {
            var invoiceRep = new InvoiceRepository();
            var invoice = invoiceRep.GetInvoiceByBookingId(bookingId);

            var invoiceRep2 = new InvoiceRepository();
            if (invoice != null)
            {
                invoiceRep2.SetNotActive(invoice);
            }
        }

        public static List<string> InvoicesToStringList(List<Invoice> invoices)
        {
            var customerRep = new CustomerRepository();
            var customers = customerRep.GetAllCustomers();

            var roomRep = new RoomRepository();
            var rooms = roomRep.GetAllRooms();

            var bookingRep = new BookingRepository();
            var bookings = bookingRep.GetAllBookings();

            var invoiceCustomerAndRoom = invoices
                .Join(customers, invoice => invoice.CustomerId, customer => customer.Id,
                    (invoice, customer) => new { Invoice = invoice, Customer = customer })
                .Join(bookings, ic => ic.Invoice.BookingId, booking => booking.Id,
                    (ic, booking) => new { ic.Invoice, ic.Customer, Booking = booking })
                .Join(rooms, icb => icb.Booking.RoomId, room => room.Id,
                    (icb, room) => new { icb.Invoice, icb.Customer, icb.Booking, Room = room });

            List<string> invoiceStrings = new List<string>();
            foreach (var item in invoiceCustomerAndRoom)
            {
                invoiceStrings.Add($"Kund: {item.Customer.Name}, Sista betalningsdatum: {item.Invoice.DueDate.ToString("yyyy-MM-dd")}\n" +
                    $"Rum: {item.Room.Id}. '{item.Room.Type}' ({item.Booking.StartDate.ToString("yyyy-MM-dd")} - {item.Booking.EndDate.ToString("yyyy-MM-dd")})\n" +
                    $"Faktura: {item.Invoice.TotalPrice} " +
                    $"[{(item.Invoice.IsPaid == false ? "\u001b[31mEj Betald\u001b[0m" : "\u001b[32mBetald\u001b[0m")}]");
            }
            return invoiceStrings;
        }

        public static decimal DetermineFullInvoicePrice(Room room, int amountOfDays)
        {
            return room.PricePerNight * amountOfDays;
        }

        public static List<Invoice> GetAllPastDue()
        {
            var invoiceRep = new InvoiceRepository();
            var invoices = invoiceRep.GetAll();
            
            var invoicesPastDue = invoices.Where(invoice => invoice.DueDate < DateTime.Now && invoice.IsPaid == false && invoice.IsActive == true).ToList();
            return invoicesPastDue;
        }
    }
}
