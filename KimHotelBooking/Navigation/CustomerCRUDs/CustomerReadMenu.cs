using KimHotelBooking.Models;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.CustomerCRUDs
{
    public class CustomerReadMenu
    {
        //public List<Customer> Customers { get; set; } = new List<Customer>();

        //public Customer Customer { get; set; } = new Customer();

        public void Run()
        {
            Console.WriteLine("Lista på alla Kunder\n");

            var customerService = new CustomerService();
            var customers = customerService.GetAllCustomersList();

            Console.WriteLine("- - - - - - - - - - - - - - - - -");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Kundnamn: {customer.Name}\n" +
                    $"Aktiv: {(customer.IsActive == true ? "Ja" : "Nej")}");
                Console.WriteLine("- - - - - - - - - - - - - - - - -");
            }            
        }
    }
}
