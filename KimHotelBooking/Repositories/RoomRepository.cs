using KimHotelBooking.Data;
using KimHotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Repositories
{
    public class RoomRepository : IRepository
    {
        public ApplicationDbContext DbContext { get; set; }
        public Build Build { get; set; } = new Build();

        public RoomRepository()
        {
            DbContext = Build.BuildApp();
        }

        public void AddToDatabase(Room room)
        {
            using (DbContext)
            {
                DbContext.Room.Add(new Room
                {
                    Type = room.Type,
                    SquareMeters = room.SquareMeters,
                    Beds = 1,
                    isAvailable = true,
                    isActive = 1,
                    PricePerNight = room.PricePerNight
                });

                DbContext.SaveChanges();
            }
        }

        public void Update(Room newRoom)
        {
            using (DbContext)
            {
                var roomToUpdate = DbContext.Room.FirstOrDefault(r => r.Id == newRoom.Id);

                roomToUpdate.Type = newRoom.Type;
                roomToUpdate.SquareMeters = newRoom.SquareMeters;
                roomToUpdate.Beds = newRoom.Beds;
                roomToUpdate.isActive = newRoom.isActive;

                DbContext.SaveChanges();
            }
        }

        public void AddBooking(Room room, Booking booking)
        {
            using (DbContext)
            {
                var roomToUpdate = DbContext.Room.FirstOrDefault(r => r.Id == room.Id);

                roomToUpdate.Bookings.Add(booking);                

                DbContext.SaveChanges();
            }
        }

        public List<Room> GetAllRooms()
        {
            List<Room> room = new List<Room>();

            using (DbContext)
            {
                room = DbContext.Room.OrderBy(r => r.Id).ToList();
            }

            return room;
        }

        public Room GetById(int id)
        {
            var room = new Room();

            using (DbContext)
            {
                room = DbContext.Room.Where(r => r.Id == id).FirstOrDefault();
            }
            return room;
        }
    }
}
