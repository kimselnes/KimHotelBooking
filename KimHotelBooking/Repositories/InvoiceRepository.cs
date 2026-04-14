using KimHotelBooking.Data;
using KimHotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Repositories
{
    public class InvoiceRepository : IRepository
    {
        public ApplicationDbContext DbContext { get; set; }
        public Build Build { get; set; } = new Build();

        public InvoiceRepository()
        {
            DbContext = Build.BuildApp();
        }

        public void Create(Invoice invoice)
        {
            using (DbContext)
            {
                // COPILOT KOD, VET INTE RIKTIGT VARFÖR ELLER VAD DEN GÖR.
                // mark existing booking as unchanged (do not insert)
                //DbContext.Booking.Attach(invoice.Booking);
                // - - - - - -  - - - - - - - - - -- - - - - - -- - - - - -
                DbContext.Invoice.Add(new Invoice
                {
                    CustomerId = invoice.CustomerId,
                    IssueDate = invoice.IssueDate,
                    DueDate = invoice.DueDate,
                    TotalPrice = invoice.TotalPrice,
                    IsPaid = invoice.IsPaid,
                    BookingId =  invoice.BookingId,
                    IsActive = true
                });

                DbContext.SaveChanges();
            }
        }

        public void Update(Invoice newInvoice)
        {
            using (DbContext)
            {
                var invoiceToUpdate = DbContext.Invoice.Where(i => i.Id == newInvoice.Id).FirstOrDefault();

                invoiceToUpdate.IssueDate = newInvoice.IssueDate; // invoiceToUpdate was null.
                invoiceToUpdate.DueDate = newInvoice.DueDate;
                invoiceToUpdate.TotalPrice = newInvoice.TotalPrice;
                invoiceToUpdate.IsPaid = newInvoice.IsPaid;

                DbContext.SaveChanges();
            }
        }

        public List<Invoice> GetAll()
        {

            List<Invoice> invoices = new List<Invoice>();

            using (DbContext)
            {
                invoices = DbContext.Invoice.OrderBy(x => x.Id).ToList();
            }

            return invoices;
        }

        public void SetNotActive(Invoice invoice)
        {
            using (DbContext)
            {
                var invoiceToDelete = DbContext.Invoice.FirstOrDefault(i => i.Id == invoice.Id);

                invoiceToDelete.IsActive = false;

                DbContext.SaveChanges();
            }
        }

        public Invoice GetInvoiceByBookingId(int bookingId)
        {
            var invoice = new Invoice();

            using (DbContext)
            {
                invoice = DbContext.Invoice.Where(i => i.BookingId == bookingId).FirstOrDefault();
            }
            return invoice;
        }

        public void AddBooking()
        {

        }
    }
}
