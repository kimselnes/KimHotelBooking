using KimHotelBooking.Data;
using KimHotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Repositories
{
    public class BookingRepository
    {
        public ApplicationDbContext DbContext { get; set; }
        public Build Build { get; set; } = new Build();

        public BookingRepository()
        {
            DbContext = Build.BuildApp();
        }


        public void Create(Booking booking, Room room, Customer customer)
        {
            // Add Booking
            using (DbContext)
            {
                DbContext.Booking.Add(new Booking
                {
                    CustomerId = customer.Id,
                    RoomId = room.Id,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    isCheckedIn = false
                    //Customer = customer
                });

                DbContext.SaveChanges();
            }
            // Update Room
            //using (DbContext)
            //{
            //    var roomToUpdate = DbContext.Room.FirstOrDefault(r => r.Id == room.Id);

            //    roomToUpdate.Bookings.Add(booking);

            //    DbContext.SaveChanges();
            //}
            // Update Customer
            //using (DbContext)
            //{
            //    var customerToUpdate = DbContext.Customer.FirstOrDefault(c => c.Id == customer.Id);

            //    customerToUpdate.Bookings.Add(booking);

            //    DbContext.SaveChanges();
            //}
        }

        public Booking GetBookingByRoomIdAndBookingDate(int roomId, DateTime startDate)
        {
            var booking = new Booking();

            using (DbContext)
            {
                booking = DbContext.Booking.Where(b => b.Id == roomId && b.StartDate == startDate).FirstOrDefault();
            }

            return booking;
        }

        public List<Booking> GetAllBookings()
        {
            List<Booking> bookings = new List<Booking>();

            using (DbContext)
            {
                bookings = DbContext.Booking.OrderBy(b => b.Id).ToList();
            }

            return bookings;
        }


    }
}
