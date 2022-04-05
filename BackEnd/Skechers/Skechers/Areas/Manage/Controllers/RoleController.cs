using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.Controllers
{
    [Area("manage")]
    public class RoleController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(DataContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View(_context.Roles.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            IdentityRole existRole = await _context.Roles.FirstOrDefaultAsync(x => x.NormalizedName == role.Name.ToUpper());
            if (existRole != null)
            {
                ModelState.AddModelError("", "This role already exists in the database!");
                return View();
            }
            if (role == existRole)
            {
                ModelState.AddModelError("", "This role already exists in the database!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _roleManager.CreateAsync(role);
            return RedirectToAction("index", "role");
        }
        public IActionResult Edit(string id)
        {
            IdentityRole existRole = _context.Roles.FirstOrDefault(x => x.Id == id);
            if (existRole == null)
            {
                return NotFound();
            }

            return View(existRole);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IdentityRole role)
        {
            IdentityRole existRole = await _context.Roles.FirstOrDefaultAsync(x => x.Id == role.Id);
            if (existRole == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {   
                return View();
            }
            if (role == existRole)
            {
                ModelState.AddModelError("", "This role already exists in the database!");
                return View();
            }
            existRole.Name = role.Name;
            _context.SaveChanges();

            return RedirectToAction("index", "role");
        }
        public async Task<ActionResult> Delete(string id)
        {
            IdentityRole existRole = _context.Roles.FirstOrDefault(x => x.Id == id);
            if (existRole == null)
            {
                return NotFound();
            }
            await _roleManager.DeleteAsync(existRole);

            return Ok();
        }
    }
}
