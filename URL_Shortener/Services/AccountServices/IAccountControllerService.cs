using URL_Shortener.Models;

namespace URL_Shortener.Services.AccountServices
{
    public interface IAccountControllerService
    {
        public Task<User?> GetUserByEmailAndPasswordAsync(string? email, string? password);
        public Task<Role?> GetUserRoleWithUserValueAsync();
        public Task AddNewUserAsync(User user);
    }
}
