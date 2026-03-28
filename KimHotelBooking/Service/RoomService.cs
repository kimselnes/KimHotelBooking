using KimHotelBooking.Data;
using KimHotelBooking.Models;

namespace KimHotelBooking.Service
{
    // Room CRUD
    public class RoomService
    {
        public ApplicationDbContext DbContext { get; set; }
        //public Room Room { get; set; }

        //public RoomService(Room room)
        //{
        //    Room = room;
        //}

        // Create Room
        public RoomService(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public void Create(Room room)
        {
            var price = DeterminePricePerNight(room);

            DbContext.Room.Add(new Room
            {
                Type = room.Type,
                SquareMeters = room.SquareMeters,
                Beds = room.Beds,
                isAvailable = true,
                isActive = 1,
                PricePerNight = price
            });

            DbContext.SaveChanges();
        }

        public decimal DeterminePricePerNight(Room room)
        {
            var pricePerNight = 0m;

            // Price accoring to Single or Double room.
            if (room.Type == "Single")
            {
                pricePerNight += 200;
            }
            else
            {
                pricePerNight += 300;
            }

            // Price according to roomsize.
            pricePerNight += room.SquareMeters * 10;

            // Price according to number of beds.
            pricePerNight += room.Beds * 100;

            return pricePerNight;
        }
    }
}
