using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skechers.Models;
using Skechers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Controllers
{
    public class SearchController : Controller
    {
        private readonly DataContext _context;
        public SearchController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Search(string markaname)
        {
            List<Product> products = _context.Product.Include(x => x.ProductImages)
                .Where(x => x.MarkaName.Contains(markaname) && !x.IsDeleted).ToList();
            return PartialView("_SearchPartial",products);
        }
    }
}
