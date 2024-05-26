using HotelListing.Core.Dtos;

namespace HotelListing.Core.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDtO userDtO);
        Task<string> CreateToken();
    }
}
