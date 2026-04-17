using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Repositories;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.RoomCRUDs
{
    public class RoomUpdateMenu
    {
        public static Room Room { get; set; } = new Room();

        public static void Run()
        {
            var rooms = RoomService.GetAllRoomsList();
            var updateOptions = RoomService.RoomsListToString();

            var updatePrompt = "\u001b[1mÄndra rum\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)";

            Menu updateMenu = new Menu(updatePrompt, updateOptions);
            int updateSelectedMenuIndex = updateMenu.Run();

            if (updateSelectedMenuIndex == -1)
            {
                RoomMenu.Run();
            }
            else
            {
                var correctId = rooms[updateSelectedMenuIndex].Id;
                Room = RoomService.GetRoomById(correctId);

                Updating();
            }
        }

        public static void Updating()
        {
            var prompt = "\u001b[1mUPPDATERA RUM\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)\n\n" +
                "Vad vill du ändra?";
            List<string> options =
            [
                $"Type: {Room.Type}",
                $"KvadratMeter: {Room.SquareMeters}",
                $"Aktiv: {(Room.IsActive == true ? "\u001b[32mJa\u001b[0m" : "\u001b[31mNej\u001b[0m")}",
                " -SPARA-"
            ];

            Menu menu = new Menu(prompt, options);
            int selectedIndex = menu.Run();

            Console.WriteLine();

            switch (selectedIndex)
            {
                case 0: // Update Type.

                    Room.Type = RoomService.SetRoomType();
                    Updating();

                    /*
                    //var typePrompt = $"Välj den nya Rumstypen.\n" +
                    //    $"Gamla: {room.Type}";

                    //List<string> typeOptions =
                    //[
                    //    $"Single",
                    //    $"Double"
                    //];

                    //Menu typeMenu = new Menu(typePrompt, typeOptions);
                    //int typeSelectedIndex = typeMenu.Run();


                    //switch (typeSelectedIndex)
                    //{
                    //    case 0:
                    //        Room.Type = "Single";
                    //        break;

                    //    case 1:
                    //        Room.Type = "Double";
                    //        break;
                    //}*/

                    break;

                case 1: // Update SquareMeters.

                    Room.SquareMeters = RoomService.SetRoomSquareMeters(Room.SquareMeters);
                    Updating();
                    break;

                case 2: // Update isActive.
                    Room.IsActive = RoomService.SetRoomIsActive();
                    Updating();
                    break;

                case 3: // Save Changes.
                    if (RoomService.CheckCorrectBeds(Room))
                    {
                        var roomService = new RoomService();
                        Room.PricePerNight = roomService.DeterminePricePerNight(Room);

                        var roomRep = new RoomRepository();
                        roomRep.Update(Room);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Du har ändrat rummet!");
                        Console.ResetColor();
                        Utility.WaitForKeyPress();

                        Run();
                    }
                    else
                    {
                        Console.WriteLine("\n\u001b[31mDu angett fel antal sängar!\u001b[0m");
                        Utility.WaitForKeyPress();
                        Updating();
                    }
                    break;

                case -1: // Go back.
                    Run();
                    break;
            }
        }
    }
}
