using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Skechers.Models;
using Skechers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Controllers
{
    public class WishListController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public WishListController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            WishListViewModel wishListVM = new WishListViewModel
            {
                WishlistItems = await _getWishItems(),
            };
            return View(wishListVM);
        }
        public async Task<IActionResult> AddWish(int id)
        {
            if (!_context.Product.Any(x => x.Id == id))
            {
                return RedirectToAction("notfound", "errors");
            }
            WishViewModel wishData = null;
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            if (user != null && user.IsAdmin == false)
            {

                WishList wishItem = _context.Wishlists.FirstOrDefault(x => x.AppUserId == user.Id && x.ProductId == id);

                if (wishItem == null)
                {
                    wishItem = new WishList
                    {
                        AppUserId = user.Id,
                        ProductId = id,
                    };
                    _context.Wishlists.Add(wishItem);
                }

                _context.SaveChanges();

                wishData = _getWishItems(_context.Wishlists.Include(x => x.Product).Where(x => x.AppUserId == user.Id).ToList());
            }
            else
            {
                List<CookieWishItemViewModel> wishItems = new List<CookieWishItemViewModel>();
                string existWishItem = HttpContext.Request.Cookies["wishItemList"];
                if (existWishItem != null)
                {
                    wishItems = JsonConvert.DeserializeObject<List<CookieWishItemViewModel>>(existWishItem);
                }
                CookieWishItemViewModel item = wishItems.FirstOrDefault(x => x.ProductId == id);
                if (item == null)
                {
                    item = new CookieWishItemViewModel
                    {
                        ProductId = id,
                    };
                    wishItems.Add(item);
                }
                string productIdStr = JsonConvert.SerializeObject(wishItems);
                HttpContext.Response.Cookies.Append("wishItemList", productIdStr);
                wishData = _getWishItems(wishItems);
            }
            TempData["Success"] = "Product added into wishlist";
            return RedirectToAction("index", "wishlist");
        }
        public IActionResult DeleteWish(int id)
        {
            if (!_context.Product.Any(x => x.Id == id))
            {
                return RedirectToAction("notfound", "errors");
            }
            List<WishItemViewModel> wishItems = new List<WishItemViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                WishList wishItem = _context.Wishlists.FirstOrDefault(x => x.ProductId == id);
                if (wishItem == null)
                {
                    return RedirectToAction("notfound", "errors"); 
                }

                _context.Wishlists.Remove(wishItem);
                _context.SaveChanges();
            }
            else
            {
                string wish = HttpContext.Request.Cookies["wishItemList"];
                wishItems = JsonConvert.DeserializeObject<List<WishItemViewModel>>(wish);
                WishItemViewModel productWish = wishItems.FirstOrDefault(x => x.ProductId == id);
                if (productWish == null)
                {
                    return RedirectToAction("notfound", "errors");
                }

                wishItems.Remove(productWish);

                HttpContext.Response.Cookies.Append("wishItemList", JsonConvert.SerializeObject(wishItems));
            }
            TempData["Success"] = "Ayakkabı favorilerden silindi";
            return RedirectToAction("index", "product");
        }
        private async Task<List<WishlistItemViewModel>> _getWishItems()
        {
            List<WishlistItemViewModel> wishlistItems = new List<WishlistItemViewModel>();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {
                List<WishList> wishItems = _context.Wishlists.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == user.Id).ToList();

                foreach (WishList item in wishItems)
                {
                    WishlistItemViewModel wishlistItem = new WishlistItemViewModel
                    {
                        Product = item.Product,
                    };
                    wishlistItems.Add(wishlistItem);
                }
            }
            else
            {
                string wishItemsStr = HttpContext.Request.Cookies["wishItemList"];
                if (wishItemsStr != null)
                {
                    List<CookieWishItemViewModel> cookieWishItems = JsonConvert.DeserializeObject<List<CookieWishItemViewModel>>(wishItemsStr);

                    foreach (CookieWishItemViewModel item in cookieWishItems)
                    {
                        WishlistItemViewModel checkoutItem = new WishlistItemViewModel
                        {
                            Product = _context.Product.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId),

                        };
                        wishlistItems.Add(checkoutItem);
                    }
                }
            }

            return wishlistItems;
        }
        private WishViewModel _getWishItems(List<CookieWishItemViewModel> cookieWishItems)
        {
            WishViewModel wishItems = new WishViewModel()
            {
                WishItems = new List<WishItemViewModel>(),
            };
            foreach (CookieWishItemViewModel item in cookieWishItems)
            {
                Product product = _context.Product.FirstOrDefault(x => x.Id == item.ProductId);
                WishItemViewModel wishItem = new WishItemViewModel
                {
                    Name = product.Name,
                    Price = (product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice),
                    SalePrice = product.SalePrice,
                    ProductId = product.Id,
                    Discount = product.DiscountPercent,
                };

            }
            return wishItems;
        }
        private WishViewModel _getWishItems(List<WishList> wishItems)
        {
            WishViewModel wish = new WishViewModel
            {
                WishItems = new List<WishItemViewModel>(),
            };
            foreach (WishList item in wishItems)
            {
                WishItemViewModel wishItem = new WishItemViewModel
                {
                    Name = item.Product.Name,
                    Price = item.Product.DiscountPercent > 0 ? (item.Product.SalePrice * (1 - item.Product.DiscountPercent / 100)) : item.Product.SalePrice,
                    ProductId = item.Product.Id,
                };
                wish.WishItems.Add(wishItem);
            }
            return wish;
        }
    }
}