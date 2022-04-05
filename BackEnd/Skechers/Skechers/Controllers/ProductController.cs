using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Skechers.Models;
using Skechers.Services;
using Skechers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public ProductController(DataContext context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }
        public IActionResult Index(int page = 1, bool? selected = null, string? word = null)
        {
            IQueryable<Product> productsquery = _context.Product
                .Include(x => x.ProductComments)
                .Include(x => x.ProductImages)
                .Include(x => x.Wishlists)
                .Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.Gender)
                .Where(x => x.IsDeleted == false).AsQueryable();

            if (selected == false) { productsquery = productsquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { productsquery = productsquery.Where(x => x.IsDeleted == true); }
            if (word != null) { productsquery = productsquery.Where(x => x.Name.Contains(word)); }

            ViewBag.select = selected;
            ViewBag.word = word;
            ViewBag.PageSize = 2;
            ViewBag.ProductCount = productsquery.Count();

            return View(PagenatedList<Product>.Create(productsquery, page, 5));
        }


        //
        public IActionResult Kadin(int page = 1, bool? selected = null, string? word = null)
        {
            IQueryable<Product> productsquery = _context.Product
                .Include(x => x.ProductComments)
                .Include(x => x.ProductImages)
                .Include(x => x.Wishlists)
                .Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.Gender)
                .Where(x => x.IsDeleted == false && x.Gender.Name== "Kadın").AsQueryable();

            if (selected == false) { productsquery = productsquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { productsquery = productsquery.Where(x => x.IsDeleted == true); }
            if (word != null) { productsquery = productsquery.Where(x => x.Name.Contains(word)); }

            ViewBag.select = selected;
            ViewBag.word = word;
            ViewBag.PageSize = 2;
            ViewBag.ProductCount = productsquery.Count();

            return View(PagenatedList<Product>.Create(productsquery, page, 5));
        }
        //

        //
        public IActionResult Erkek(int page = 1, bool? selected = null, string? word = null)
        {
            IQueryable<Product> productsquery = _context.Product
                .Include(x => x.ProductComments)
                .Include(x => x.ProductImages)
                .Include(x => x.Wishlists)
                .Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.Gender)
                .Where(x => x.IsDeleted == false && x.Gender.Name == "Erkek").AsQueryable();

            if (selected == false) { productsquery = productsquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { productsquery = productsquery.Where(x => x.IsDeleted == true); }
            if (word != null) { productsquery = productsquery.Where(x => x.Name.Contains(word)); }

            ViewBag.select = selected;
            ViewBag.word = word;
            ViewBag.PageSize = 2;
            ViewBag.ProductCount = productsquery.Count();

            return View(PagenatedList<Product>.Create(productsquery, page, 5));
        }
        //

        //
        public IActionResult Cocuk(int page = 1, bool? selected = null, string? word = null)
        {
            IQueryable<Product> productsquery = _context.Product
                .Include(x => x.ProductComments)
                .Include(x => x.ProductImages)
                .Include(x => x.Wishlists)
                .Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.Gender)
                .Where(x => x.IsDeleted == false && x.Gender.Name == "Çocuk").AsQueryable();

            if (selected == false) { productsquery = productsquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { productsquery = productsquery.Where(x => x.IsDeleted == true); }
            if (word != null) { productsquery = productsquery.Where(x => x.Name.Contains(word)); }

            ViewBag.select = selected;
            ViewBag.word = word;
            ViewBag.PageSize = 2;
            ViewBag.ProductCount = productsquery.Count();

            return View(PagenatedList<Product>.Create(productsquery, page, 5));
        }
        //

        //
        public IActionResult Indirim(int page = 1, bool? selected = null, string? word = null)
        {
            IQueryable<Product> productsquery = _context.Product
                .Include(x => x.ProductComments)
                .Include(x => x.ProductImages)
                .Include(x => x.Wishlists)
                .Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.Gender)
                .Where(x => x.IsDeleted == false && x.DiscountPercent > 0).AsQueryable();

            if (selected == false) { productsquery = productsquery.Where(x => x.IsDeleted == false && x.DiscountPercent > 0) ; }
            if (selected == true) { productsquery = productsquery.Where(x => x.IsDeleted == true && x.DiscountPercent >0); }
            if (word != null) { productsquery = productsquery.Where(x => x.Name.Contains(word)); }

            ViewBag.select = selected;
            ViewBag.word = word;
            ViewBag.SelectedPage = page;
            ViewBag.ProductCount = productsquery.Where(x=>x.DiscountPercent>0).Count();

            return View(PagenatedList<Product>.Create(productsquery, page, 5));
        }
        //


        public IActionResult Detail(int id)
        {
            Product product = _context.Product.Include(x => x.ProductImages)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.ProductComments)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size)
                .FirstOrDefault(x => x.Id == id && x.IsDeleted==false);

            if (product == null) { return RedirectToAction("notfound", "errors"); }

            ProductDetailViewModel productDetailviewModel = new ProductDetailViewModel
            {
                Product = product,
                Products = _context.Product.Where(x => !x.IsDeleted).ToList(),
                Comment = new ProductComment(),
                ProductSizes = _context.ProductSizes.Where(x => !x.IsDeleted && x.ProductId == product.Id).ToList(),
                ProductColors=_context.ProductColors.Where(x=>x.IsDeleted==false && x.ProductId==product.Id).ToList()
            };

            return View(productDetailviewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(ProductComment comment)
        {
            Product product = _context.Product
                .Include(x => x.ProductImages).Include(x => x.Gender)
                .Include(x => x.ProductColors).ThenInclude(x => x.Color)
                .Include(x => x.ProductSizes).ThenInclude(x => x.Size)
                .Include(x => x.ProductComments).FirstOrDefault(x => x.Id == comment.ProductId && !x.IsDeleted);

            //List<ProductColor> productColors = _context.ProductColors.Where(x => x.ProductId == product.Id).ToList();

            if (product == null) { return NotFound(); }

            ProductDetailViewModel productDetailVM = new ProductDetailViewModel
            {
                Product = product,
                Comment = comment,
                Colors = _context.Color.Where(x => !x.IsDeleted).ToList(),
                Products = _context.Product.Where(x => !x.IsDeleted).ToList()
            };

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Comment Error";
                return View("Detail", productDetailVM);
            }

            if (!_context.Product.Any(x => x.Id == comment.ProductId))
            {
                TempData["Error"] = "Selected Product not found";
                return View("Detail", productDetailVM);
            }

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user == null)
            {
                TempData["Warning"] = "Giriş yap ve ya y";
                return View("detail", productDetailVM);
            }
            else
            {
                comment.AppUserId = user.Id;
                comment.Email = user.Email;
                comment.FullName = user.FullName;
            }
            comment.Status = false;
            comment.CreatedAt = DateTime.UtcNow.AddHours(4);

            _context.ProductComments.Add(comment);
            _context.SaveChanges();

            TempData["Success"] = "Comment added successfully";

            return RedirectToAction("detail", new { Id = comment.ProductId });
        }
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            ProductComment productComment = await _context.ProductComments.FirstOrDefaultAsync(x => x.Id == commentId && x.IsDeleted == false) ;

            if (productComment == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            _context.ProductComments.Remove(productComment);
            _context.SaveChanges();

            return RedirectToAction("detail", "product");
        }
        public async Task<IActionResult> AddBasket(int productId , int sizeId)
        {
            if (!_context.Product.Any(x => x.Id == productId))
            {
                return NotFound();
            }

            ProductSize productSize = await _context.ProductSizes.Include(x => x.Product).ThenInclude(x=>x.ProductImages)
                .Include(x=>x.Product).ThenInclude(x=>x.ProductSizes)
                .Include(x => x.Size).FirstOrDefaultAsync(x => x.ProductId == productId && x.SizeId == sizeId);

            if (productSize == null) {
                TempData["Error"] = "Ölçüsüz ayakkabı olmaz!!";
                return NotFound();
            }

            BasketViewModel data = null;
            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            if (user != null && user.IsAdmin == false)
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.AppUserId == user.Id && x.ProductId == productId);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = user.Id,
                        ProductId = productId,
                        Count = 1,
                        SizeId = sizeId,
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                
                _context.SaveChanges();
                
                data = _getBasketItems(_context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == user.Id).ToList());
            }
            else
            {
                List<CookieBasketItemViewModel> basketItems = new List<CookieBasketItemViewModel>();
                string existBasketItems = HttpContext.Request.Cookies["basketItemList"];

                if (existBasketItems != null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(existBasketItems);
                }

                //CookieBasketItemViewModel item = basketItems.FirstOrDefault(x => x.ProductId == productId);
                CookieBasketItemViewModel item = basketItems.FirstOrDefault(x => x.ProductId == productId && x.SizeName == productSize.Size.Name);
                if (item == null)
                {
                    item = new CookieBasketItemViewModel
                    {
                        ProductId = productId,
                        Count = 1,
                        SizeName = productSize.Product.ProductSizes.FirstOrDefault(x => x.SizeId == sizeId).Size.Name,
                        SizeId = productSize.SizeId,
                        PosterImage = productSize.Product.ProductImages.FirstOrDefault(x=>x.PosterStatus==true).Image,
                        DiscountPercent = productSize.Product.DiscountPercent,
                        MarkaName = productSize.Product.MarkaName,
                        Name = productSize.Product.Name,
                        Price= productSize.Product.SalePrice,
                    };
                    if (productSize.Product.ProductSizes.FirstOrDefault(x => x.ProductId == productId).SizeCount > 0)
                    {
                         basketItems.Add(item);
                    }
                }
                else
                    item.Count++;

                string bookIdsStr = JsonConvert.SerializeObject(basketItems);

                HttpContext.Response.Cookies.Append("basketItemList", bookIdsStr);

                data = _getBasketItems(basketItems);
            }


            //TempData.SizeId = _context.ProductSizes.FirstOrDefault(x => x.SizeId == sizeId).Size.Name;

            TempData["Success"] = "Sepete " + _context.ProductSizes.Include(x => x.Size)
                .FirstOrDefault(x => x.SizeId == sizeId).Size.Name + " Numaralı Ayakkabı Eklendi";


            return Ok(data);
        }
        #region ShowBasket
        public IActionResult ShowBasket()
        {
            string productIdsStr = HttpContext.Request.Cookies["basketitemlist"];
            List<BasketItemViewModel> productIds = new List<BasketItemViewModel>();

            if (productIdsStr != null)
            {
                productIds = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(productIdsStr);
            }

            return Json(productIds);
        }
        #endregion
        #region Basket
        //public async Task<IActionResult> Basket()
        //{
        //    AppUser member = null;

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        member = await _userManager.FindByNameAsync(User.Identity.Name);
        //    }

        //    List<BasketItemViewModel> basketItemsVM = new List<BasketItemViewModel>();
        //    BasketViewModel data = null;

        //    if (member != null && member.IsAdmin == false)
        //    {
        //        List<BasketItem> basketItems = _context.BasketItems.Include(x => x.Product)
        //            .ThenInclude(x => x.ProductImages)
        //            .Include(x => x.Size)
        //            .Where(x => x.AppUserId == member.Id)
        //            .ToList();

        //        data = _getBasketItems(_context.BasketItems.Include(x => x.Product)
        //            .ThenInclude(x => x.ProductImages)
        //            .Where(x => x.AppUserId == member.Id)
        //            .ToList());
        //    }
        //    else
        //    {
        //        List<CookieBasketItemViewModel> basketItems = new List<CookieBasketItemViewModel>();
        //        string bookIdsStr = JsonConvert.SerializeObject(basketItems);
        //        HttpContext.Response.Cookies.Append("basketItemList", bookIdsStr);
        //        data = _getBasketItems(basketItems);
        //    }

        //    return View(basketItemsVM);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Basket(Order order)
        //{
        //    AppUser user = _userManager.Users.FirstOrDefault(x => x.IsAdmin == false && x.UserName == User.Identity.Name);

        //    if (user == null) { return RedirectToAction("notfound", "errors"); }

        //    List<CheckoutItemViewModel> checkoutItems = await _getcheckoutitems();

        //    List<BasketViewModel> basketItems = new List<BasketViewModel>();
        //    string existBasketItems = HttpContext.Request.Cookies["basketItemList"];

        //    if (existBasketItems != null)
        //    {
        //        basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(existBasketItems);
        //    }

        //    if (checkoutItems.Count == 0)
        //    {
        //        ModelState.AddModelError("", "There is nothing here");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return RedirectToAction("error", "errors");
        //    }
        //    return RedirectToAction("profile", "checkout");
        //}
        #endregion
        public IActionResult Basket()
        {
            AppUser user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);

            List<BasketItemViewModel> basketsVM = new List<BasketItemViewModel>();

            basketsVM = _context.BasketItems.Select(x => new BasketItemViewModel()
            {
                Count = x.Count,
                DiscountPercent = x.Product.DiscountPercent,
                Name = x.Product.Name,
                Price = x.Product.SalePrice,
                ProductId = x.ProductId,
                PosterImage = x.Product.ProductImages.FirstOrDefault(x => x.PosterStatus == true).Image,
                Product = _context.Product.FirstOrDefault(y => y.Id == x.ProductId),
                SizeName = x.Size.Name
            }).ToList();

            return View(basketsVM);
        }
        public async Task<IActionResult> Checkout()
        {
            CheckoutViewModel checkoutVM = new CheckoutViewModel
            {
                CheckoutItems = await _getcheckoutitems(),
                Order = new Order()
            };

            return View(checkoutVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(Order order)
        {
            AppUser user = null;

            List<CheckoutItemViewModel> checkoutItems = await _getcheckoutitems();

            if (User.Identity.IsAuthenticated)
            {
                user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && x.IsAdmin == false);
            }

            if (checkoutItems.Count == 0)
                ModelState.AddModelError("", "Hic bir sey yok");

            if (user == null && string.IsNullOrWhiteSpace(order.Email))
                ModelState.AddModelError("Email", "Email zorunlu");

            if (user == null && string.IsNullOrWhiteSpace(order.FullName))
                ModelState.AddModelError("FullName", "FullName zorunlu");

            if (!ModelState.IsValid)
            {
                return View("Checkout", new CheckoutViewModel { CheckoutItems = checkoutItems, Order = order });
            }

            if (user != null)
            {
                order.Email = user.Email;
                order.FullName = user.FullName;
                order.AppUserId = user.Id;
            }

            Order lastOrder = _context.Orders.OrderByDescending(x => x.Id).FirstOrDefault();

            order.CodePrefix = order.FullName[0].ToString().ToUpper() + order.Email[0].ToString().ToUpper();
            order.CodeNumber = lastOrder == null ? 1001 : lastOrder.CodeNumber + 1;
            order.CreateAt = DateTime.UtcNow.AddHours(4);
            order.Status = Enum.OrderStatus.Pending;
            order.OrderItems = new List<OrderItem>();

            foreach (CheckoutItemViewModel item in checkoutItems)
            {
                OrderItem orderItem = new OrderItem
                {
                    ProductId = item.Product.Id,
                    Count = item.Count,
                    CostPrice = item.Product.CostPrice,
                    SalePrice = item.Product.SalePrice,
                    DiscountPercent = item.Product.DiscountPercent
                };

                order.TotalAmount += orderItem.DiscountPercent > 0
                    ? (orderItem.SalePrice * (1 - orderItem.DiscountPercent / 100)) * orderItem.Count
                    : orderItem.SalePrice * orderItem.Count;
                order.DeliveryStatus = Enum.OrderDeliveryStatus.OnWay;
                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            if (user != null)
            {
                _context.BasketItems.RemoveRange(_context.BasketItems.Where(x => x.AppUserId == user.Id));
                _context.SaveChanges();
            }
            else
            {
                HttpContext.Response.Cookies.Delete("basketItemList");
            }

            _emailService.Send(order.AppUser.Email, "Siperiş kabul edildi", "Shechers gururla sunar :)");
            return RedirectToAction("profile", "account");
        }
        public IActionResult DeleteBasket(int productId)
        {
            if (!_context.Product.Any(x => x.Id == productId)) { return NotFound(); }

            List<BasketItemViewModel> productsDetail = new List<BasketItemViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                BasketItem basketItem = _context.BasketItems.Include(x => x.Size).FirstOrDefault(x => x.ProductId == productId);
                if (basketItem == null) { return NotFound(); }
                if (basketItem.Count == 1) { _context.BasketItems.Remove(basketItem); }
                else { basketItem.Count--; }
                _context.SaveChanges();
            }
            else
            {
                string basket = HttpContext.Request.Cookies["basketItemList"];
                productsDetail = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basket);
                BasketItemViewModel productBasket = productsDetail.FirstOrDefault(x => x.ProductId == productId);
                if (productBasket == null) { return NotFound(); }
                if (productBasket.Count == 1) { productsDetail.Remove(productBasket); }
                else { productBasket.Count--; }
                HttpContext.Response.Cookies.Append("basketItemList", JsonConvert.SerializeObject(productsDetail));
            }
            return RedirectToAction("basket", "product");
        }
        #region filter
        //public IActionResult Filter(int? genderId)
        //{
        //    var products = _context.Product.Include(x => x.Gender)
        //        .Include(x => x.ProductImages)
        //        .Include(x => x.ProductSizes)
        //        .Include(x => x.ProductColors)
        //        .Include(x => x.Gender)
        //        .Where(x => x.IsDeleted == false)
        //        .AsQueryable();

        //    ViewBag.GenderId = genderId;

        //    if (genderId != null)
        //    {
        //        products = products.Where(x => x.GenderId == genderId) ;
        //    }

        //    ProductFIlterViewModel productFVM = new ProductFIlterViewModel
        //    {
        //        Genders = _context.Genders.Where(x => x.IsDeleted == false).ToList(),
        //    };

        //    return View(productFVM);
        //}
        #endregion  
        private BasketViewModel _getBasketItems(List<CookieBasketItemViewModel> cookieBasketItems)
        {
            BasketViewModel basket = new BasketViewModel
            {
                BasketItems = new List<BasketItemViewModel>(),
            };

            foreach (CookieBasketItemViewModel item in cookieBasketItems)
            {
                Product product = _context.Product.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId);
                BasketItemViewModel basketItem = new BasketItemViewModel
                {
                    Name = product.Name,
                    Price = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice,
                    ProductId = product.Id,
                    Count = item.Count,
                    PosterImage = product.ProductImages.FirstOrDefault(x => x.PosterStatus == true)?.Image,
                    DiscountPercent = product.DiscountPercent
                };

                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.BasketItems.Add(basketItem);
            }

            return basket;
        }
        private BasketViewModel _getBasketItems(List<BasketItem> basketItems)
        {
            BasketViewModel basket = new BasketViewModel
            {
                BasketItems = new List<BasketItemViewModel>(),
            };

            foreach (BasketItem item in basketItems)
            {
                BasketItemViewModel basketItem = new BasketItemViewModel
                {
                    Name = item.Product.Name,
                    Price = item.Product.DiscountPercent > 0 ? (item.Product.SalePrice * (1 - item.Product.DiscountPercent / 100)) : item.Product.SalePrice,
                    ProductId = item.Product.Id,
                    Count = item.Count,
                    PosterImage = item.Product.ProductImages.FirstOrDefault(x => x.PosterStatus == true)?.Image,
                    DiscountPercent = item.Product.DiscountPercent
                };

                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.BasketItems.Add(basketItem);
            }

            return basket;
        }
        private async Task<List<CheckoutItemViewModel>> _getcheckoutitems()
        {
            List<CheckoutItemViewModel> checkoutItems = new List<CheckoutItemViewModel>();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {
                List<BasketItem> basketItems = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == user.Id).ToList();

                foreach (BasketItem item in basketItems)
                {
                    CheckoutItemViewModel checkoutItem = new CheckoutItemViewModel
                    {
                        Product = item.Product,
                        Count = item.Count
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }
            else
            {
                string basketItemsStr = HttpContext.Request.Cookies["basketItemList"];

                if (basketItemsStr != null)
                {
                    List<CookieBasketItemViewModel> basketItems = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(basketItemsStr);

                    foreach (CookieBasketItemViewModel item in basketItems)
                    {
                        CheckoutItemViewModel checkoutItem = new CheckoutItemViewModel
                        {
                            Product = _context.Product.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId),
                            Count = item.Count
                        };
                        checkoutItems.Add(checkoutItem);
                    }
                }
            }

            return checkoutItems;
        }
    }
}
