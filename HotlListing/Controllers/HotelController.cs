﻿using AutoMapper;
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
            try
            {
                var hotels = await _unitOfWork.Hotles.GetAll(null, null, new List<string> { "Country" });
                var results = _mapper.Map<IList<HotelDto>>(hotels);
                return Ok(results);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal server error . Please try again later");
            }

        }
        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetHotelbyId(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotles.Get(x => x.Id == id, new List<string> { "Country" });
                var result = _mapper.Map<HotelDto>(hotel);
                return Ok(result);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotelbyId)}");
                return StatusCode(500, "Internal server error . Please try again later");
            }

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

            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);
                await _unitOfWork.Hotles.Insert(hotel);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something wen wrong for {nameof(CreateHotel)} ");

                return StatusCode(500, "Internal server error");
            }

        }
    }
}
