using KimHotelBooking.Data;
using KimHotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Repositories
{
    public class CustomerRepository : IRepository
    {
        public ApplicationDbContext DbContext { get; set; }
        public Build Build { get; set; } = new Build();

        public CustomerRepository()
        {
            DbContext = Build.BuildApp();
        }

        public void Create(Customer customer)
        {
            using (DbContext)
            {
                DbContext.Customer.Add(new Customer
                {
                    Name = customer.Name,
                    IsActive = true
                });

                DbContext.SaveChanges();
            }
        }

        public void Update(Customer newCustomer)
        {
            using (DbContext)
            {
                var customerToUpdate = DbContext.Customer.FirstOrDefault(c => c.Id == newCustomer.Id);

                customerToUpdate.Name = newCustomer.Name;
                customerToUpdate.IsActive = newCustomer.IsActive;

                DbContext.SaveChanges();
            }
            //using (DbContext)
            //{
            //    var customerToUpdate = DbContext.Customer.Where(r => r.Id == newCustomer.Id).FirstOrDefault();
            //    customerToUpdate = newCustomer;

            //    DbContext.SaveChanges();
            //}
        }

        public void Delete(Customer customer)
        {
            using (DbContext)
            {
                var customerToDelete = DbContext.Customer.FirstOrDefault(r => r.Id == customer.Id);

                customerToDelete.IsActive = false;

                DbContext.SaveChanges();
            }
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (DbContext)
            {
                customers = DbContext.Customer.OrderBy(x => x.Id).ToList();
            }

            return customers;
        }

        public void AddBooking(Customer customer, Booking booking)
        {
            using (DbContext)
            {
                var customerToUpdate = DbContext.Customer.FirstOrDefault(c => c.Id == customer.Id);
                
                if (customerToUpdate.Bookings == null)
                {
                    customerToUpdate.Bookings = new List<Booking>();
                }
                customerToUpdate.Bookings.Add(booking);

                DbContext.SaveChanges();
            }
        }

        public void AddBookingInvoice(Customer customer, Booking booking, Invoice invoice)
        {
            using (DbContext)
            {
                var customerToUpdate = DbContext.Customer.FirstOrDefault(c => c.Id == customer.Id);

                if (customerToUpdate.Invoices == null)
                {
                    customerToUpdate.Invoices = new List<Invoice>();
                }
                //customerToUpdate.Bookings.Add(booking);
                customerToUpdate.Invoices.Add(invoice);

                DbContext.SaveChanges();
            }
        }

        public Customer GetById(int id)
        {
            var customer = new Customer();

            using (DbContext)
            {
                customer = DbContext.Customer.Where(c => c.Id == id).FirstOrDefault();
            }
            return customer;
        }

        public Customer GetByName(string name)
        {
            var customer = new Customer();

            using (DbContext)
            {
                customer = DbContext.Customer.Where(c => c.Name == name && c.IsActive == true).FirstOrDefault();
            }

            return customer;
        }

    }
}
