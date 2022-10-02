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
    public class UsersController : Controller
    {
        private readonly URL_Shortener_Context _context;

        public UsersController(URL_Shortener_Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            User user;
            //id = _context.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name).Id;
            try
            {
                user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == _context.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name).Id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
            }
            if(user != null)
                return View(user);
            return RedirectToAction("Login", "Account");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
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
            Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == _context.Users.FirstOrDefault(
                u => u.Email == HttpContext.User.Identity.Name).RoleId);

            if (userRole != null)
            {
                user.Role = userRole;
                user.RoleId = userRole.Id;
            }
            //user.Role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Users == null)
            {
                return Problem("Entity set 'URL_Shortener_Context.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return _context.Users.Any(e => e.Id == id);
        }
    }
}
