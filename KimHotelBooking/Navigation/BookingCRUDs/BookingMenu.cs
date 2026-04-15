using Azure.Core;
using KimHotelBooking.Models;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.BookingCRUDs
{
    public class BookingMenu
    {
        public static Customer Customer { get; set; } = new Customer();
        public static void Run()
        {
            SelectCustomer();

            SelectDeleteOrUpdate();
        }

        public static void SelectCustomer()
        {
            Console.Clear();
            Console.WriteLine("\u001b[1mAVBOKA RUM/ÄNDRA BOKNING\u001b[0m\n" +
            "(Skriv \u001b[31mBACK\u001b[0m och tryck ENTER för att gå bakåt)\n\n" +
            "Ange kundnamn.");

            Console.Write("Kundnamn: ");
            string customerName = Console.ReadLine();

            if (customerName == "" || customerName == null)
            {
                Console.WriteLine("\u001b[31mDu måste ange ett giltigt kundnamn.\u001b[0m");
                SelectCustomer();
            }
            else if (customerName.ToUpper() == "BACK")
            {
                MainMenu.GuestMenu();
            }
            else
            {
                var customerService = new CustomerService();
                Customer = customerService.GetCustomerByName(customerName);

                if (Customer != null)
                {
                    Console.WriteLine($"\nDu har loggat in med \u001b[32m{Customer.Name}.\u001b[0m");
                    Utility.WaitForKeyPress();
                }
                else
                {
                    Console.WriteLine("\n\u001b[31mDu har angivit fel namn.\u001b[0m");
                    Utility.WaitForKeyPress();

                    SelectCustomer();
                }
            }
        }

        public static void SelectDeleteOrUpdate()
        {
            var prompt = "\u001b[1mAVBOKA RUM/ÄNDRA BOKNING\u001b[0m\n" +
            "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
            "Vad vill du göra?";

            List<string> options =
            [
                "Avboka rum",
                "Ändra bokning"
            ];

            Menu menu = new Menu(prompt, options);
            int selectedIndex = menu.Run();
            switch (selectedIndex)
            {
                case 0:
                    // Cancel booking.
                    BookingDeleteMenu.Run(Customer);
                    break;
                case 1:
                    // Change booking.
                    BookingUpdateMenu.Run(Customer);
                    break;
                case -1:
                    // Back to main menu.
                    MainMenu.GuestMenu();
                    break;
            }
        }
    }
}
