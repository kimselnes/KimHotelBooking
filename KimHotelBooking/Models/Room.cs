using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KimHotelBooking.Models
{
    public class Room : IModels
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } // "Single" or "Double".
        
        public int SquareMeters { get; set; } // 20-40 m2.

        [Range(1, 5)]
        public int Beds { get; set; } // 1-3 beds.

        public decimal PricePerNight { get; set; } // Single = 200, Double = 300, 10/m2, 100/Beds.

        public bool isAvailable { get; set; } // false = No, true = Yes.

        [Range(0,1)]
        public int isActive { get; set; } // 0 = No, 1 = Yes

        public List<Booking> Bookings { get; set; }
    }
}
