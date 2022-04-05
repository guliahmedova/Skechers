using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Skechers.Areas.Manage.ViewModels;
using Skechers.Helper;
using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.Controllers
{
    [Area("manage")]
    public class OfferController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;
        public OfferController(DataContext context, IWebHostEnvironment _env)
        {
            _context = context;
            this._env = _env;
        }
        public IActionResult Index(int page = 1, bool? selected = null, string? word = null)
        {
            IQueryable<Offer> offerquery = _context.Offers.Where(x=>x.IsDeleted==false).AsQueryable();

            if (selected == false) { offerquery = offerquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { offerquery = offerquery.Where(x => x.IsDeleted == true); }
            if (word != null) { offerquery = offerquery.Where(x => x.Name.Contains(word)); }

            ViewBag.SelectedPage = page;


            return View(PagenatedList<Offer>.Create(offerquery, page, 2));
        }
        public IActionResult Create() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Offer offer)
        {
            if (offer.ImageFile == null)
                ModelState.AddModelError("ImageFile", "ImageFile is required");
            else if (offer.ImageFile.Length > 2097152)
                ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB");
            else if (offer.ImageFile.ContentType != "image/jpeg" && offer.ImageFile.ContentType != "image/png" && offer.ImageFile.ContentType != "image/webp")
                ModelState.AddModelError("ImageFile", "ContentType must be image/jpeg or image/png");

            if (!ModelState.IsValid) { return RedirectToAction("error", "errors"); }

            offer.Image = FIleManager.Save(_env.WebRootPath, "uploads/products", offer.ImageFile);
            
            _context.Offers.Add(offer);
            _context.SaveChanges();

            return RedirectToAction("index", "offer");
        }
        public IActionResult Edit(int id)
        {
            Offer offer = _context.Offers.FirstOrDefault(x => x.Id == id );

            if (offer==null)
            {
                return RedirectToAction("notfound", "errors");
            }

            return View(offer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Offer offer)
        {
            Offer existoffer = _context.Offers.FirstOrDefault(x => x.Id == offer.Id);

            if (existoffer == null) { return RedirectToAction("notfound", "errors"); }

            if (offer.ImageFile != null)
            {
                if (offer.ImageFile.Length > 2097152)
                { ModelState.AddModelError("ImageFile", "ImageFile max size is 2MB"); return View(existoffer); }
                else if (offer.ImageFile.ContentType != "image/jpeg" && offer.ImageFile.ContentType != "image/png" && offer.ImageFile.ContentType != "image/webp")
                { ModelState.AddModelError("ImageFile", "ContentType must be image/jpeg or image/png"); return View(existoffer); }
            }

            if (!ModelState.IsValid) { return RedirectToAction("error", "errors"); }

            if (offer.ImageFile != null)
            {
                FIleManager.Delete(_env.WebRootPath, "uploads/products", existoffer.Image);
                existoffer.Image = FIleManager.Save(_env.WebRootPath, "uploads/products", offer.ImageFile);
            }

            if (offer.Image != null)
            {
                string newfilename = FIleManager.Save(_env.WebRootPath, "uploads/products", offer.ImageFile);

                FIleManager.Delete(_env.WebRootPath, "uploads/products", existoffer.Image);

                existoffer.Image = newfilename;
            }

            existoffer.Name = offer.Name;
            _context.SaveChanges();

            return RedirectToAction("index", "offer");
        }
        public IActionResult Delete(int id)
        {
            Offer offer = _context.Offers.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (offer == null) { return RedirectToAction("notfound", "errors"); }

            offer.IsDeleted = true;
            _context.SaveChanges();

            return RedirectToAction("index", "offer");
        }
    }
}
