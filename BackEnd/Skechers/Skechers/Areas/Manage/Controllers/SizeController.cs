using Microsoft.AspNetCore.Mvc;
using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SizeController : Controller
    {
        private readonly DataContext _context;

        public SizeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, bool? selected = null, decimal? word = null)
        {
            IQueryable<Size> sizequery = _context.Sizes.Where(x=>!x.IsDeleted).AsQueryable();

            if (selected == false) { sizequery = sizequery.Where(x => x.IsDeleted == false); }
            if (selected == true) { sizequery = sizequery.Where(x => x.IsDeleted == true); }
            if (word != null) { sizequery = sizequery.Where(x => x.Name == word); }

            ViewBag.SelectedPage = page;

            return View(PagenatedList<Size>.Create(sizequery, page, 10));
        }
        public IActionResult Create() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Size size)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("notfound", "errors");
            }

            _context.Sizes.Add(size);

            _context.SaveChanges();
            return RedirectToAction("index", "size");
        }
        public IActionResult Edit(int Id)
        {
            Size size = _context.Sizes.FirstOrDefault(x => x.Id == Id);

            if (size == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            return View(size);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Size size)
        {
            Size existsize = _context.Sizes.FirstOrDefault(x => x.Id == size.Id);

            if (existsize == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            existsize.Name = size.Name;

            _context.SaveChanges();

            return RedirectToAction("index", "size");
        }
        public IActionResult Delete(int id)
        {
            Size size = _context.Sizes.FirstOrDefault(x => x.Id == id);

            if (size == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            size.IsDeleted = true;

            _context.SaveChanges();

            return RedirectToAction("index", "size");
        }
    }
}
