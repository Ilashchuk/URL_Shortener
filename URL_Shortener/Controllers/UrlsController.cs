using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Models;

namespace URL_Shortener.Controllers
{
    public class UrlsController : Controller
    {
        private readonly URL_Shortener_Context _context;

        public UrlsController(URL_Shortener_Context context)
        {
            _context = context;
        }

        // GET: Urls
        public async Task<IActionResult> Index()
        {
            var uRL_Shortener_Context = _context.Urls.Include(u => u.User);
            return View(await uRL_Shortener_Context.ToListAsync());
        }

        // GET: Urls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Urls == null)
            {
                return NotFound();
            }

            var url = await _context.Urls
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (url == null)
            {
                return NotFound();
            }

            return View(url);
        }

        // GET: Urls/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Urls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Link")] Url url)
        {
            if (ModelState.IsValid)
            {
                url.Date = DateTime.Now;
                User user = _context.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
                url.UserId = user.Id;

                _context.Add(url);
                await _context.SaveChangesAsync();
                ///////////
                url.ShortLink = IdToShortURL(url.Id);
                //////////
                _context.Update(url);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", new { id = url.Id });
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", url.UserId);
            return RedirectToAction("Details", new { id = url.Id });
        }

        // GET: Urls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Urls == null)
            {
                return NotFound();
            }

            var url = await _context.Urls.FindAsync(id);
            if (url == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", url.UserId);
            return View(url);
        }

        // POST: Urls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Link,ShortLink,UserId,Date")] Url url)
        {
            if (id != url.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(url);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrlExists(url.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", url.UserId);
            return View(url);
        }

        // GET: Urls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Urls == null)
            {
                return NotFound();
            }

            var url = await _context.Urls
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (url == null)
            {
                return NotFound();
            }

            return View(url);
        }

        // POST: Urls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Urls == null)
            {
                return Problem("Entity set 'URL_Shortener_Context.Urls'  is null.");
            }
            var url = await _context.Urls.FindAsync(id);
            if (url != null)
            {
                _context.Urls.Remove(url);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UrlExists(int id)
        {
          return _context.Urls.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ReverceToLongLink(int id)
        {
            Url url = await _context.Urls.FirstOrDefaultAsync(u => u.Id == id);
            return Redirect(url.Link);
        }

        static String IdToShortURL(int n)
        {
            // Map to store 62 possible characters
            char[] map = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

            String shorturl = "";

            // Convert given integer id to a base 62 number
            while (n > 0)
            {
                // use above map to store actual character
                // in short url
                shorturl += (map[n % 62]);
                n = n / 62;
            }

            // Reverse shortURL to complete base conversion
            return Reverse(shorturl);

        }

        static String Reverse(String input)
        {
            char[] a = input.ToCharArray();
            int l, r = a.Length - 1;
            for (l = 0; l < r; l++, r--)
            {
                char temp = a[l];
                a[l] = a[r];
                a[r] = temp;
            }
            return String.Join("", a);
        }

        // Function to get integer ID back from a short url
        static int shortURLtoID(String shortURL)
        {
            int id = 0; // initialize result

            // A simple base conversion logic
            for (int i = 0; i < shortURL.Length; i++)
            {
                if ('a' <= shortURL[i] &&
                           shortURL[i] <= 'z')
                    id = id * 62 + shortURL[i] - 'a';
                if ('A' <= shortURL[i] &&
                           shortURL[i] <= 'Z')
                    id = id * 62 + shortURL[i] - 'A' + 26;
                if ('0' <= shortURL[i] &&
                           shortURL[i] <= '9')
                    id = id * 62 + shortURL[i] - '0' + 52;
            }
            return id;
        }
    }
}
