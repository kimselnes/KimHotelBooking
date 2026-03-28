using KimHotelBooking.Data;
using KimHotelBooking.Tools;

namespace KimHotelBooking.Navigation.RoomCRUDs
{
    public class RoomMenu
    {
        public ApplicationDbContext DbContext { get; set; }

        public RoomMenu(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public void Run()
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
                case 0: // Create room.
                    var roomCreateMenu = new RoomCreateMenu(DbContext);
                    roomCreateMenu.Run();
                    break;

                case 1: // Show rooms.
                    var roomReadMenu = new RoomReadMenu(DbContext);
                    roomReadMenu.ReadAllRooms();
                    break;

                case 2: // Update room.
                    break;

                case 3: // Delete room.
                    break;

                case 4: // Go back.
                    //MainMenu.Run(dbContext);
                    break;
            }
        }
    }
}
