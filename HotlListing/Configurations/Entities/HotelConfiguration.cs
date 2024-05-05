using HotlListing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotlListing.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
              new Hotel { Id = 1, Name = "Sandal resort spa", Adress = "adress", CountryId = 1, Rating = 4.5 },
              new Hotel { Id = 2, Name = "Palace", Adress = "Uk", CountryId = 2, Rating = 5 },
              new Hotel { Id = 3, Name = "Sandal resort spa 2", Adress = "BS", CountryId = 1, Rating = 4.5 });
        }
    }
}
