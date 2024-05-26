using AutoMapper;
using HotelListing.Core.Dtos;
using HotelListing.Core.IRespository;
using HotelListing.Core.Models;
using HotelListing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotlListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        /*        [Authorize]
        */
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {

            var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
            var results = _mapper.Map<IList<CountryDto>>(countries);
            return Ok(results);


        }

        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {

            var country = await _unitOfWork.Countries.Get(x => x.Id == id, new List<string> { "Hotels" });
            var res = _mapper.Map<CountryDto>(country);
            return Ok(res);

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post attemp for {nameof(CreateCountry)} ");

                return BadRequest(ModelState);
            }


            var country = _mapper.Map<Country>(countryDto);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();
            return CreatedAtRoute("GetCountry", new { id = country.Id }, country);




        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDto countryDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid Put attemp for {nameof(UpdateCountry)} ");

                return BadRequest(ModelState);
            }


            var country = await _unitOfWork.Countries.Get(x => x.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid Put attemp for {nameof(UpdateCountry)} ");
                return BadRequest("Submitted data is invalid");

            }
            _mapper.Map(countryDto, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();
            return NoContent();



        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeletCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid Delete attemp for {nameof(DeletCountry)} ");

                return BadRequest();
            }


            var country = await _unitOfWork.Countries.Get(x => x.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid Delete attemp for {nameof(DeletCountry)} ");
                return BadRequest("Country id is invalid");

            }
            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();
            return NoContent();




        }
    }
}
