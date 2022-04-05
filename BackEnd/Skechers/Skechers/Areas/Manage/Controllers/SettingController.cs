using Microsoft.AspNetCore.Mvc;
using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SettingController : Controller
    {
        private readonly DataContext _context;
        public SettingController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, bool? selected = null, string? key = null)
        {
            IQueryable<Setting> settingquery = _context.Settings.AsQueryable();

            if (selected == false) { settingquery = settingquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { settingquery = settingquery.Where(x => x.IsDeleted == true); }
            if (key != null) { settingquery = settingquery.Where(x => x.Key.Contains(key)); }


            ViewBag.SelectedPage = page;
            return View(PagenatedList<Setting>.Create(settingquery, page, 5));
        }
        public IActionResult Create() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Setting setting)
        {
            _context.Settings.Add(setting);
            _context.SaveChanges();

            return RedirectToAction("index", "setting");
        }
        public IActionResult Edit(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(x => x.Id == id);

            if (setting == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            return View(setting);
        }
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Setting setting)
        {
            Setting existsetting = _context.Settings.FirstOrDefault(x => x.Id == setting.Id);

            if (existsetting==null)
            {
                return RedirectToAction("notfound", "errors");
            }

            existsetting.Key = setting.Key;
            existsetting.Value = setting.Value;

            _context.SaveChanges();

            return RedirectToAction("index", "setting");
        }
        public IActionResult Delete(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(x => x.Id == id);

            if (setting == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            _context.Settings.Remove(setting);
            _context.SaveChanges();

            return RedirectToAction("index", "setting");
        }
    }
}
