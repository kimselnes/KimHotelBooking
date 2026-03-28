using KimHotelBooking.Data;
using KimHotelBooking.Tools;

namespace KimHotelBooking.Navigation.RoomCRUDs
{
    public class RoomReadMenu
    {
        //public static Room Room { get; set; } = new Room();
        public ApplicationDbContext DbContext { get; set; }

        public RoomReadMenu(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public void Run()
        {
            //Console.WriteLine("Här är alla rum i hotellet.");

            //foreach (var room in DbContext.Room.Select(r => r))
            //{
            //    Console.WriteLine($"{room.Type} rum\n" +
            //        $"{room.SquareMeters} kvadratmeter\n" +
            //        $"{room.Beds} sängar");
            //    Console.WriteLine("- - - - - - - - - - - - - - -");
            //}

            //Utility.WaitForKeyPress();
        }

        public void ReadAllRooms()
        {
            Console.WriteLine("Här är alla rum i hotellet.");

            foreach (var room in DbContext.Room.Select(r => r))
            {
                Console.WriteLine($"{room.Type}-rum\n" +
                    $"{room.SquareMeters} kvadratmeter\n" +
                    $"{room.Beds} sängar");
                Console.WriteLine("\n- - - - - - - - - - - - - - -\n");
            }
        }
    }
}
