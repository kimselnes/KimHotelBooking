using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Tools;

namespace KimHotelBooking.Navigation.RoomCRUDs
{
    public class RoomReadMenu
    {
        public static Room Room { get; set; } = new Room();
        public static void Run(ApplicationDbContext context)
        {
            Console.WriteLine("Här är alla rum i hotellet.");

            foreach (var room in context.Room.Select(r => r))
            {
                Console.WriteLine($"{room.Type} rum\n" +
                    $"{room.SquareMeters} kvadratmeter\n" +
                    $"{room.Beds} sängar");
                Console.WriteLine("- - - - - - - - - - - - - - -");
            }

            Utility.WaitForKeyPress();
        }
    }
}
