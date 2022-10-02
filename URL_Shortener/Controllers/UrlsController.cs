using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Models;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{
    public class UrlsController : Controller
    {
        private readonly IUrlsControllerService _urlsControllerService;
        private readonly IUsersCntrollerService _usersCntrollerService;
        private readonly IShortenerAlgorithmService _shortenerAlgorithmService;

        public UrlsController(IUrlsControllerService urlsControllerService, 
            IUsersCntrollerService usersCntrollerService,
            IShortenerAlgorithmService shortenerAlgorithmService)
        {
            _urlsControllerService = urlsControllerService;
            _usersCntrollerService = usersCntrollerService;
            _shortenerAlgorithmService = shortenerAlgorithmService;
        }

        // GET: Urls
        public async Task<IActionResult> Index()
        {
            return View(await _urlsControllerService.GetUrlsAsync());
        }

        // GET: Urls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _urlsControllerService.TheUrlsTableIsEmpty())
            {
                return NotFound();
            }

            var url = await _urlsControllerService.GetUrlByIdAsync(id);
            if (url == null)
            {
                return NotFound();
            }
            return View(url);
        }

        // GET: Urls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Urls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Link")] Url url)
        {
            if (ModelState.IsValid && _urlsControllerService.IsUniq(url))
            {
                url.Date = DateTime.Now;

                var user = await _usersCntrollerService.GetUserByEmailAsync(HttpContext.User.Identity.Name);
                url.UserId = user.Id;

                await _urlsControllerService.AddNewUrlAsync(url);
                ///////////
                url.ShortLink = _shutenerAlgorithmService.IdToShortURL(url.Id);
                //////////
                await _urlsControllerService.UpdateUrlAsync(url);

                return RedirectToAction("Details", new { id = url.Id });
            }
            return Problem("Models data is Invalid or URL is olready exist.");
        }

        
        // GET: Urls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _urlsControllerService.TheUrlsTableIsEmpty())
            {
                return NotFound();
            }

            var url = await _urlsControllerService.GetUrlByIdAsync(id);

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
            if (_urlsControllerService.TheUrlsTableIsEmpty())
            {
                return Problem("Entity set 'URL_Shortener_Context.Urls'  is null.");
            }
            var url = await _urlsControllerService.GetUrlByIdAsync(id);
            if (url != null)
            {
                await _urlsControllerService.DeleteUserAsync(url);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ReverceToLongLink(int id)
        {
            var url = await _urlsControllerService.GetUrlByIdAsync(id);
            if (url != null)
            {
                return Redirect(url.Link);
            }
            return Problem("Hyston! We haw a problem!");
        }
    }
}
