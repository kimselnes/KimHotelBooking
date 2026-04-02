using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Service;
using KimHotelBooking.Tools;

namespace KimHotelBooking.Navigation.RoomCRUDs
{
    public class RoomReadMenu
    {
        //public static Room Room { get; set; } = new Room();

        public static void Run()
        {
            var rooms = RoomService.GetAllRoomsList();

            Console.Clear();
            Console.WriteLine("Här är alla rum i hotellet.\n");
            Console.WriteLine("- - - - - - - - - - - - - - -");

            foreach (var room in rooms)
            {
                Console.WriteLine($"{room.Type}-rum\n" +
                    $"Kvadrat: {room.SquareMeters}\n" +
                    $"Sängar: {room.Beds}\n" +
                    $"SEK per natt: {room.PricePerNight}\n" +
                    $"Aktiv: {(room.isActive == 1 ? "\u001b[32mJa\u001b[0m" : "\u001b[31mNej\u001b[0m")}");
                Console.WriteLine("- - - - - - - - - - - - - - -");
            }
        }
    }
}
