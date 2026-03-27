using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KimHotelBooking.Models
{
    public class Invoice : IModels
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal TotalPrice { get; set; }

        public bool isPaid { get; set; }

        public Booking Booking { get; set; }
    }
}
