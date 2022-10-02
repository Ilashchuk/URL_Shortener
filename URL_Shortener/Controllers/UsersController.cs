using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.EntityFrameworkCore; 
using URL_Shortener.Data; 
using URL_Shortener.Models; 
using URL_Shortener.Services.UsersServices; 
 
namespace URL_Shortener.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersCntrollerService _usersControllerService;

        public UsersController(IUsersCntrollerService usersCntrollerService)
        {
            _usersControllerService = usersCntrollerService;
        }

        // GET: Users 
        public async Task<IActionResult> Index()
        {
            var user = await _usersControllerService.GetUserByEmailAsync(HttpContext.User.Identity.Name);

            if (user != null)
                return View(user);
            return RedirectToAction("Login", "Account");
        }

        // GET: Users/Edit/5 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _usersControllerService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5 
        // To protect from overposting attacks, enable the specific properties you want to bind to. 
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Password,RoleId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            Role userRole = await _usersControllerService.GetUserRoleByEmailAsync(HttpContext.User.Identity.Name);

            if (userRole != null)
            {
                user.Role = userRole;
                user.RoleId = userRole.Id;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _usersControllerService.UpdateUserAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_usersControllerService.UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _usersControllerService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_usersControllerService.TheUserTableIsEmpty())
            {
                return Problem("Entity set 'URL_Shortener_Context.Users'  is null.");
            }
            var user = await _usersControllerService.GetUserByIdAsync(id);
            await _usersControllerService.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
