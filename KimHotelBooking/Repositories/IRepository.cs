using KimHotelBooking.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking.Repositories
{
    public interface IRepository
    {
        public ApplicationDbContext DbContext { get; set; }
        public Build Build { get; set; }
    }
}
