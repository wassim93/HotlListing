using System.ComponentModel.DataAnnotations;

namespace HotlListing.Dtos
{
    public class CreateHotelDto
    {
        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "Hotel name too long")]
        public string Name { get; set; }

        [StringLength(maximumLength: 250, ErrorMessage = "Adress  too long")]
        public string Adress { get; set; }

        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }

        public int CountryId { get; set; }

    }
    public class HotelDto : CreateHotelDto
    {
        public int Id { get; set; }
        public CountryDto Country { get; set; }
    }

    public class UpdateHotelDto : CreateHotelDto
    {


    }
}
