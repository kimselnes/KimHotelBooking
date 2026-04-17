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
        public string SetName(string name)
        {
            if (name.Length <= 50)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nDu valde namnet {name}!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nNamnet du valde är för långt");
                Console.ResetColor();
            }

            return name;
        }

        public static bool SetIsActive()
        {
            var active = false;

            var setActivePrompt = "Ska kunden vara AKTIV eller INTE";
            List<string> setActiveOptions =
            [
                "Aktiv",
                "INTE aktiv"
            ];

            Menu setActiveMenu = new Menu(setActivePrompt, setActiveOptions);
            int setActiveSelectedIndex = setActiveMenu.Run();

            switch (setActiveSelectedIndex)
            {
                case 0:
                    active = true;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDu har angett 'Aktiv'");
                    Console.ResetColor();
                    break;
                case 1:
                    active = false;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDu har angett 'INTE aktiv'");
                    Console.ResetColor();
                    break;
            }

            return active;
        }

        public List<Customer> GetAllCustomersList()
        {
            var customerRep = new CustomerRepository();
            var customers = customerRep.GetAllCustomers();

            return customers;
        }

        public List<Customer> GetAllActiveCustomersList()
        {
            var customerRep = new CustomerRepository();
            var tempCustomers = customerRep.GetAllCustomers();

            List<Customer> customers = tempCustomers.Where(c => c.IsActive == true).ToList();

            return customers;
        }

        public List<string> CustomerListToStrings()
        {
            List<string> customersStrings = new List<string>();

            var customersList = GetAllCustomersList();

            foreach (var customer in customersList)
            {
                var tempString = $"{customer.Id}. " +
                    $"{customer.Name} " +
                    $"[{(customer.IsActive == true ? "\u001b[32mAKTIV\u001b[0m" : "\u001b[31mOAKTIV\u001b[0m")}]";

                customersStrings.Add(tempString);
            }

            return customersStrings;
        }

        public List<string> CustomerListToStringsOnlyActive()
        {
            List<string> customersStrings = new List<string>();

            var customersList = GetAllCustomersList();

            foreach (var customer in customersList.Where(c => c.IsActive == true))
            {
                var tempString = $"{customer.Id}. " +
                    $"{customer.Name} " +
                    $"[{(customer.IsActive == true ? "\u001b[32mAKTIV\u001b[0m" : "\u001b[31mOAKTIV\u001b[0m")}]";

                customersStrings.Add(tempString);
            }

            return customersStrings;
        }

        public static Customer GetCustomerById(int id)
        {
            var customerRep = new CustomerRepository();
            var customer = customerRep.GetById(id);

            return customer;
        }

        public Customer GetCustomerByName(string name)
        {
            var customerRep = new CustomerRepository();
            var customer = customerRep.GetByName(name);

            return customer;
        }

        public void DeleteById(Customer customer)
        {
            var customerRep = new CustomerRepository();
            customerRep.Delete(customer);
        }

        public static bool CheckIfBooking(Customer customer)
        {
            var bookingRep = new BookingRepository();
            var bookings = bookingRep.GetAllBookings();

            List<Booking> customerBookings = bookings.Where(b => b.CustomerId == customer.Id).ToList();

            if (customerBookings.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static List<Customer> CustomersOver2Bookings()
        {
            var customerService = new CustomerService();
            var customers = customerService.GetAllCustomersList();

            var bookings = BookingService.GetAllBookingsList();

            var customersOver3Bookings = customers
                .Where(c => bookings.Count(b => b.CustomerId == c.Id) > 2)
                .ToList();

            return customersOver3Bookings;
        }
    }
}
