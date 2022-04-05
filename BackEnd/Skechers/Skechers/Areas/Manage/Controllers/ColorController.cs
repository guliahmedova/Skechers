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
    public class ColorController : Controller
    {
        private readonly DataContext _context;
        public ColorController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, bool? selected = null, string? word = null)
        {
            IQueryable<Color> colorquery = _context.Color.Where(x=>x.IsDeleted==false).AsQueryable();

            if (selected == false) { colorquery = colorquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { colorquery = colorquery.Where(x => x.IsDeleted == true); }
            if (word != null) { colorquery = colorquery.Where(x => x.Name.Contains(word)); }

            ViewBag.SelectedPage = page;

            return View(PagenatedList<Color>.Create(colorquery, page, 5));
        }
        public IActionResult Create() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Color color)
        {
            if (!ModelState.IsValid) { return RedirectToAction("error", "errors"); }
            _context.Color.Add(color);
            _context.SaveChanges();
            return RedirectToAction("index", "color");
        }
        public IActionResult Edit(int Id)
        {
            Color color = _context.Color.FirstOrDefault(x => x.Id == Id);

            if (color==null)
            {
                return RedirectToAction("notfound", "errors");
            }

            return View(color);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Color color)
        {
            Color existcolor = _context.Color.FirstOrDefault(x => x.Id == color.Id);

            if (existcolor == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            existcolor.Name = color.Name;

            _context.SaveChanges();

            return RedirectToAction("index", "color");
        }
        public IActionResult Delete(int id)
        {
            Color color = _context.Color.FirstOrDefault(x => x.Id == id && x.IsDeleted==false);

            if (color == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            color.IsDeleted = true;

            _context.SaveChanges();

            return RedirectToAction("index", "color");
        }
    }
}
