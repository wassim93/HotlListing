using HotelListing.Core.Dtos;
using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.Core.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApiUser> _userManager;
        private ApiUser _user;

        public AuthManager(IConfiguration configuration, UserManager<ApiUser> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> CreateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = await GetClaims(); // Await the GetClaims method
            var tokensOptions = new JwtSecurityToken(issuer: _configuration.GetSection("Jwt:Issuer").Get<string>(),
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokensOptions);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,_user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        public async Task<bool> ValidateUser(LoginUserDtO userDtO)
        {
            _user = await _userManager.FindByNameAsync(userDtO.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, userDtO.Password));

        }
    }
}
