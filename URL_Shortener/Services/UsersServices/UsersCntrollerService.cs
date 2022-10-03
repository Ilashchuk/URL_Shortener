using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using URL_Shortener.Data;
using URL_Shortener.Models;
using URL_Shortener.Services.AccountServices;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace URL_Shortener.Services.UsersServices
{
    public class UsersCntrollerService : IUsersCntrollerService, IAccountControllerService
    {
        private readonly URL_Shortener_Context _context;
        public UsersCntrollerService(URL_Shortener_Context context)
        {
            _context = context;
        }

        public Task<User?> GetUserByEmailAsync(string? email) => _context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Email == email);
        public Task<User?> GetUserByIdAsync(int? id) => _context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == id);
        public Task<Role?> GetUserRoleByEmailAsync(string? email) => _context.Roles.FirstOrDefaultAsync(r => r.Id == 
                                                        _context.Users.FirstOrDefault(x => x.Email == email).RoleId);
        public Task<User?> GetUserByEmailAndPasswordAsync(string? email, string? password) 
            => _context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        public Task<Role?> GetUserRoleWithUserValueAsync() => _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
        public async Task AddNewUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(User? user)
        {
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
        }
        public bool TheUserTableIsEmpty()
        {
            if (_context.Users == null)
            {
                return true;
            }
            return false;
        }
        public bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
