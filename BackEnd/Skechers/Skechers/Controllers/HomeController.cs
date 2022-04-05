using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skechers.Models;
using Skechers.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Products = _context.Product.Include(x => x.ProductImages).Where(x => x.IsDeleted == false).Take(30).ToList(),
                NewProducts = _context.Product.Include(x => x.ProductImages).Where(x => x.IsNew == true && x.IsDeleted == false && x.IsFeatured == false).Include(x => x.ProductImages).Take(30).ToList(),
                IndirimProduct = _context.Product.Include(x => x.ProductImages).Where(x => x.IsFeatured == true && x.IsDeleted == false && x.IsNew == false).Include(x => x.ProductImages).Take(30).ToList(),
                Offers = _context.Offers.Where(x => x.IsDeleted == false).ToList(),
            };

            return View(homeViewModel);
        }
        public IActionResult Partial()
        {
            return PartialView("_ProductTabSlider", _context.Product.Include(x => x.ProductImages).Where(x => x.IsDeleted == false).Take(20).ToList());
        }
    }
}
