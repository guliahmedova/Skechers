using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skechers.Areas.Manage.ViewModels;
using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public DashboardController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<Order> todayorders = _context.Orders.Where(x => x.Status == Enum.OrderStatus.Accepted).Where(x => x.CreateAt.Day == DateTime.Now.Day).ToList();
            List<Order> totalsales = _context.Orders.ToList();

            decimal todaysale = 0;
            foreach (Order item in todayorders)
            {
                todaysale += item.TotalAmount;
            }

            decimal totalsale = 0;
            foreach (Order item in totalsales)
            {
                totalsale += item.TotalAmount;
            }

            ViewBag.TodaySale = todaysale;
            ViewBag.TotalSale = totalsale;

            OrderViewModel orderVM = new OrderViewModel
            {
                Products = _context.Product.ToList(),
                Orders = totalsales
            };

            return View(orderVM);
        }
        #region CreateAdmin
        //public async Task<IActionResult> Create()
        //{
        //    AppUser admin = new AppUser
        //    {
        //        UserName = "SuperAdmin",
        //        FullName = "Super Admin"
        //    };

        //    var result = await _userManager.CreateAsync(admin, "Admin123");

        //    return Ok(result);
        //}
        #endregion
    }
}
