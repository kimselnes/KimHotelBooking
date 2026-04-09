using KimHotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace KimHotelBooking.Data
{
    public class DataInitializer
    {
        public void MigrateAndSeed(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
            SeedCustomers(dbContext);
            SeedRooms(dbContext);
            dbContext.SaveChanges();
        }

        private void SeedCustomers(ApplicationDbContext dbContext)
        {
            // Seed 1
            if (!dbContext.Customer.Any(c => c.Name == "Britt"))
            {
                dbContext.Customer.Add(new Customer
                {
                    Name = "Britt",
                    IsActive = true
                });
            }
            // Seed 2
            if (!dbContext.Customer.Any(c => c.Name == "Marit"))
            {
                dbContext.Customer.Add(new Customer
                {
                    Name = "Marit",
                    IsActive = true                   
                });
            }
            // Seed 3
            if (!dbContext.Customer.Any(c => c.Name == "Bosse"))
            {
                dbContext.Customer.Add(new Customer
                {
                    Name = "Bosse",
                    IsActive = true
                });
            }
            // Seed 4
            if (!dbContext.Customer.Any(c => c.Name == "Karl"))
            {
                dbContext.Customer.Add(new Customer
                {
                    Name = "Karl",
                    IsActive = true
                });
            }
        }

        // OM JAG ÄNDRAR PÅ HUR MAN RÄKNAR UT PricePerNight MÅSTE JAG ÄNDRA I MIN SEED.
        private void SeedRooms(ApplicationDbContext dbContext)
        {
            // Seed 1
            if (!dbContext.Room.Any(
                c => c.Type == "Single" && 
                c.SquareMeters == 20 && 
                c.Beds == 1 &&
                c.PricePerNight == 500m))
            {
                dbContext.Room.Add(new Room
                {
                    Type = "Single",
                    SquareMeters = 20,
                    Beds = 1,
                    PricePerNight = 500m,
                    IsAvailable = true,
                    IsActive = true
                });
            }
            // Seed 2
            if (!dbContext.Room.Any(
                c => c.Type == "Single" &&
                c.SquareMeters == 30 &&
                c.Beds == 1 &&
                c.PricePerNight == 600m))
            {
                dbContext.Room.Add(new Room
                {
                    Type = "Single",
                    SquareMeters = 30,
                    Beds = 1,
                    PricePerNight = 600m,
                    IsAvailable = true,
                    IsActive = true
                });
            }
            // Seed 3
            if (!dbContext.Room.Any(
                c => c.Type == "Double" &&
                c.SquareMeters == 30 &&
                c.Beds == 1 &&
                c.PricePerNight == 700m))
            {
                dbContext.Room.Add(new Room
                {
                    Type = "Double",
                    SquareMeters = 30,
                    Beds = 1,
                    PricePerNight = 700m,
                    IsAvailable = true,
                    IsActive = true
                });
            }
            // Seed 4
            if (!dbContext.Room.Any(
                c => c.Type == "Double" &&
                c.SquareMeters == 40 &&
                c.Beds == 1 &&
                c.PricePerNight == 800m))
            {
                dbContext.Room.Add(new Room
                {
                    Type = "Double",
                    SquareMeters = 40,
                    Beds = 1,
                    PricePerNight = 800m,
                    IsAvailable = true,
                    IsActive = true
                });
            }
        }
    }
}
