using KimHotelBooking.Models;
using KimHotelBooking.Repositories;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.BookingCRUDs
{
    public class BookingUpdateMenu
    {
        public static Customer Customer { get; set; } = new Customer();
        public static Room Room { get; set; } = new Room();
        public static Booking Booking { get; set; } = new Booking();
        public static Invoice Invoice { get; set; } = new Invoice();
        public static int CorrectBookingId { get; set; }
        public static int OriginalBeds { get; set; }

        public static void Run(Customer customer)
        {
            Customer = customer;

            var prompt = "\u001b[1mÄNDRA BOKNING\u001b[0m\n" +
            "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
            $"{customer.Name}\nVilken bokning vill du ändra?";

            var bookingsToUpdate = BookingService.GetAllIsActiveCheckedOut(customer);
            var bookingsToUpdateOptions = BookingService.BookingsWithListToStringForCancelUpdateBookings(bookingsToUpdate);
            var anyDoubleRooms = BookingService.CheckAnyDoubleRooms(bookingsToUpdate);

            if (bookingsToUpdate.Count == 0 || !anyDoubleRooms)
            {
                Console.WriteLine("\n\u001b[31mDu har inga bokningar att ändra.\u001b[0m");
                Utility.WaitForKeyPress();
                BookingMenu.SelectDeleteOrUpdate();
            }

            Menu menu = new Menu(prompt, bookingsToUpdateOptions);
            int selectedIndex = menu.Run();

            if (selectedIndex == -1)
            {
                BookingMenu.Run();
            }
            else
            {
                CorrectBookingId = bookingsToUpdate[selectedIndex].Id;
                Booking = BookingService.GetByBookingId(CorrectBookingId);
                Room = RoomService.GetRoomById(Booking.RoomId);
                OriginalBeds = Booking.RoomBeds;
                Updating();
            }
        }

        public static void Updating()
        {
            var updatePrompt = "\u001b[1mUPPDATERA BOKNING\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vad vill du ändra?";

            List<string> options =
            [
                $"Sängar: {Booking.RoomBeds}",
                " -SPARA-"
            ];

            Menu menu = new Menu(updatePrompt, options);
            int selectedIndex = menu.Run();

            Console.WriteLine();

            switch (selectedIndex)
            {
                case 0: // Update Beds.
                    ExtraBed();
                    Updating();
                    break;

                case 1: // Save Changes.

                    BookingService.UpdateBookingBeds(Booking);
                    Console.WriteLine("\n\u001b[32mBokningen har uppdaterats\u001b[0m");
                    UpdateInvoicePrice();
                    BookingMenu.Run();
                    break;

                case -1: // Go back.
                    Run(Customer);
                    break;
            }

        }

        public static void UpdateInvoicePrice()
        {
            if (Booking.RoomBeds != OriginalBeds)
            {
                Room.Beds = Booking.RoomBeds;
                var roomService = new RoomService();
                Room.PricePerNight = roomService.DeterminePricePerNight(Room);

                Invoice = InvoiceService.GetInvoiceByBookingId(Booking.Id);
                Invoice.TotalPrice = InvoiceService.DetermineFullInvoicePrice(Room, (Booking.EndDate - Booking.StartDate).Days + 1);

                var invoiceRep = new InvoiceRepository();
                invoiceRep.Update(Invoice);


                if (Booking.RoomBeds > OriginalBeds)
                {
                    var priceIncrease = ((Booking.RoomBeds - OriginalBeds) * 100) * ((Booking.EndDate - Booking.StartDate).Days + 1);
                    Console.WriteLine($"\nFakturan har ökat med {priceIncrease} SEK på grund av extra sängar.");
                    Utility.WaitForKeyPress();
                    Run(Customer);
                }
                else if (Booking.RoomBeds < OriginalBeds)
                {
                    var priceReduction = ((OriginalBeds - Booking.RoomBeds) * 100) * ((Booking.EndDate - Booking.StartDate).Days + 1);
                    Console.WriteLine($"\nFakturan har sänkts med {priceReduction} SEK på grund av färre sängar.");
                    if (Invoice.IsPaid == true)
                    {
                        Console.WriteLine($"Eftersom fakturan redan är betald får du tillbaka {priceReduction} SEK till ditt konto.");
                    }
                    Utility.WaitForKeyPress();
                    Run(Customer);
                }
            }
        }

        public static void ExtraBed()
        {
            if (Room.SquareMeters >= 30 && Room.SquareMeters < 40)
            {
                var bedsPrompt1 = "\u001b[1mUPPDATERA BOKNING\u001b[0m\n" +
                    "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n" +
                    "Vad vill du göra?";

                List<string> bedsOptions1 = [
                    "Lägga till 1 säng = +100 SEK/natt",
                    "Ta bort 1 säng = -100 SEK/natt"];

                Menu bedsMenu1 = new Menu(bedsPrompt1, bedsOptions1);
                int bedsSelectedIndex1 = bedsMenu1.Run();

                switch (bedsSelectedIndex1)
                {
                    case 0: // +1 Beds
                        if (Booking.RoomBeds == 1)
                        {
                            Booking.RoomBeds += 1;
                            Console.WriteLine("\n\u001b[32mDu har lagt till en säng\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        else
                        {
                            Console.WriteLine("\n\u001b[31mDu kan inte lägga till fler sängar i det här rummet\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        break;

                    case 1: // -1 Beds
                        if (Booking.RoomBeds == 2)
                        {
                            Booking.RoomBeds -= 1;
                            Console.WriteLine("\n\u001b[32mDu har tagit bort en säng\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        else
                        {
                            Console.WriteLine("\n\u001b[31mDu kan inte ta bort fler sängar i det här rummet\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        break;

                    case -1:
                        Updating();
                        break;
                }
            }
            else if (Room.SquareMeters >= 40)
            {
                var bedsPrompt2 = "\u001b[1mNY RUMSBOKNING\u001b[0m\n" +
                    "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n" +
                    "Vill du lägga till extra sängar?";

                List<string> bedsOptions2 = [
                    "Lägga till 1 säng = +100 SEK/natt",
                    "Lägga till 2 sängar = +200 SEK/natt",
                    "Ta bort 1 säng = -100 SEK/natt",
                    "Ta bort 2 sängar = -200 SEK/natt"];

                Menu bedsMenu2 = new Menu(bedsPrompt2, bedsOptions2);
                int bedsSelectedIndex2 = bedsMenu2.Run();

                switch (bedsSelectedIndex2)
                {
                    case 0: // +1 Beds
                        if (Booking.RoomBeds == 1 || Booking.RoomBeds == 2)
                        {
                            Booking.RoomBeds += 1;
                            Console.WriteLine("\n\u001b[32mDu har lagt till en säng\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        else
                        {
                            Console.WriteLine("\n\u001b[31mDu kan inte lägga till fler sängar i det här rummet\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        break;

                    case 1: // +2 Beds
                        if (Booking.RoomBeds == 1)
                        {
                            Booking.RoomBeds += 2;
                            Console.WriteLine("\n\u001b[32mDu har lagt till en säng\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        else
                        {
                            Console.WriteLine("\n\u001b[31mDu kan inte lägga till fler sängar i det här rummet\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        break;

                    case 2: // -1 Beds 
                        if (Booking.RoomBeds == 2 || Booking.RoomBeds == 3)
                        {
                            Booking.RoomBeds -= 1;
                            Console.WriteLine("\n\u001b[32mDu har tagit bort en säng\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        else
                        {
                            Console.WriteLine("\n\u001b[31mDu kan inte ta bort fler sängar i det här rummet\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        break;

                    case 3: // -2 Beds
                        if (Booking.RoomBeds == 3)
                        {
                            Booking.RoomBeds -= 2;
                            Console.WriteLine("\n\u001b[32mDu har tagit bort två sängar\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        else
                        {
                            Console.WriteLine("\n\u001b[31mDu kan inte ta bort fler sängar i det här rummet\u001b[0m");
                            Utility.WaitForKeyPress();
                        }
                        break;


                    case -1:
                        Updating();
                        break;

                }
            }
        }                
    }
}
