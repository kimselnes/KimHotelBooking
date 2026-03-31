using KimHotelBooking.Data;
using KimHotelBooking.Navigation.CustomerCRUDs;
using KimHotelBooking.Navigation.RoomCRUDs;
using KimHotelBooking.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Navigation
{
    public class MainMenu
    {
        public ApplicationDbContext DbContext { get; set; }


        public MainMenu()
        {            
        }

        public MainMenu(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public void Run()
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
                    var roomMenu = new RoomMenu(DbContext);
                    roomMenu.Run();
                    break;
                case 2:
                    // CRUD customers.
                    //var customerMenu = new CustomerMenu();
                    //customerMenu.Run();
                    CustomerMenu.Run();
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
                    //var roomReadMenu = new RoomReadMenu(DbContext);
                    //roomReadMenu.Run();
                    // - - - - - -  - - - - - - 
                    var roomCreateMenu = new RoomCreateMenu(DbContext);
                    roomCreateMenu.Run();
                    break;
            }



            Utility.WaitForKeyPress();
        }
    }
}
