using KimHotelBooking.Models;
using KimHotelBooking.Navigation.CustomerCRUDs;
using KimHotelBooking.Repositories;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
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

            //var newMenu = new NewBookingMenu();

            switch (customerSelectedIndex)
            {
                case 0: // Create new Customer
                    CreateNewCustomer();
                    break;

                case 1: // Select Customer
                    Console.Clear();
                    SelectCustomer();
                    break;

                case -1: // Exit
                    MainMenu.Run();
                    break;
            }
        }

        public static void CreateNewCustomer()
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

            switch (createSelectedIndex)
            {
                case 0:
                    Console.WriteLine("Välj kundens namn\n");
                    Console.Write("Namn: ");
                    var name = Console.ReadLine();

                    if (name != null)
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




            //var customerCreateMenu = new CustomerCreateMenu();
            //customerCreateMenu.Run();
        }

        public static void SelectCustomer()
        {
            Console.WriteLine("\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n" + // Kan inte använda ESC.
                "Ange ditt kund-konto:");

            Console.Write("Ange namn: ");
            var nameInput = Console.ReadLine();

            if (nameInput != null)
            {
                var customerService = new CustomerService();
                Customer = customerService.GetCustomerByName(nameInput);

                if (Customer != null)
                {
                    Console.WriteLine($"Du har loggat in med \u001b[32m{Customer.Name}.\u001b[0m");
                    Utility.WaitForKeyPress();

                    NewBooking();
                }
                else
                {
                    Console.WriteLine("\u001b[31mDu har angivit fel namn.\u001b[0m");
                    Utility.WaitForKeyPress();

                    SelectCustomer();
                }
            }
            else
            {
                Console.WriteLine("\u001b[31mDu har angivit fel namn.\u001b[0m");
                Utility.WaitForKeyPress();

                SelectCustomer();
            }

        }

        public static void NewBooking()
        {
            Console.Clear();
            // ANGE DATUM
            NewStartDate();
            Console.Clear();

            NewEndDate();
            Console.Clear();

            // KOLLA VILKA RUM SOM ÄR LEDIGA OCH LISTA DEM

            var availableRooms = BookingService.GetAllAvailableRooms(Booking);
            var availableRoomsOptions = RoomService.RoomsListToStringForBooking(availableRooms);

            var availablePrompt = "\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n" + // Kan inte använda ESC.
                $"Tillgängliga rum under perioden {Booking.StartDate} - {Booking.EndDate}";

            Menu availableMenu = new Menu(availablePrompt, availableRoomsOptions);
            int availableSelectedIndex = availableMenu.Run();



            // ANGE VILKET RUM MAN VILL ANVÄNDA
            if (availableSelectedIndex == -1)
            {
                NewBooking();
            }
            else
            {
                // Need to make sure i get the correct Room ID here, because the index of the menu options is not the same as the Room ID.
                var correctId = availableRooms[availableSelectedIndex].Id;
                Room = RoomService.GetRoomById(correctId);
            }
            // OM MAN VÄLJER DUBBELRUM FRÅGAS DET IFALL MAN VILL LÄGGA TILL SÄNGAR
            if (Room.Type == "Double")
            {
                Console.Clear();
                ExtraBed();
            }

            // Booking ska kopplas till Customer och till Room och allt ska läggas in i Databasen.
            Booking = BookingService.ConnectBookingWithRoomCustomer(Booking, Room, Customer);

            var bookingRep = new BookingRepository();
            bookingRep.Create(Booking, Room, Customer);

            // Jag behöver plocka aktuella Booking från databasen och lägga in den
            // i property:n Booking.
            //Booking = BookingService.GetBookingByRoomIdAndBookingDate(Room, Booking.StartDate);
            Booking = BookingService.GetLastInserted();

            // - - - - - - - - - - - - - - - - - - - - - - - 
            // Update Room with new Booking
            var roomRep = new RoomRepository();
            roomRep.AddBooking(Room, Booking);

            // Update Customer with new Booking
            var customerRep = new CustomerRepository();
            customerRep.AddBooking(Customer, Booking);

            // Skapa Invoice som ska kopplas till Customer
            var roomService = new RoomService();
            Room.PricePerNight = roomService.DeterminePricePerNight(Room);

            //int bookingTimeLengthDays = BookingService.TimeDifference(Booking.StartDate, Booking.EndDate);
            //Invoice.TotalPrice = InvoiceService.DetermineFullInvoicePrice(Room, bookingTimeLengthDays);
            //// FÅR INTE MED MIG TOTALPRICE FÖR INVOICE
            //Invoice = InvoiceService.SetDatesPaidBooking(Booking);

            // De 3 kodraderna över kan jag sätta ihop i en och samma metod.
            Invoice = InvoiceService.SetInvoiceValues(Booking, Room);

            // - - - - - - - -  - - - - - - - - - - - - - - 

            InvoiceRepository invoiceRep = new InvoiceRepository();
            invoiceRep.Create(Invoice);

            // MSÅTE GÖRA KLART DATABAS KONNECTIONIAZION
            BookingService.CompleteBookingDb(Booking, Room, Customer, Invoice);
        }

        public static void NewStartDate()
        {
            Console.WriteLine("\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" + // Kan inte använda ESC.
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
                        DateTime startDate = new DateTime(yearInput, monthInput, dayInput);

                        if (startDate.Date >= currentDate.Date)
                        {
                            try
                            {
                                Booking.StartDate = startDate;
                                Console.WriteLine("\n\u001b[32mDitt start-datum är giltigt.\u001b[0m");
                                Utility.WaitForKeyPress();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\n\u001b[31mFelaktigt format på datumet.\u001b[0m");
                                Utility.WaitForKeyPress();
                                Console.Clear();
                                NewStartDate();
                                throw;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n\u001b[31mDatumet måste vara dagens eller framtida.\u001b[0m");
                            Utility.WaitForKeyPress();
                            Console.Clear();
                            NewStartDate();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n\u001b[31mAngav fel format på dag.\u001b[0m");
                        Utility.WaitForKeyPress();
                        Console.Clear();
                        NewStartDate();
                    }
                }
                else
                {
                    Console.WriteLine("\n\u001b[31mAngav fel format på månad.\u001b[0m");
                    Utility.WaitForKeyPress();
                    Console.Clear();
                    NewStartDate();
                }
            }
            else
            {
                Console.WriteLine("\n\u001b[31mAngav fel format på år.\u001b[0m");
                Utility.WaitForKeyPress();
                Console.Clear();
                NewStartDate();
            }
        }

        public static void NewEndDate()
        {
            Console.WriteLine("\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" + // Kan inte använda ESC.
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
                                try
                                {
                                    Booking.EndDate = endDate;
                                    Console.WriteLine("\n\u001b[32mDitt slut-datum är giltigt.\u001b[0m");
                                    Utility.WaitForKeyPress();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\n\u001b[31mFelaktigt format på datumet.\u001b[0m");
                                    Utility.WaitForKeyPress();
                                    Console.Clear();
                                    NewEndDate();
                                    throw;
                                }

                            }
                            else
                            {
                                Console.WriteLine("\n\u001b[31mDu måste ange ett slut-datum som kommer efter ditt angivna start-datum\u001b[0m");
                                Utility.WaitForKeyPress();
                                Console.Clear();
                                NewEndDate();
                            }
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("\n\u001b[31mDu måste ange ett giltigt datum.\u001b[0m");
                            Utility.WaitForKeyPress();
                            Console.Clear();
                            NewStartDate();
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("\n\u001b[31mAngav fel format på dag.\u001b[0m");
                        Utility.WaitForKeyPress();
                        Console.Clear();
                        NewEndDate();
                    }
                }
                else
                {
                    Console.WriteLine("\n\u001b[31mAngav fel format på månad.\u001b[0m");
                    Utility.WaitForKeyPress();
                    Console.Clear();
                    NewEndDate();
                }
            }
            else
            {
                Console.WriteLine("\n\u001b[31mAngav fel format på år.\u001b[0m");
                Utility.WaitForKeyPress();
                Console.Clear();
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
