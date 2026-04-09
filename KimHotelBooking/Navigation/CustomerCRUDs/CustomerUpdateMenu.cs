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
            List<string> customerUpdateOptions = customerService.CustomerListToStrings();

            Menu customerUpdateMenu = new Menu(customerUpdatePrompt, customerUpdateOptions);
            int customerUpdateSelectedIndex = customerUpdateMenu.Run();


            //var customersList = customerService.GetAllCustomersList();
            ////var customerToUpdate = customersList.Where(c => c.Id == customerUpdateSelectedIndex);
            //Customer = customersList.Where(c => c.Id == customerUpdateSelectedIndex).FirstOrDefault();

            if (customerUpdateSelectedIndex == -1)
            {
                //var customerMenu = new CustomerMenu();
                //customerMenu.Run();
                CustomerMenu.Run();
            }
            else
            {
                Customer = customerService.GetCustomerById(customerUpdateSelectedIndex + 1);
                Updating();
            }
        }

        public void Updating()
        {
            //var oldName = Customer.Name;

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
                    var customerRep = new CustomerRepository();
                    customerRep.Update(Customer);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nKunden är uppdaterad!");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();

                    CustomerMenu.Run();
                    //var customerMenu = new CustomerMenu();
                    //customerMenu.Run();
                    break;

                case -1: // Exit
                    //customerMenu.Run();
                    Run();
                    break;
            }
        }
    }
}
