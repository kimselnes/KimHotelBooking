using KimHotelBooking.Models;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.CustomerCRUDs
{
    public class CustomerDeleteMenu
    {
        public Customer Customer { get; set; }

        public void Run()
        {
            var customerDeletePrompt = "\u001b[1mTA BORT KUNDER\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vilken kund vill du ta bort?";

            var customerService = new CustomerService();


            List<string> customerDeleteOptions = customerService.CustomerListToStringsOnlyActive();

            Menu customerDeleteMenu = new Menu(customerDeletePrompt, customerDeleteOptions);
            int customerDeleteSelectedIndex = customerDeleteMenu.Run();


            if (customerDeleteSelectedIndex == -1)
            {
                CustomerMenu.Run();
            }
            else
            {
                List<Customer> customerActive = customerService.GetAllCustomersList()
                    .Where(c => c.IsActive == true).ToList();

                var correctCustomerToDelete = customerActive.ElementAt(customerDeleteSelectedIndex);
                Customer = CustomerService.GetCustomerById(correctCustomerToDelete.Id);
                //Customer = customerService.GetCustomerById(correctCustomerToDelete.Id);

                if (CustomerService.CheckIfBooking(Customer) == true)
                {
                    Console.WriteLine("Kunden har aktiva bokningar och kan därför inte tas bort.");
                    Utility.WaitForKeyPress();
                    Run();
                }
                else
                {
                    Deleting();
                }

            }
        }

        public void Deleting()
        {
            var customerDeletePrompt = "\u001b[1mTA BORT KUND\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                $"Är du säker på att du vill ta bort {Customer.Name}?";

            List<string> customerDeleteOptions =
            [
                $"Ja",
                $"Nej"
            ];

            Menu customerDeleteMenu = new Menu(customerDeletePrompt, customerDeleteOptions);
            int customerDeleteSelectedIndex = customerDeleteMenu.Run();

            switch (customerDeleteSelectedIndex)
            {
                case 0: // YES, remove.
                    var customerService = new CustomerService();
                    customerService.DeleteById(Customer);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Du har tagit bort {Customer.Name}!");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();

                    CustomerMenu.Run();
                    break;

                case 1: // NO, do not remove.
                    Run();
                    break;

                case -1: // Go back.
                    CustomerMenu.Run();
                    break;
            }
        }
    }
}
