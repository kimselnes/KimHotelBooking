using KimHotelBooking.Models;
using KimHotelBooking.Navigation.RoomCRUDs;
using KimHotelBooking.Repositories;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace KimHotelBooking.Navigation.CustomerCRUDs
{
    public class CustomerUpdateMenu
    {
        public Customer Customer { get; set; } = new Customer();

        public void Run()
        {
            var customerUpdatePrompt = "\u001b[1mUPPDATERA KUNDER\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vilken kund vill du uppdatera?";

            var customerService = new CustomerService();
            var customersList = customerService.GetAllCustomersList();
            List<string> customerUpdateOptions = customerService.CustomerListToStrings();

            Menu customerUpdateMenu = new Menu(customerUpdatePrompt, customerUpdateOptions);
            int customerUpdateSelectedIndex = customerUpdateMenu.Run();

            if (customerUpdateSelectedIndex == -1)
            {
                CustomerMenu.Run();
            }
            else
            {
                var correctId = customersList[customerUpdateSelectedIndex].Id;
                Customer = CustomerService.GetCustomerById(correctId);
                Updating();
            }
        }

        public void Updating()
        {
            var customerUpdatePrompt = "\u001b[1mUPPDATERA KUND\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vad vill du ändra?";

            List<string> customerUpdateOptions =
            [
                $"Namn: {Customer.Name}",
                $"Aktiv: {(Customer.IsActive == true ? "\u001b[32mJa\u001b[0m" : "\u001b[31mNej\u001b[0m")}",
                $"-[ SPARA ÄNDRINGAR ]-"
            ];

            Menu customerUpdateMenu = new Menu(customerUpdatePrompt, customerUpdateOptions);
            int customerUpdateSelectedIndex = customerUpdateMenu.Run();


            switch (customerUpdateSelectedIndex)
            {
                case 0: // Name
                    Console.WriteLine($"\nNuvarande namn: {Customer.Name}");
                    Console.Write("Ange nytt namn: ");
                    var nameInput = Console.ReadLine();


                    var customerService = new CustomerService();
                    Customer.Name = customerService.SetName(nameInput);

                    Utility.WaitForKeyPress();
                    Updating();
                    break;

                case 1: // isActive

                    Customer.IsActive = CustomerService.SetIsActive();
                    Utility.WaitForKeyPress();
                    Updating();
                    break;

                case 2: // Save to Database
                    if (CustomerService.CheckIfBooking(Customer) == true && Customer.IsActive == false)
                    {
                        Console.WriteLine("Kunden har aktiva bokningar och kan därför inte ändras till \u001b[31mOAKTIV\u001b[0m");
                        Customer.IsActive = true;
                        Utility.WaitForKeyPress();
                        Updating();
                    }
                    else
                    {
                        var customerRep = new CustomerRepository();
                        customerRep.Update(Customer);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nKunden är uppdaterad!");
                        Console.ResetColor();
                        Utility.WaitForKeyPress();

                        CustomerMenu.Run();
                    }
                    break;

                case -1: // Exit
                    Run();
                    break;
            }
        }
    }
}
