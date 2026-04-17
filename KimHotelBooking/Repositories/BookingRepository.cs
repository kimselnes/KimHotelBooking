using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Repositories
{
    public class BookingRepository // Vill inte lägga till : IRepository sista dagen ifall det skulle skapa problem.
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
                    RoomBeds = room.Beds,
                    IsCheckedIn = false,
                    IsActive = true
                });

                DbContext.SaveChanges();
            }
        }

        public Booking GetById(int id)
        {
            var booking = new Booking();

            using (DbContext)
            {
                booking = DbContext.Booking.Where(b => b.Id == id).FirstOrDefault();
            }
            return booking;
        }

        public Booking GetLastInserted()
        {
            var booking = new Booking();

            using (DbContext)
            {
                booking = DbContext.Booking.OrderByDescending(b => b.Id).FirstOrDefault();
            }

            return booking;
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

        public void SetIsCheckedIn(int bookingId)
        {
            using (DbContext)
            {
                var bookingToUpdate = DbContext.Booking.FirstOrDefault(b => b.Id == bookingId);

                bookingToUpdate.IsCheckedIn = true;

                DbContext.SaveChanges();
            }
        }

        public void SetIsCheckedOut(int bookingId)
        {
            using (DbContext)
            {
                var bookingToUpdate = DbContext.Booking.FirstOrDefault(b => b.Id == bookingId);

                bookingToUpdate.IsCheckedIn = false;

                DbContext.SaveChanges();
            }
        }

        public void SetNotActive(int bookingId)
        {
            using (DbContext)
            {
                var bookingToUpdate = DbContext.Booking.FirstOrDefault(b => b.Id == bookingId);
                bookingToUpdate.IsActive = false;

                DbContext.SaveChanges();
            }
        }

        public void SetBookingBeds(Booking booking)
        {
            using (DbContext)
            {
                var bookingToUpdate = DbContext.Booking.FirstOrDefault(b => b.Id == booking.Id);
                bookingToUpdate.RoomBeds = booking.RoomBeds;

                DbContext.SaveChanges();
            }
        }
    }
}
