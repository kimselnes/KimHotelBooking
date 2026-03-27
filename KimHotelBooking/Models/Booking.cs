using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KimHotelBooking.Models
{
    public class Booking : IModels
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        //public decimal TotalPrice { get; set; }

        public bool isCheckedIn { get; set; }

        public Customer Customer { get; set; }

    }
}
