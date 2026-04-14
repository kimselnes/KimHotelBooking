using KimHotelBooking.Models;
using KimHotelBooking.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Service
{
    public class BookingService
    {
        public Room Room { get; set; } = new Room();
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
            RoomRepository roomRep = new RoomRepository();
            roomRep.AddBooking(room, booking);

            // Add Booking and Invoice to Customer
            //CustomerRepository customerRep = new CustomerRepository();
            //customerRep.AddBookingInvoice(customer, booking, invoice);

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
            //booking.Customer = customer;

            //room.Bookings.Add(booking);

            //customer.Bookings.Add(booking);

            return booking;
        }

        public static Booking GetByBookingId(int id)
        {
            var bookingRep = new BookingRepository();
            var booking = bookingRep.GetById(id);

            return booking;
        }

        public static Booking GetLastInserted()
        {
            var bookingRep = new BookingRepository();
            var booking = bookingRep.GetLastInserted();

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
                .Where(room => !bookedRoomsIds.Contains(room.Id) && room.IsActive == true) // existingBooking.IsActive == true lades till senare. Mer Error Testing kan krävas.
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

        public static List<string> BookingsWithListToString(List<Booking> bookingsList)
        {
            List<string> bookingsStrings = new List<string>();

            foreach (var booking in bookingsList)
            {
                var customer = CustomerService.GetCustomerById(booking.CustomerId);

                var tempString = $"{customer.Name} - " +
                    $"({booking.Id}. " +
                    $"{booking.StartDate.ToString("yyyy-MM-dd")} - {booking.EndDate.ToString("yyyy-MM-dd")})";
                //$"[{(customer.IsActive == true ? "\u001b[32mAKTIV\u001b[0m" : "\u001b[31mOAKTIV\u001b[0m")}]";

                bookingsStrings.Add(tempString);
            }

            return bookingsStrings;
        }

        public static List<string> BookingsWithListToStringForCancelUpdateBookings(List<Booking> bookingsList)
        {
            List<string> bookingsInfoStrings = new List<string>();
            var roomsList = RoomService.GetAllRoomsList().Where(r => r.IsActive == true).ToList();
            var invoiceList = InvoiceService.GetAllInvoices().Where(i => i.IsActive == true).ToList();



            foreach (var booking in bookingsList)
            {
                var room = roomsList.FirstOrDefault(r => r.Id == booking.RoomId);
                var invoice = invoiceList.FirstOrDefault(i => i.BookingId == booking.Id);

                var tempString = $"Datum: {booking.StartDate.ToString("yyyy-MM-dd")} - {booking.EndDate.ToString("yyyy-MM-dd")}\n" +
                    $"{room.Type}-rum med {booking.RoomBeds} sängar.\n" +
                    $"Faktura: {invoice.TotalPrice} ({(invoice.IsPaid == true ? "\u001b[32mBetald\u001b[0m" : "\u001b[31mEj betald\u001b[0m")})";

                bookingsInfoStrings.Add(tempString);
            }

            return bookingsInfoStrings;
        }

        public static List<Booking> GetAllIsCheckedIn()
        {
            var bookings = GetAllBookingsList();

            var checkedInBookings = bookings.Where(b => b.IsCheckedIn).ToList();

            return checkedInBookings;
        }

        public static List<string> BookingListToStringsOnlyCheckedOut(List<Booking> bookingsList)
        {
            List<string> bookingsStrings = new List<string>();

            //var bookingsList = GetAllBookingsList();

            foreach (var booking in bookingsList.Where(b => b.IsCheckedIn == false))
            {
                var customer = CustomerService.GetCustomerById(booking.CustomerId);

                var tempString = $"{customer.Name} - " +
                    $"({booking.Id}. " +
                    $"{booking.StartDate.ToString("yyyy-MM-dd")} - {booking.EndDate.ToString("yyyy-MM-dd")})";
                //$"[{(customer.IsActive == true ? "\u001b[32mAKTIV\u001b[0m" : "\u001b[31mOAKTIV\u001b[0m")}]";

                bookingsStrings.Add(tempString);
            }

            return bookingsStrings;
        }

        public static List<Booking> GetAllIsActiveCheckedOut(Customer customer) // And future.
        {
            // HÄMTA ALLA BOKNINGAR SOM ÄR AKTIVA, INTE ÄR CHECKEDIN OCH ÄR NÅGONGÅNG I FRAMTIDEN.
            // Maybe add a TIME condition like : && b.EndDate >= DateTime.Now.Date to the LINQ below?
            var bookingsToDelete = GetAllBookingsList().Where(b => b.IsCheckedIn == false && b.IsActive == true && b.CustomerId == customer.Id).ToList();

            return bookingsToDelete;
        }

        public static void CheckInCustomer(int bookingId)
        {
            var bookingRep = new BookingRepository();
            bookingRep.SetIsCheckedIn(bookingId);
        }

        public static void CheckOutCustomer(int bookingId)
        {
            var bookingRep = new BookingRepository();
            bookingRep.SetIsCheckedOut(bookingId);
        }

        public static void SetNotActive(int bookingId)
        {
            var bookingRep = new BookingRepository();
            bookingRep.SetNotActive(bookingId);
        }

        public static void UpdateBookingBeds(Booking booking)
        {
            var bookingRep = new BookingRepository();
            bookingRep.SetBookingBeds(booking);
        }

        public static bool CheckAnyDoubleRooms(List<Booking> bookingsList)
        {
            var roomsList = RoomService.GetAllRoomsList();
            var anyDoubleRooms = false;
            anyDoubleRooms = bookingsList.Any(b => roomsList.FirstOrDefault(r => r.Id == b.RoomId).Type == "Double");
            return anyDoubleRooms;
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
