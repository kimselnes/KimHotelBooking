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

        public int CustomerId { get; set; }

        public int RoomId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(1, 5)]
        public int RoomBeds { get; set; }

        public bool IsCheckedIn { get; set; }

        public bool IsActive { get; set; }
    }
}
