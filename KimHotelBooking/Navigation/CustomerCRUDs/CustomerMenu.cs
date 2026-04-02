using KimHotelBooking.Models;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.CustomerCRUDs
{
    public class CustomerMenu // Försöker att inte använda DbContext här.
    {
        //public Customer Customer { get; set; }

        public static void Run()
        {
            var customerPrompt = "\u001b[1mHANTERA KUNDER\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)";

            List<string> customerOptions =
            [
                "Skapa ny kund",
                "Visa alla kunder",
                "Ändra kund",
                "Ta bort kund"
            ];

            Menu customerMenu = new Menu(customerPrompt, customerOptions);
            int customerSelectedIndex = customerMenu.Run();            


            switch (customerSelectedIndex)
            {
                case 0: // Create Customer
                    var customerCreateMenu = new CustomerCreateMenu();
                    customerCreateMenu.Run();
                    break;

                case 1: // Read Customers
                    var customerReadMenu = new CustomerReadMenu();
                    customerReadMenu.Run();

                    Utility.WaitForKeyPress();
                    Run();
                    break;

                case 2: // Update Customer
                    var customerUpdateMenu = new CustomerUpdateMenu();
                    customerUpdateMenu.Run();
                    break;

                case 3: // Delete Customer
                    var customerDeleteMenu = new CustomerDeleteMenu();
                    customerDeleteMenu.Run();
                    break;

                case -1:
                    MainMenu.Run();
                    break;
            }
        }
    }
}
