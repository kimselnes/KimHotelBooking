using KimHotelBooking.Data;
using KimHotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Repositories
{
    public class CustomerRepository
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
                    isActive = 1
                });

                DbContext.SaveChanges();
            }
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using(DbContext)
            {
                customers = DbContext.Customer.Select(x => x).ToList();
            }

            return customers;
        }
    }
}
