using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KimHotelBooking.Navigation.RoomCRUDs
{
    public class RoomCreateMenu
    {
        public static Room Room { get; set; } = new Room();

        public static void Run()
        {
            var createPrompt = "\u001b[1mSKAPA NY KUND\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Ange ny kund:";

            List<string> createOptions =
            [
                "Single eller Double",
                "Kvadratmeter",
                "SPARA NYTT RUM"
            ];

            Menu createMenu = new Menu(createPrompt, createOptions);
            int createSelectedIndex = createMenu.Run();

            switch (createSelectedIndex)
            {
                case 0: // Type
                    Console.Clear();
                    //var newType = SetType();
                    //newRoom.Type = newType;
                    // - - - - - - - - - - - -
                    //newRoom.Type = SetType();
                    // - - - - - - - - - - - -
                    Room.Type = RoomService.SetRoomType();
                    Run();
                    break;

                case 1: // SquareMeter
                    Console.Clear();

                    //newRoom.SquareMeters = SetSquareMeters();
                    // - - - - - - - - - - - -
                    Room.SquareMeters = RoomService.SetRoomSquareMeters();

                    Run();
                    break;

                case 2: // Save changes.

                    if (RoomService.CheckCorrectBeds(Room))
                    {
                        var roomService = new RoomService();
                        roomService.Create(Room);

                        Console.WriteLine("\n\u001b[32mDu har lagt till ett nytt rum!\u001b[0m");
                        Utility.WaitForKeyPress();

                        RoomMenu.Run();
                    }
                    else
                    {
                        Console.WriteLine("\n\u001b[31mDu angett fel antal sängar!\u001b[0m");
                        Utility.WaitForKeyPress();
                        Run();
                    }

                    break;

                case -1: // Go back.
                    RoomMenu.Run();
                    break;
            }

            // Visa values som man angett ovan
        }

        //public static string SetType()
        //{
        //    var type = "";

        //    var setTypePrompt = "Ska rummet vara Single eller Double";
        //    List<string> setTypeOptions =
        //    [
        //        "Single",
        //        "Double"
        //    ];

        //    Menu setTypeMenu = new Menu(setTypePrompt, setTypeOptions);
        //    int setTypeSelectedIndex = setTypeMenu.Run();

        //    switch (setTypeSelectedIndex)
        //    {
        //        case 0:
        //            type = "Single";

        //            Console.ForegroundColor = ConsoleColor.Green;
        //            Console.WriteLine($"\nDu har anget {type}");
        //            Console.ResetColor();
        //            Utility.WaitForKeyPress();
        //            break;
        //        case 1:
        //            type = "Double";
        //            Console.ForegroundColor = ConsoleColor.Green;
        //            Console.WriteLine($"\nDu har anget {type}");
        //            Console.ResetColor();
        //            Utility.WaitForKeyPress();
        //            break;
        //    }

        //    return type;
        //}

        //public static int SetSquareMeters()
        //{
        //    Console.WriteLine("Välj hur många kvadratmeter rummer är (20-40)m2\n");
        //    Console.Write("Kvadratmeter: ");

        //    if (!Int32.TryParse(Console.ReadLine(), out var squareMeters))
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine("\nDu angav inte ett heltal");
        //        Console.ResetColor();
        //        Utility.WaitForKeyPress();
        //    }
        //    else
        //    {
        //        if (squareMeters < 20 || squareMeters > 40)
        //        {
        //            Console.WriteLine(
        //                "\nKvadratmeter kan bara vara mellan 20 och 40");
        //            Console.ResetColor();
        //            Utility.WaitForKeyPress();
        //        }
        //        else
        //        {
        //            Console.ForegroundColor = ConsoleColor.Green;
        //            Console.WriteLine($"\nDu har anget {squareMeters} kvadratmeter");
        //            Console.ResetColor();
        //            Utility.WaitForKeyPress();
        //        }
        //    }

        //    return squareMeters;
        //}
        //public static int SetBeds() // Måste se till att Single inte kan ange 2 eller 3.
        //{
        //    var beds = 0;

        //    var setBedPrompt = "Hur många sängar ska rummet ha (Single '1'), (Double '1-3')";
        //    List<string> setBedsOptions =
        //    [
        //        "1",
        //        "2",
        //        "3"
        //    ];

        //    Menu setBedsMenu = new Menu(setBedPrompt, setBedsOptions);
        //    int setBedSelectedIndex = setBedsMenu.Run();

        //    switch (setBedSelectedIndex)
        //    {
        //        case 0:
        //            beds = 1;

        //            Console.ForegroundColor = ConsoleColor.Green;
        //            Console.WriteLine($"\nDu har anget {beds}");
        //            Console.ResetColor();
        //            Utility.WaitForKeyPress();
        //            break;

        //        case 1:
        //            beds = 2;

        //            Console.ForegroundColor = ConsoleColor.Green;
        //            Console.WriteLine($"\nDu har anget {beds}");
        //            Console.ResetColor();
        //            Utility.WaitForKeyPress();
        //            break;

        //        case 2:
        //            beds = 3;

        //            Console.ForegroundColor = ConsoleColor.Green;
        //            Console.WriteLine($"\nDu har anget {beds}");
        //            Console.ResetColor();
        //            Utility.WaitForKeyPress();
        //            break;
        //    }

        //    return beds;
        //}

        //public static void CheckCorrectBedsType(Room room)
        //{
        //    if (room.Type == "Single" && room.Beds > 1)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine($"\nDu kan inte ha mer än 1 säng i 'Single' rum.");
        //        Console.ResetColor();
        //        Utility.WaitForKeyPress();
        //    }
        //}

        //public static void CheckCorrectBedsSquareMeters(Room room)
        //{
        //    if (room.SquareMeters < 40 && room.Beds == 3)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine($"\nDu kan inte ha mer än 2 sängar under 40 m2.");
        //        Console.ResetColor();
        //        Utility.WaitForKeyPress();
        //    }
        //    else if (room.SquareMeters < 30 && room.Beds == 2)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine($"\nDu kan inte ha mer än 1 säng under 30 m2.");
        //        Console.ResetColor();
        //        Utility.WaitForKeyPress();
        //    }
        //}

        public void TEST()
        {
            Room.Type = "Single";
            Room.SquareMeters = 22;
            Room.Beds = 1;

            Run();
        }
    }
}
