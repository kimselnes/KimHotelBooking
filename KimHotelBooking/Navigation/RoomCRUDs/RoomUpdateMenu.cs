using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.RoomCRUDs
{
    public class RoomUpdateMenu
    {
        public ApplicationDbContext DbContext { get; set; }
        public static Room Room { get; set; } = new Room();

        public RoomUpdateMenu(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public void Run()
        {
            List<Room> roomsList = AllRoomsList();
            var updateOptions = RoomsListToString(roomsList);
            //updateOptions.Insert(0, "GÅ TILLBAKA UTAN ATT SPARA");

            var updatePrompt = "\u001b[1mÄndra rum\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)";

            Menu updateMenu = new Menu(updatePrompt, updateOptions);
            int updateSelectedMenuIndex = updateMenu.Run();

            if (updateSelectedMenuIndex == -1)
            {
                var roomMenu = new RoomMenu(DbContext);
                roomMenu.Run();
            }
            else
            {
                //var roomToUpdate = DbContext.Room.ElementAt(updateSelectedMenuIndex);
                Room = DbContext.Room.ElementAt(updateSelectedMenuIndex);

                Updating();
            }
        }

        public void Updating()
        {
            var prompt = "Vad vill du ändra";
            List<string> options =
            [
                $"Type: {Room.Type}",
                $"KvadratMeter: {Room.SquareMeters}",
                $"Sängar: {Room.Beds}",
                $"Aktiv: {(Room.isActive == 1 ? "Ja" : "Nej")}",
                " -SPARA-",
                " -GÅ TILLBAKA-"
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

                    Room.SquareMeters = RoomService.SetRoomSquareMeters();
                    Updating();
                    break;

                case 2: // Update Beds.
                    Room.Beds = RoomService.SetRoomBeds();
                    Updating();

                    break;

                case 3: // Update isActive.
                    Room.isActive = RoomService.SetRoomIsActive();
                    Updating();

                    break;

                case 4: // Save Changes.

                    //RoomService.CheckCorrectBeds(Room);
                    if (RoomService.CheckCorrectBeds(Room))
                    {
                        var updateRoom = new RoomService(DbContext);
                        updateRoom.Update(Room);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Du har ändrat rummet!");
                        Console.ResetColor();
                        Utility.WaitForKeyPress();

                        Updating();
                    }
                    else
                    {
                        Updating();
                    }
                    break;

                case 5: // Go back.
                    Run();
                    break;
            }
        }

        public List<Room> AllRoomsList()
        {
            List<Room> rooms = new List<Room>(DbContext.Room.Select(r => r));

            //foreach (var room in DbContext.Room.Select(r => r))
            //{
            //    rooms.Add(room);
            //}

            return rooms;
        }

        public List<string> RoomsListToString(List<Room> roomsList)
        {
            List<string> roomsString = new List<string>();
            foreach (var room in roomsList)
            {
                var tempString = $"{room.Id}. {room.Type}-rum " +
                    $"({room.SquareMeters} kvadrat - {room.Beds} säng/sängar) " +
                    $"[{(room.isActive == 1 ? "AKTIV" : "OAKTIV")}]";

                roomsString.Add(tempString);
            }

            return roomsString;
        }


    }
}
