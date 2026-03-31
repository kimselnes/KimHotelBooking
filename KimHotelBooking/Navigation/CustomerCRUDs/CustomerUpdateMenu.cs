using KimHotelBooking.Models;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
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
                "Vad vill du ändra?";

            List<string> customerUpdateOptions =
            [
                $"Namn: {Customer.Name}",
                $"Aktiv: {(Customer.isActive == 1 ? "Ja" : "Nej")}",
                $"-[ SPARA ÄNDRINGAR ]-"
            ];

            Menu customerUpdateMenu = new Menu(customerUpdatePrompt, customerUpdateOptions);
            int customerUpdateSelectedIndex = customerUpdateMenu.Run();


            switch (customerUpdateSelectedIndex)
            {
                case 0: // Name
                    break;

                case 1: // isActive
                    break;

                case 2: // Save to Database
                    break;

                case -1: // Exit
                    break;
            }
        }
    }
}
