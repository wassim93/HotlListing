using AutoMapper;
using HotlListing.Dtos;
using HotlListing.Models;
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
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly UserManager<ApiUser> _userManager;

        public AccountController(UserManager<ApiUser> userManager, ILogger<AccountController> logger, IMapper mapper, SignInManager<ApiUser> signInManager)
        {
            _signInManager = signInManager;
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

        public async Task<IActionResult> Login([FromBody] LoginUserDtO userDto)
        {
            _logger.LogInformation($"Login attempt for {userDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var res = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password, false, false);
                if (!res.Succeeded)
                {
                    return Unauthorized(userDto);
                }
                return Accepted();


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
                return Problem($"something went wrong in{nameof(Login)}", statusCode: 500);
            }


        }
    }
}
