using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Models;

namespace URL_Shortener.Services.URLsServices
{
    public class UrlsControllerService : IUrlsControllerService
    {
        private readonly URL_Shortener_Context _context;
        public UrlsControllerService(URL_Shortener_Context context)
        {
            _context = context;
        }

        public Task<List<Url>> GetUrlsAsync()
        {
            return _context.Urls.Include(u => u.User).ToListAsync();
        }
        public Task<Url?> GetUrlByIdAsync(int? id) => _context.Urls.Include(u => u.User).FirstOrDefaultAsync(x => x.Id == id);
        public async Task AddNewUrlAsync(Url url)
        {
            _context.Urls.Add(url);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUrlAsync(Url url)
        {
            _context.Entry(url).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public bool IsUniq(Url? url)
        {
            if (url != null && _context.Urls.FirstOrDefault(x => x.Link == url.Link) == null)
            {
                return true;
            }
            return false;
        }
        public async Task DeleteUserAsync(Url? url)
        {
            if (url != null)
            {
                _context.Urls.Remove(url);
            }
            await _context.SaveChangesAsync();
        }

        public bool TheUrlsTableIsEmpty()
        {
            if (_context.Urls == null)
            {
                return true;
            }
            return false;
        }
    }
}
