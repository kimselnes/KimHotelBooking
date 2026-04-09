using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KimHotelBooking.Models
{
    public class Customer : IModels
    {
        [Key]
        public int Id { get ; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        //[Range(0, 1)]
        public bool IsActive { get; set; } // false = No, true = Yes

        public List<Booking> Bookings { get; set; }

        public List<Invoice> Invoices { get; set; }
    }
}
