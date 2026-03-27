using KimHotelBooking.Data;
using KimHotelBooking.Navigation.RoomCRUDs;
using KimHotelBooking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation
{
    public static class MainMenu
    {
        public static void Run(ApplicationDbContext dbContext)
        {
            var prompt = "Välkommen till Kim's Hotel";
            
            List<string> options = ["Boka rum",
                "Hantera rum",
                "Hantera kunder",
                "Hantera bokningar",
                "Avsluta",
                "TEST"]; // TA BORT TEST NÄR APPEN ÄR KLAR!!!

            Menu menu = new Menu(prompt, options);
            int selectedIndex = menu.Run();


            switch (selectedIndex)
            {
                case 0:
                    // Book a room.
                    break;
                case 1:
                    // CRUD rooms.
                    RoomMenu.Run(dbContext);
                    break;
                case 2:
                    // CRUD customers.
                    break;
                case 3:
                    // CRUD bookings.
                    break;
                case 4:
                    // Quit app.
                    Environment.Exit(0);
                    break;
                case 5: // TA BORT EFTER ATT APPEN ÄR KLAR!!!
                    // Test
                    //RoomCreateMenu.Run(dbContext);
                    RoomCreateMenu.TEST(dbContext);

                    break;
            }



            Utility.WaitForKeyPress();
        }
    }
}
