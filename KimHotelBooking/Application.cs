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
            /*
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();


            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);

            using (var dbContext = new ApplicationDbContext(options.Options))
            {
                dbContext.Database.Migrate();
            }
            */

            Console.Title = "Kim's Hotel";

            var buildApp = new Build();

            using var dbContext = buildApp.BuildApp();

            using (dbContext)
            {
                var dataInitiaizer = new DataInitializer();
                dataInitiaizer.MigrateAndSeed(dbContext);
            }


            MainMenu.Run(dbContext);



            Console.WriteLine("Klart!");
            Console.ReadLine();
        }
    }
}
