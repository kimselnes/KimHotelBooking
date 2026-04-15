using KimHotelBooking.Models;
using KimHotelBooking.Repositories;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace KimHotelBooking.Navigation.CustomerCRUDs
{
    public class CustomerCreateMenu
    {
        public Customer Customer { get; set; } = new Customer();

        public void Run()
        {
            var createPrompt = "\u001b[1mSKAPA KUND\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)";

            List<string> createOptions =
            [
                "Namn",
                "-[ SPARA NYA KUNDEN ]-"
            ];

            Menu createMenu = new Menu(createPrompt, createOptions);
            int createSelectedIndex = createMenu.Run();

            
            var customerMenu = new CustomerMenu();

            switch (createSelectedIndex)
            {
                case 0: // Set Name
                    Console.WriteLine("Välj kundens namn\n");
                    Console.Write("Namn: ");
                    var name = Console.ReadLine();

                    if (name.ToUpper() == "BACK")
                    {
                        Console.WriteLine("\n\u001b[31mBack är inte ett tillåtet namn.\u001b[0m");
                        Utility.WaitForKeyPress();
                        Run();
                    }
                    else if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("\n\u001b[31mNamnet kan inte vara tomt.\u001b[0m");
                        Utility.WaitForKeyPress();
                        Run();
                    }

                    var customerService = new CustomerService();
                    Customer.Name = customerService.SetName(name);

                    Run();
                    break;

                case 1: // Save new Customer
                    var customerRep = new CustomerRepository();
                    customerRep.Create(Customer);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Du har sparat nya kunden!");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();

                    CustomerMenu.Run();
                    //var customerMenu = new CustomerMenu();
                    //customerMenu.Run();
                    break;

                case -1: // Exit                    
                    CustomerMenu.Run();
                    break;
            }
        }
    }
}
