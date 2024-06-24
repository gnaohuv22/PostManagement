using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Settings
{
    public interface IAuthentication
    {
        Task<AppUser?> Login(string email, string password);
        Task<bool> Register(AppUser user);
    }
    public class Authentication : IAuthentication
    {
        private readonly IUserServices _userServices;

        public Authentication(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public async Task<AppUser?> Login(string email, string password)
        {
            var entry = await _userServices.GetUserByEmail(email);
            if (entry != null)
            {
                if (entry.Password.Equals(password))
                {
                    return entry;
                }
            }
            return null;
        }

        public async Task<bool> Register(AppUser user)
        {
            var entry = await _userServices.GetUserByEmail(user.Email);
            if (entry == null)
            {
                await _userServices.AddOrEditUser(user);
                return true;
            }
            return false;
        }
    }
}
