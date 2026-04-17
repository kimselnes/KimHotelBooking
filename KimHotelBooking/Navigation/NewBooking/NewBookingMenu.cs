using KimHotelBooking.Models;
using KimHotelBooking.Navigation.CustomerCRUDs;
using KimHotelBooking.Repositories;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace KimHotelBooking.Navigation.NewBooking
{
    public class NewBookingMenu
    {
        public static Customer Customer { get; set; } = new Customer();
        public static Booking Booking { get; set; } = new Booking();
        public static Room Room { get; set; } = new Room();
        public static Invoice Invoice { get; set; } = new Invoice();

        public static void Run()
        {
            var customerPrompt = "\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Ange nytt eller existerande kund-konto:";

            List<string> customerOptions =
            [
                $"Skapa nytt kund-konto",
                $"Ange existerande kund-konto"
            ];

            Menu customerMenu = new Menu(customerPrompt, customerOptions);
            int customerSelectedIndex = customerMenu.Run();

            switch (customerSelectedIndex)
            {
                case 0: // Create new Customer
                    CreateNewCustomer();
                    break;

                case 1: // Select Customer
                    Utility.FullClearConsole();
                    SelectCustomer();
                    break;

                case -1: // Exit
                    MainMenu.GuestMenu();
                    break;
            }
        }

        public static void CreateNewCustomer()
        {
            var createPrompt = "\u001b[1mSKAPA KUND\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)";

            List<string> createOptions =
            [
                $"Namn: {Customer.Name}",
                "-[ SPARA NYA KUNDEN ]-"
            ];

            Menu createMenu = new Menu(createPrompt, createOptions);
            int createSelectedIndex = createMenu.Run();

            switch (createSelectedIndex)
            {
                case 0:
                    Console.WriteLine("Välj kundens namn\n");
                    Console.Write("Namn: ");
                    var name = Console.ReadLine();

                    if (name.ToUpper() == "BACK")
                    {
                        Console.WriteLine("\n\u001b[31mBack är inte ett tillåtet namn.\u001b[0m");
                        Utility.WaitForKeyPress();
                        Run();
                    }
                    else if (!string.IsNullOrWhiteSpace(name))
                    {
                        var customerService = new CustomerService();
                        Customer.Name = customerService.SetName(name);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "Du måste ange ett namn eller trycka \u001b[31mESC\u001b[0m.");
                        Console.ResetColor();
                        Utility.WaitForKeyPress();
                    }
                    CreateNewCustomer();
                    break;

                case 1:
                    var customerRep = new CustomerRepository();
                    customerRep.Create(Customer);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Du har sparat nya kunden!");
                    Console.WriteLine("Du måste 'logga in' med nya kontot");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();

                    Run();
                    break;

                case -1:
                    Run();
                    break;
            }
        }

        public static void SelectCustomer()
        {
            Utility.FullClearConsole();
            Console.WriteLine("\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                "(Skriv \u001b[31mBack\u001b[0m för att gå bakåt)\n\n" +
                "Ange ditt kund-konto");

            Console.Write("\nAnge namn: ");
            var nameInput = Console.ReadLine();


            if (!string.IsNullOrWhiteSpace(nameInput))
            {
                var customerService = new CustomerService();
                Customer = customerService.GetCustomerByName(nameInput);

                if (Customer != null)
                {
                    Console.WriteLine($"\nDu har loggat in med \u001b[32m{Customer.Name}.\u001b[0m");
                    Utility.WaitForKeyPress();

                    NewBooking();
                }
                else if (nameInput.ToUpper() == "BACK")
                {
                    Run();
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
                Console.WriteLine("\n\u001b[31mDu har angivit fel namn.\u001b[0m");
                Utility.WaitForKeyPress();
                SelectCustomer();
            }

        }

        public static void NewBooking()
        {
            Utility.FullClearConsole();

            NewStartDate();
            Utility.FullClearConsole();

            NewEndDate();
            Utility.FullClearConsole();

            var availableRooms = BookingService.GetAllAvailableRooms(Booking);
            var availableRoomsOptions = RoomService.RoomsListToStringForBooking(availableRooms);

            var availablePrompt = "\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n" +
                $"Tillgängliga rum under perioden {Booking.StartDate.ToString("yyyy-MM-dd")} - {Booking.EndDate.ToString("yyyy-MM-dd")}";

            Menu availableMenu = new Menu(availablePrompt, availableRoomsOptions);
            int availableSelectedIndex = availableMenu.Run();

            if (availableSelectedIndex == -1)
            {
                NewBooking();
            }
            else
            {
                var correctId = availableRooms[availableSelectedIndex].Id;
                Room = RoomService.GetRoomById(correctId);
            }

            if (Room.Type == "Double")
            {
                Utility.FullClearConsole();
                ExtraBed();
            }

            Booking = BookingService.ConnectBookingWithRoomCustomer(Booking, Room, Customer);

            var bookingRep = new BookingRepository();
            bookingRep.Create(Booking, Room, Customer);

            Booking = BookingService.GetLastInserted();

            var roomRep = new RoomRepository();
            roomRep.AddBooking(Room, Booking);

            // Update Customer with new Booking
            var customerRep = new CustomerRepository();
            customerRep.AddBooking(Customer, Booking);

            // Créate Invoice and connect to Customer
            var roomService = new RoomService();
            Room.PricePerNight = roomService.DeterminePricePerNight(Room);

            Invoice = InvoiceService.SetInvoiceValues(Booking, Room, Customer);

            InvoiceRepository invoiceRep = new InvoiceRepository();
            invoiceRep.Create(Invoice);

            BookingService.CompleteBookingDb(Booking, Room, Customer, Invoice);

            Console.WriteLine("\n\u001b[32mBokningen har slutförts och fakturan har skapats.\u001b[0m");
            Utility.WaitForKeyPress();
            MainMenu.Run();
        }

        public static void NewStartDate()
        {
            Console.WriteLine("\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                "Ange önskat start-datum som du vill boka rum på:");

            Console.Write("\nÅrtal (YYYY): ");

            if (Int32.TryParse(Console.ReadLine(), out var yearInput))
            {
                Console.Write("\nMånad (MM): ");

                if (Int32.TryParse(Console.ReadLine(), out var monthInput))
                {
                    Console.Write("\nDag (DD): ");

                    if (Int32.TryParse(Console.ReadLine(), out var dayInput))
                    {
                        DateTime currentDate = DateTime.Now;

                        try
                        {
                            DateTime startDate = new DateTime(yearInput, monthInput, dayInput);

                            if (startDate.Date >= currentDate.Date)
                            {
                                Booking.StartDate = startDate;
                                Console.WriteLine("\n\u001b[32mDitt start-datum är giltigt.\u001b[0m");
                                Utility.WaitForKeyPress();
                            }
                            else
                            {
                                Console.WriteLine("\n\u001b[31mDatumet måste vara dagens eller framtida.\u001b[0m");
                                Utility.WaitForKeyPress();
                                Utility.FullClearConsole();
                                NewStartDate();
                            }

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("\n\u001b[31mFelaktigt format på datumet.\u001b[0m");
                            Utility.WaitForKeyPress();
                            Utility.FullClearConsole();
                            NewStartDate();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n\u001b[31mAngav fel format på dag.\u001b[0m");
                        Utility.WaitForKeyPress();
                        Utility.FullClearConsole();
                        NewStartDate();
                    }
                }
                else
                {
                    Console.WriteLine("\n\u001b[31mAngav fel format på månad.\u001b[0m");
                    Utility.WaitForKeyPress();
                    Utility.FullClearConsole();
                    NewStartDate();
                }
            }
            else
            {
                Console.WriteLine("\n\u001b[31mAngav fel format på år.\u001b[0m");
                Utility.WaitForKeyPress();
                Utility.FullClearConsole();
                NewStartDate();
            }
        }

        public static void NewEndDate()
        {
            Console.WriteLine("\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                "Ange önskat slut-datum som du vill boka rum på:");

            Console.Write("\nÅrtal (YYYY): ");

            if (Int32.TryParse(Console.ReadLine(), out var yearInput))
            {
                Console.Write("\nMånad (MM): ");

                if (Int32.TryParse(Console.ReadLine(), out var monthInput))
                {
                    Console.Write("\nDag (DD): ");

                    if (Int32.TryParse(Console.ReadLine(), out var dayInput))
                    {

                        try
                        {
                            DateTime endDate = new DateTime(yearInput, monthInput, dayInput);

                            if (endDate >= Booking.StartDate)
                            {
                                Booking.EndDate = endDate;
                                Console.WriteLine("\n\u001b[32mDitt slut-datum är giltigt.\u001b[0m");
                                Utility.WaitForKeyPress();
                            }
                            else
                            {
                                Console.WriteLine("\n\u001b[31mDu måste ange ett slut-datum som kommer efter ditt angivna start-datum\u001b[0m");
                                Utility.WaitForKeyPress();
                                Utility.FullClearConsole();
                                NewEndDate();
                            }
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("\n\u001b[31mFelaktigt format på datumet.\u001b[0m");
                            Utility.WaitForKeyPress();
                            Utility.FullClearConsole();
                            NewEndDate();
                        }

                    }
                    else
                    {
                        Console.WriteLine("\n\u001b[31mAngav fel format på dag.\u001b[0m");
                        Utility.WaitForKeyPress();
                        Utility.FullClearConsole();
                        NewEndDate();
                    }
                }
                else
                {
                    Console.WriteLine("\n\u001b[31mAngav fel format på månad.\u001b[0m");
                    Utility.WaitForKeyPress();
                    Utility.FullClearConsole();
                    NewEndDate();
                }
            }
            else
            {
                Console.WriteLine("\n\u001b[31mAngav fel format på år.\u001b[0m");
                Utility.WaitForKeyPress();
                Utility.FullClearConsole();
                NewEndDate();
            }
        }

        public static void ExtraBed()
        {
            if (Room.SquareMeters >= 30 && Room.SquareMeters < 40)
            {
                var bedsPrompt1 = "\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                    "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n" +
                    "Vill du lägga till extra sängar?";

                List<string> bedsOptions1 = [
                    "Nej, ingen extra säng",
                        "Ja, 1 extra + 100 SEK/natt"];

                Menu bedsMenu1 = new Menu(bedsPrompt1, bedsOptions1);
                int bedsSelectedIndex1 = bedsMenu1.Run();

                switch (bedsSelectedIndex1)
                {
                    case 0: // No extra beds

                        break;

                    case 1: // 1 extra bed
                        Room.Beds = 2;
                        Console.WriteLine("\n\u001b[32mDu har lagt till en säng\u001b[0m");
                        Utility.WaitForKeyPress();
                        break;

                    case -1:
                        NewBooking();
                        break;
                }
            }
            else if (Room.SquareMeters >= 40)
            {
                var bedsPrompt2 = "\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                    "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n" +
                    "Vill du lägga till extra sängar?";

                List<string> bedsOptions2 = [
                    "Nej, ingen extra säng",
                        "Ja, 1 extra + 100 SEK/natt",
                        "Ja, 2 extra + 200 SEK/natt"];

                Menu bedsMenu2 = new Menu(bedsPrompt2, bedsOptions2);
                int bedsSelectedIndex2 = bedsMenu2.Run();

                switch (bedsSelectedIndex2)
                {
                    case 0: // No extra beds

                        break;

                    case 1: // 1 extra bed
                        Room.Beds = 2;
                        Console.WriteLine("\n\u001b[32mDu har lagt till en säng\u001b[0m");
                        Utility.WaitForKeyPress();
                        break;

                    case 2: // 2 extra beds
                        Room.Beds = 3;
                        Console.WriteLine("\n\u001b[32mDu har lagt till två sängar\u001b[0m");
                        Utility.WaitForKeyPress();
                        break;

                    case -1:
                        NewBooking();
                        break;
                }
            }
        }
    }
}
