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
                    IssueDate = invoice.IssueDate,
                    DueDate = invoice.DueDate,
                    TotalPrice = invoice.TotalPrice,
                    isPaid = invoice.isPaid,
                    BookingId =  invoice.BookingId
                });

                DbContext.SaveChanges();
            }
        }

        public void AddBooking()
        {

        }
    }
}
