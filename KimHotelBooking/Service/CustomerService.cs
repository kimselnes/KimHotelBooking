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
        //public Customer Customer { get; set; }

        //public CustomerService(Customer customer)
        //{
        //    Customer = customer;
        //}

        //public void Create()
        //{
        //}

        public string SetName(string name)
        {
            // CHECKA DUPLICATE!!!!!
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

        public static int SetIsActive()
        {
            var active = 0;

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
                    active = 1;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDu har angett 'Aktiv'");
                    Console.ResetColor();
                    break;
                case 1:
                    active = 0;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDu har angett 'INTE aktiv'");
                    Console.ResetColor();
                    break;
            }

            return active;
        }

        //public bool CheckDuplicateName()
        //{
        //    var check;
        //    return check;
        //}

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

            List<Customer> customers = tempCustomers.Where(c => c.isActive == 1).ToList();

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
                    $"[{(customer.isActive == 1 ? "\u001b[32mAKTIV\u001b[0m" : "\u001b[31mOAKTIV\u001b[0m")}]";

                customersStrings.Add(tempString);
            }

            return customersStrings;
        }

        public List<string> CustomerListToStringsOnlyActive()
        {
            List<string> customersStrings = new List<string>();

            var customersList = GetAllCustomersList();

            foreach (var customer in customersList.Where(c => c.isActive == 1))
            {
                var tempString = $"{customer.Id}. " +
                    $"{customer.Name} " +
                    $"[{(customer.isActive == 1 ? "\u001b[32mAKTIV\u001b[0m" : "\u001b[31mOAKTIV\u001b[0m")}]";

                customersStrings.Add(tempString);
            }

            return customersStrings;
        }

        public Customer GetCustomerById(int id)
        {
            var customerRep = new CustomerRepository();
            var customer = customerRep.GetById(id);

            return customer;
        }

        public void DeleteById(Customer customer)
        {
            var customerRep = new CustomerRepository();
            customerRep.Delete(customer);
        }
    }
}
