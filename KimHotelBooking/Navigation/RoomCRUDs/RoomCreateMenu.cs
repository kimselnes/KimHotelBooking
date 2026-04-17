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
            var createPrompt = "\u001b[1mSKAPA NYTT RUM\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Ange ny kund:";

            List<string> createOptions =
            [
                $"Single eller Double: {Room.Type}",
                $"Kvadratmeter: {Room.SquareMeters}",
                "-[SPARA NYTT RUM]-"
            ];

            Menu createMenu = new Menu(createPrompt, createOptions);
            int createSelectedIndex = createMenu.Run();

            switch (createSelectedIndex)
            {
                case 0: // Type
                    Console.Clear();

                    Room.Type = RoomService.SetRoomType();
                    Run();
                    break;

                case 1: // SquareMeter
                    Console.Clear();

                    Room.SquareMeters = RoomService.SetRoomSquareMeters(Room.SquareMeters);

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
        }
    }
}
