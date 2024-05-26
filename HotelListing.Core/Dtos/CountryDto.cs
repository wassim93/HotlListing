using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core.Dtos
{
    public class CreateCountryDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "country name too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 3, ErrorMessage = "short country name too long")]
        public string ShortName { get; set; }
    }
    public class CountryDto : CreateCountryDto
    {
        public int Id { get; set; }
        public IList<HotelDto> Hotels { get; set; }


    }

    public class UpdateCountryDto : CreateCountryDto
    {
        public IList<CreateHotelDto> Hotels { get; set; }

    }

}
