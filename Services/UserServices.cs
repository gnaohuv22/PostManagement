using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{

    public interface IUserServices
    {
        Task<List<AppUser>> GetAll();
        Task<List<AppUser>> GetUsersByEmail(string email);
        Task<AppUser> GetUserById(int id);
        Task<AppUser> GetUserByEmail(string email);
        Task<bool> AddOrEditUser(AppUser user);
        Task<bool> DeleteUser(int id);
    }
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;

        public UserServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrEditUser(AppUser user)
        {
            var entry = await _context.AppUser.FirstOrDefaultAsync(x => x.UserId == user.UserId);
            if (entry == null)
            {
                _context.AppUser.Add(user);
            }
            else
            {
                entry.Fullname = user.Fullname;
                entry.Address = user.Address;
                entry.Email = user.Email;
                entry.Password = user.Password;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _context.AppUser.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var entry = await _context.AppUser.FirstOrDefaultAsync(x => x.UserId == id);
            if (entry != null)
            {
                _context.AppUser.Remove(entry);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<AppUser>> GetAll()
        {
            return await _context.AppUser.AsQueryable().ToListAsync();
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _context.AppUser.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<List<AppUser>> GetUsersByEmail(string email)
        {
            return await _context.AppUser.Where(x => x.Email.Contains(email)).ToListAsync();
        }
    }
}
