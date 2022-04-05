using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skechers.Enum;
using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.Controllers
{
    [Area("manage")]
    public class OrderController : Controller
    {
        private readonly DataContext _context;
        public OrderController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, OrderStatus? orderStatus = OrderStatus.Pending)
        {
            IQueryable<Order> queryorder = _context.Orders.AsQueryable();

            if (orderStatus == OrderStatus.Accepted) { queryorder = queryorder.Where(x => x.Status == OrderStatus.Accepted); }
            if (orderStatus == OrderStatus.Canceled) { queryorder = queryorder.Where(x => x.Status == OrderStatus.Canceled); }
            if (orderStatus == OrderStatus.Pending) { queryorder = queryorder.Where(x => x.Status == OrderStatus.Pending); }
            if (orderStatus == OrderStatus.Rejected) { queryorder = queryorder.Where(x => x.Status == OrderStatus.Rejected); }

            ViewBag.SelectedPage = page;

            return View(PagenatedList<Order>.Create(queryorder, page, 8));
        }
        public IActionResult Update(int id)
        {
            Order order = _context.Orders.Include(x => x.AppUser).Include(x => x.OrderItems).ThenInclude(x => x.Product).ThenInclude(x => x.ProductImages).
                Include(x => x.AppUser).Include(x => x.OrderItems).ThenInclude(x => x.Product).ThenInclude(x => x.ProductSizes).ThenInclude(x => x.Size).
                FirstOrDefault(x => x.Id == id);
            if (order == null) return RedirectToAction("notfound", "errors");
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Accepted(int id)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null) return RedirectToAction("notfound", "errors");
            order.Status = (OrderStatus)2;
            _context.SaveChanges();
            TempData["Success"] = "Siparisiniz kabul edildi";
            return RedirectToAction("Update", new { id = id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Canceled(int id)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null) return RedirectToAction("notfound", "errors");
            order.Status = (OrderStatus)4;
            _context.SaveChanges();
            TempData["Success"] = "Siparişini iptal edildu";
            return RedirectToAction("Update", new { id = id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Pending(int id)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null) return RedirectToAction("notfound", "errors");
            order.Status = (OrderStatus)1;
            _context.SaveChanges();
            TempData["Success"] = "Siparişini beklemede...";
            return RedirectToAction("Update", new { id = id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rejected(int id)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null) return RedirectToAction("notfound", "errors");
            order.Status = (OrderStatus)3;
            _context.SaveChanges();
            TempData["Success"] = "Siparişini kabul edilmedi";
            return RedirectToAction("Update", new { id = id });
        }
    }
}
