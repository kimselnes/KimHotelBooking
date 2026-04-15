using KimHotelBooking.Data;
using KimHotelBooking.Tools;

namespace KimHotelBooking.Navigation.RoomCRUDs
{
    public class RoomMenu
    {
        public static void Run()
        {
            var prompt = "\u001b[1mHANTERA RUM\u001b[0m\n" +
                "(Tryck \u001b[31mESC\u001b[0m för att gå bakåt)";

            List<string> roomOptions =
            [
                "Skapa nytt rum",
                "Visa alla rum",
                "Ändra rum",
                "Ta bort rum"
            ];

            Menu roomMenu = new Menu(prompt, roomOptions);
            int selectedIndex = roomMenu.Run();

            switch (selectedIndex)
            {
                case 0: // Create room.
                    RoomCreateMenu.Run();
                    break;

                case 1: // Show rooms.
                    RoomReadMenu.Run();
                    Utility.WaitForKeyPress();
                    MainMenu.GuestMenu();

                    break;

                case 2: // Update room.
                    RoomUpdateMenu.Run();
                    break;

                case 3: // Delete room.
                    break;

                case -1: // Go back.
                    MainMenu.AdminMenu();
                    break;
            }
        }
    }
}
