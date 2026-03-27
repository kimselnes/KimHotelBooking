using Azure.Core;
using KimHotelBooking.Data;
using KimHotelBooking.Tools;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation.RoomCRUDs
{
    public static class RoomMenu
    {
        public static void Run(ApplicationDbContext dbContext)
        {
            var prompt = "Hantera rum";

            List<string> roomOptions =
            [
                "Skapa nytt rum",
                "Visa alla rum",
                "Ändra rum",
                "Ta bort rum",
                "Gå tillbaka"
            ];

            Menu roomMenu = new Menu(prompt, roomOptions);
            int selectedIndex = roomMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    // Create room.
                    RoomCreateMenu.Run(dbContext);
                    break;
                case 1:
                    RoomReadMenu.Run(dbContext);
                    // Show rooms.
                    break;
                case 2:
                    // Update room.
                    break;
                case 3:
                    // Delete room.
                    break;
                case 4:
                    // Go back.
                    MainMenu.Run(dbContext);
                    break;
            }


        }
    }
}
