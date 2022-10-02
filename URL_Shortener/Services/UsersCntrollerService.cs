using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Models;

namespace URL_Shortener.Services
{
    public class UsersCntrollerService : IUsersCntrollerService
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
