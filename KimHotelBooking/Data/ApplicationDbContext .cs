using KimHotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace KimHotelBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Room> Room { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Invoice> Invoice { get; set; }


        /// <summary>
        /// Tom konstruktor: Denna tomma konstruktor behövs om du vill använda migrations
        /// (dvs. skapa databasen stegvis baserat på ändringar i datamodellen).
        /// </summary>
        public ApplicationDbContext()
        {
        }

        /// <summary>
        /// Konstruktor med alternativ (options):
        /// Denna konstruktor tar in inställningar som skickas från appens konfiguration,
        /// t.ex. anslutningssträngen.
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        /// <summary>
        /// Metoden `OnConfiguring`: används första gången applikationen körs för att
        /// koppla databasen till rätt server.
        /// Om anslutningssträngen inte redan är inställd, anger vi en direkt här.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(@"Server=.;Database=Invoice12345;Trusted_Connection=True;TrustServerCertificate=true;");
                optionsBuilder.UseSqlServer(@"Server=.;Database=KimHotelBooking;Trusted_Connection=True;TrustServerCertificate=true;");
            }
        }
    }
}
