using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Skechers.Models;
using Skechers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Services
{
    public class LayoutService
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LayoutService(DataContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Setting>> GetSettings()
        {
            return await _context.Settings.ToListAsync();
        }
        public async Task<List<BasketItemViewModel>> GetBasketItems()
        {
            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            AppUser member = null;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName.Contains(_httpContextAccessor.HttpContext.User.Identity.Name) && !x.IsAdmin);
            }
            if (member == null)
            {
                string itemStr = _httpContextAccessor.HttpContext.Request.Cookies["basketItemList"];
                if (itemStr != null) { basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(itemStr); }
            }
            else
            {
                List<BasketItem> basketItemsDb = _context.BasketItems
                    .Include(x => x.Product).ThenInclude(x => x.ProductImages)
                    .Include(x => x.Product).ThenInclude(x => x.ProductSizes).ThenInclude(x => x.Size)
                    .Where(x => x.AppUserId == member.Id).ToList();
                basketItems = basketItemsDb.Select(x => new BasketItemViewModel
                {
                    Count = x.Count,
                    SizeId = x.SizeId,
                    Name = x.Product.Name,
                    ProductId = x.ProductId,
                    DiscountPercent = x.Product.DiscountPercent,
                    SizeName = x.Product.ProductSizes.FirstOrDefault(ps => ps.SizeId == x.SizeId).Size.Name,
                    PosterImage = x.Product.ProductImages.FirstOrDefault(x => x.PosterStatus == true).Image
                }).ToList();
            }
            return basketItems;
        }
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Product.Include(x => x.ProductImages)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size)
                .Include(x => x.ProductComments)
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }
    }
}
