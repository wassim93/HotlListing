using AutoMapper;
using HotlListing.Dtos;
using HotlListing.IRespository;
using HotlListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotlListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;
        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {

            var hotels = await _unitOfWork.Hotles.GetAll(null, null, new List<string> { "Country" });
            var results = _mapper.Map<IList<HotelDto>>(hotels);
            return Ok(results);




        }
        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetHotelbyId(int id)
        {

            var hotel = await _unitOfWork.Hotles.Get(x => x.Id == id, new List<string> { "Country" });
            var result = _mapper.Map<HotelDto>(hotel);
            return Ok(result);




        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto hotelDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post attemp for {nameof(CreateHotel)} ");

                return BadRequest(ModelState);
            }


            var hotel = _mapper.Map<Hotel>(hotelDto);
            await _unitOfWork.Hotles.Insert(hotel);
            await _unitOfWork.Save();
            return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);



        }


        [HttpPut]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDto hotelDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid Put attemp for {nameof(UpdateHotel)} ");

                return BadRequest(ModelState);
            }


            var hotel = await _unitOfWork.Hotles.Get(x => x.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid Put attemp for {nameof(UpdateHotel)} ");
                return BadRequest("Submitted data is invalid");

            }
            _mapper.Map(hotelDto, hotel);
            _unitOfWork.Hotles.Update(hotel);
            await _unitOfWork.Save();
            return NoContent();



        }


        [HttpDelete]
        [Authorize(Roles = "admin")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeletHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid Delete attemp for {nameof(DeletHotel)} ");

                return BadRequest();
            }


            var country = await _unitOfWork.Hotles.Get(x => x.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid Delete attemp for {nameof(DeletHotel)} ");
                return BadRequest("Country id is invalid");

            }
            await _unitOfWork.Hotles.Delete(id);
            await _unitOfWork.Save();
            return NoContent();



        }
    }

}
