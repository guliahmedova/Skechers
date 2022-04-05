using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Controllers
{
    public class OfferController : Controller
    {
        private readonly DataContext _context;

        public OfferController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, bool? selected = null, string? word = null)
        {
            IQueryable<Product> productsquery = _context.Product
                .Include(x => x.ProductComments)
                .Include(x => x.ProductImages)
                .Include(x => x.Wishlists)
                .Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.Gender)
                .Where(x => x.IsDeleted == false && x.DiscountPercent == 10).AsQueryable();

            if (selected == false) { productsquery = productsquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { productsquery = productsquery.Where(x => x.IsDeleted == true); }
            if (word != null) { productsquery = productsquery.Where(x => x.Name.Contains(word)); }

            ViewBag.select = selected;
            ViewBag.word = word;
            ViewBag.PageSize = 2;
            ViewBag.ProductCount = productsquery.Count();

            return View(PagenatedList<Product>.Create(productsquery, page, 5));
        }
    }
}
