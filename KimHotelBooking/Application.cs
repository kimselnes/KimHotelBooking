using KimHotelBooking.Data;
using KimHotelBooking.Navigation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimHotelBooking
{
    public class Application
    {
        public void Run()
        {
            Console.Title = "Kim's Hotel";

            var buildApp = new Build();

            using (var dbContext = buildApp.BuildApp())
            {
                var dataInitiaizer = new DataInitializer();
                dataInitiaizer.MigrateAndSeed(dbContext);
            }

            MainMenu.Run();
        }
    }
}
