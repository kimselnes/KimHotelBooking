using KimHotelBooking.Models;
using KimHotelBooking.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Service
{
    public class BookingService
    {
        public Room Room{ get; set; } = new Room();
        public Customer Customer { get; set; } = new Customer();

        /*//public static List<Room> GetAllAvailableRooms(Booking checkBooking) // Ske i RoomService??
        //{

        //    var allBookings = GetAllBookingsList();
        //    List<Booking> availableRoomsBookings = new List<Booking>();

        //    foreach (var booking in allBookings)
        //    {
        //        if (!IsDateOverlap(checkBooking, booking))
        //        {
        //            availableRoomsBookings.Add(booking);
        //        }
        //    }

        //    var allAvailableRooms = new List<Room>();

        //    var rooms = RoomService.GetAllRoomsList();

        //    foreach (var booking in availableRoomsBookings)
        //    {
        //        var room = rooms.FirstOrDefault(r => r.Id == booking.RoomId); // Jämnför jag endast rum som har bokningar?

        //        if (room != null)
        //        {
        //            allAvailableRooms.Add(room);
        //            allAvailableRooms.OrderBy(r => r.Id);
        //        }
        //    }

        //    // - - - - - - - - - - - - - - - - - - - - - - - - - - 



        //    return allAvailableRooms;
        //}*/

        
        public static void CompleteBookingDb(Booking booking, Room room, Customer customer, Invoice invoice)
        {
            // Add Booking to Room
            RoomRepository roomRep= new RoomRepository();
            roomRep.AddBooking(room, booking);

            // Add Booking and Invoice to Customer
            CustomerRepository customerRep = new CustomerRepository();
            customerRep.AddBookingInvoice(customer, booking, invoice);

            // Add Booking to Invoice
            //InvoiceRepository invoiceRep = new InvoiceRepository();
            //invoiceRep.AddBooking(invoice, booking);

            // Booking får sin Room och Customer tidigare.

        }

        

        public static int TimeDifference(DateTime start, DateTime end)
        {
            var differenceDays = (end - start).Days;
            return differenceDays;
        }

        public static Booking ConnectBookingWithRoomCustomer(Booking booking, Room room, Customer customer)
        {
            booking.CustomerId = customer.Id;
            booking.RoomId = room.Id;
            booking.Customer = customer;

            //room.Bookings.Add(booking);

            //customer.Bookings.Add(booking);

            return booking;
        }

        public static Booking GetBookingByRoomIdAndBookingDate(Room room, DateTime startDate)
        {
            var roomId = room.Id;

            var bookingRep = new BookingRepository();
            var booking = bookingRep.GetBookingByRoomIdAndBookingDate(roomId, startDate);

            return booking;
        }

        public static List<Room> GetAllAvailableRooms(Booking checkBooking) // Ske i RoomService??
        {

            var allRooms = RoomService.GetAllRoomsList();
            var allBookings = GetAllBookingsList();

            var bookedRoomsIds = allBookings
                .Where(existingBooking => IsDateOverlap(checkBooking, existingBooking))
                .Select(existingBooking => existingBooking.RoomId)
                .Distinct()
                .ToList();

            var availableRooms = allRooms
                .Where(room => !bookedRoomsIds.Contains(room.Id))
                .OrderBy(room => room.Id)
                .ToList();

            return availableRooms;
        }

        public static List<Booking> GetAllBookingsList()
        {
            var bookingRep = new BookingRepository();
            var bookings = bookingRep.GetAllBookings();

            return bookings;
        }

        //public static List<string> BookingsListToString()
        //{
        //    List<string> bookingsString = new List<string>();
        //    var bookingsList = GetAllBookingsList();

        //    foreach (var booking in bookingsList)
        //    {
        //        var tempString = $"{booking.Id}. {room.Type}-rum " +
        //            $"({room.SquareMeters} kvadrat - {room.Beds} säng/sängar) " +
        //            $"[{(room.isActive == 1 ? "\u001b[32mAKTIV\u001b[0m" : "\u001b[31mOAKTIV\u001b[0m")}]";

        //        bookingsString.Add(tempString);
        //    }

        //    return bookingsString;
        //}

        //public void DateRange(DateTime start, DateTime end)
        //{
        //    var dateTime = new DateTime();
        //    dateTime.
        //}

        public static bool IsDateOverlap(Booking newBooking, Booking oldBooking)
        {
            if (newBooking.StartDate >= oldBooking.StartDate && newBooking.StartDate <= oldBooking.EndDate)
            {
                return true;
            }
            else if (newBooking.EndDate >= oldBooking.StartDate && newBooking.EndDate <= oldBooking.EndDate)
            {
                return true;
            }
            else if (oldBooking.StartDate >= newBooking.StartDate && oldBooking.StartDate <= newBooking.EndDate)
            {
                return true;
            }
            else if (oldBooking.EndDate >= newBooking.StartDate && oldBooking.EndDate <= newBooking.EndDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
