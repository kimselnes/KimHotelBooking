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
                DbContext.Invoice.Add(new Invoice
                {
                    IssueDate = invoice.IssueDate,
                    DueDate = invoice.DueDate,
                    TotalPrice = invoice.TotalPrice,
                    isPaid = invoice.isPaid,
                    Booking = invoice.Booking                    
                });

                DbContext.SaveChanges();
            }
        }

        public void AddBooking()
        {

        }
    }
}
