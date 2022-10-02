using URL_Shortener.Models;

namespace URL_Shortener.Services.UsersServices
{
    public interface IUsersCntrollerService
    {
        public Task<User?> GetUserByEmailAsync(string? email);
        public Task<User?> GetUserByIdAsync(int? id);
        public Task<Role?> GetUserRoleByEmailAsync(string? email);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(User? user);
        public bool TheUserTableIsEmpty();

        public bool UserExists(int id);
    }
}
