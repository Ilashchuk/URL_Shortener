
using URL_Shortener.Models;

namespace URL_Shortener.Services.URLsServices
{
    public interface IUrlsControllerService
    {
        public Task<List<Url>> GetUrlsAsync();

        public bool TheUrlsTableIsEmpty();
        public Task<Url?> GetUrlByIdAsync(int? id);
        public Task AddNewUrlAsync(Url url);
        public Task UpdateUrlAsync(Url url);
        public bool IsUniq(Url? url);
        public Task DeleteUserAsync(Url? url);
    }
}
