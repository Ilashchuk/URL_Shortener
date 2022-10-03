using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using URL_Shortener.Services.AboutViewService;

namespace URL_Shortener.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutControllerServise _aboutControllerService;

        public AboutController(IAboutControllerServise aboutControllerServise)
        {
            _aboutControllerService = aboutControllerServise;
        }
        // GET: AboutController
        public async Task<IActionResult> Index()
        {
            ViewBag.Text = await _aboutControllerService.GetTextAsync();
            return View();
        }

        // GET: AboutController/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit()
        {
            string text = await _aboutControllerService.GetTextAsync();
            if (text == null)
            {
                return NotFound();
            }
            ViewBag.Text = text;
            return View();
        }

        // POST: AboutController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public async Task<ActionResult> Edit(string text)
        {

            if (text == null)
            {
                return BadRequest("Must de some text about your algorithm.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _aboutControllerService.ChangeTextAsync(text);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
