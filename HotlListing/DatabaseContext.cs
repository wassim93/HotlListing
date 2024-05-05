using HotlListing.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotlListing
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Jamaica", ShortName = "JM" },
                new Country { Id = 2, Name = "Bahamas", ShortName = "BS" },
                new Country { Id = 3, Name = "United kingdom", ShortName = "UK" });

            modelBuilder.Entity<Hotel>().HasData(
              new Hotel { Id = 1, Name = "Sandal resort spa", Adress = "adress", CountryId = 1, Rating = 4.5 },
              new Hotel { Id = 2, Name = "Palace", Adress = "Uk", CountryId = 2, Rating = 5 },
              new Hotel { Id = 3, Name = "Sandal resort spa 2", Adress = "BS", CountryId = 1, Rating = 4.5 });
        }

    }
}
