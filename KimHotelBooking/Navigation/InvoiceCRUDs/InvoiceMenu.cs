using KimHotelBooking.Models;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.InvoiceCRUDs
{
    public class InvoiceMenu
    {
        private static Customer Customer { get; set; } = new Customer();
        public static void Run()
        {
            SelectCustomer();

            PayInvoice();
        }

        public static void SelectCustomer()
        {
            Console.WriteLine("\u001b[1mBETALA FAKTUROR\u001b[0m\n" +
            "(Skriv \u001b[31mBACK\u001b[0m och tryck ENTER för att gå bakåt)\n\n" +
            "Ange kundnamn");

            Console.Write("\nKundnamn: ");
            string customerName = Console.ReadLine();

            if (customerName.ToUpper() == "BACK")
            {
                MainMenu.GuestMenu();
            }
            else if (!String.IsNullOrEmpty(customerName))
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
            else
            {
                Console.WriteLine("\n\u001b[31mFelaktigt namn givet.\u001b[0m");
                Utility.WaitForKeyPress();
                SelectCustomer();
            }
        }

        public static void PayInvoice()
        {
            // Get all invoices for the customer.
            var invoices = InvoiceService.GetAllInvoicesForCustomer(Customer.Id);
            var unpaidInvoices = invoices.Where(i => i.IsPaid == false).ToList();

            if (unpaidInvoices.Count == 0)
            {
                Console.WriteLine("\n\u001b[31mDu har inga fakturor att betala.\u001b[0m");
                Utility.WaitForKeyPress();
                Run();
            }
            else
            {
                // List all invoices and let the user select which one to pay.
                var invoiceOptions = InvoiceService.InvoicesToStringList(unpaidInvoices);

                var prompt = "\u001b[1mBETALA FAKTUROR\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vilken faktura vill du betala?";
                Menu menu = new Menu(prompt, invoiceOptions);
                int selectedIndex = menu.Run();

                if (selectedIndex == -1)
                {
                    Run();
                }
                else
                {
                    var selectedInvoice = unpaidInvoices[selectedIndex];
                    InvoiceService.SetIsPaidTrue(selectedInvoice);
                    Console.WriteLine("\n\u001b[32mFakturan är betald!\u001b[0m");
                    Utility.WaitForKeyPress();
                    PayInvoice();
                }
            }
        }
    }
}
