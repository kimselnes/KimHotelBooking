using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KimHotelBooking.Models
{
    public interface IModels
    {
        [Key]
        public int Id { get; set; }
    }
}
