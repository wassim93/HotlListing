using AutoMapper;
using HotlListing.Dtos;
using HotlListing.Models;
using HotlListing.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace HotlListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        private readonly UserManager<ApiUser> _userManager;

        public AccountController(UserManager<ApiUser> userManager, ILogger<AccountController> logger, IMapper mapper, IAuthManager authManager)
        {
            _authManager = authManager;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            _logger.LogInformation($"registration attempt for {userDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var res = await _userManager.CreateAsync(user, userDto.Password);
                if (!res.Succeeded)
                {
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userDto.Roles);
                return Accepted("Registartion successful");


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem($"something went wrong in{nameof(Register)}", statusCode: 500);
            }


        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] LoginUserDtO userDTO)
        {
            _logger.LogInformation($"Login Attempt for {userDTO.Email} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _authManager.ValidateUser(userDTO))
                {
                    return Unauthorized();
                }

                return Accepted(new TokenRequest { Token = await _authManager.CreateToken() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");
                return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
            }


        }
    }
}
