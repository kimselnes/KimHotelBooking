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
        public void Run()
        {
            Utility.FullClearConsole();
            Console.WriteLine("Lista på alla Kunder\n");

            var customerService = new CustomerService();
            var customers = customerService.GetAllCustomersList();
            var customerStatus = customers.Select(c => new
            {
                FullStatus = $"Kundnamn: {c.Name}\n" +
                    $"Aktiv: {(c.IsActive == true ? "\u001b[32mJa\u001b[0m" : "\u001b[31mNej\u001b[0m")}"
            }).ToList();

            Console.WriteLine("- - - - - - - - - - - - - - - - -");

            foreach (var customer in customerStatus )
            {
                Console.WriteLine(customer.FullStatus);
                Console.WriteLine("- - - - - - - - - - - - - - - - -");
            }          
        }

        public static void VIP()
        {
            Utility.FullClearConsole();
            Console.WriteLine("Alla kunder som har fler än 2 bokningar totalt\n" +
                "\u001b[32mGe dem VIP-Treatment!\u001b[0m\n\n");

            var vipCustomers = CustomerService.CustomersOver2Bookings().Where(c => c.IsActive == true);

            foreach (var customer in vipCustomers)
            {
                Console.WriteLine($"Kundnamn: {customer.Name}\n");
            }
        }
    }
}
