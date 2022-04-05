using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skechers.Areas.Manage.ViewModels;
using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.Controllers
{
    [Area("manage")]
    public class GenderController : Controller
    {
        private readonly DataContext _context;
        public GenderController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, bool? selected = null, string? word = null)
        {
            IQueryable<Gender> genderquery = _context.Genders.Where(x=>x.IsDeleted==false).AsQueryable();

            if (selected == false) { genderquery = genderquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { genderquery = genderquery.Where(x => x.IsDeleted == true); }
            if (word != null) { genderquery = genderquery.Where(x => x.Name.Contains(word)); }
            ViewBag.SelectedPage = page;
            return View(PagenatedList<Gender>.Create(genderquery, page, 2));
        }
        public IActionResult Create() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Gender gender)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("error", "errors");
            }

            _context.Genders.Add(gender);
            _context.SaveChanges();

            return RedirectToAction("index", "gender");
        }
        public IActionResult Edit(int id)
        {
            Gender gender = _context.Genders.FirstOrDefault(x => x.Id == id);

            if (gender==null)
            {
                return RedirectToAction("notfound", "errors");
            }

            return View(gender);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Gender gender)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("error", "errors");
            }

            Gender existgender = _context.Genders.FirstOrDefault(x => x.Id == gender.Id);
            if (existgender==null)
            {
                return RedirectToAction("notfound", "errors");
            }

            existgender.Name = gender.Name;

            _context.SaveChanges();

            return RedirectToAction("index", "gender");
        }
        public IActionResult Delete(int id)
        {
            Gender gender = _context.Genders.FirstOrDefault(x => x.Id == id);

            if (gender == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            gender.IsDeleted = true;
            _context.SaveChanges();

            return RedirectToAction("index", "gender");
        }
    }
}
