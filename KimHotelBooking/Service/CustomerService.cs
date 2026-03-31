using KimHotelBooking.Models;
using KimHotelBooking.Repositories;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Service
{
    public class CustomerService
    {
        //public Customer Customer { get; set; }

        //public CustomerService(Customer customer)
        //{
        //    Customer = customer;
        //}

        //public void Create()
        //{
        //}

        public string SetName()
        {
            //var name = "";

            Console.WriteLine("Välj kundens namn\n");
            Console.Write("Namn: ");
            var name = Console.ReadLine();

            // CHECKA LÄNGD!!!!!

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Du valde namnet {name}!");
            Console.ResetColor();
            Utility.WaitForKeyPress();

            return name;
        }

        public List<Customer> GetAllCustomersList()
        {
            var customerRep = new CustomerRepository();
            var customers = customerRep.GetAllCustomers();

            return customers;
        }
    }
}
