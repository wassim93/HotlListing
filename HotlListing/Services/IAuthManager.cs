using HotlListing.Dtos;

namespace HotlListing.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDtO userDtO);
        Task<string> CreateToken();
    }
}
