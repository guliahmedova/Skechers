using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _web;
        public ProductController(DataContext context, IWebHostEnvironment web)
        {
            _context = context;
            _web = web;
        }
        public IActionResult Index(int page = 1, bool? selected = null, string? word = null) 
        {
            IQueryable<Product> productquery = _context.Product.Where(x => x.IsDeleted == false)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductComments)
                .AsQueryable();

            if (selected == false) { productquery = productquery.Where(x => x.IsDeleted == false); }
            if (selected == true) { productquery = productquery.Where(x => x.IsDeleted == true); }
            if (word != null) { productquery = productquery.Where(x => x.Name.Contains(word)); }

            ViewBag.SelectedPage = page;

            return View(PagenatedList<Product>.Create(productquery, page, 10));
        }
        public IActionResult Create() 
        {
            ViewBag.Genders = _context.Genders.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Sizes = _context.Sizes.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Colors = _context.Color.Where(x => x.IsDeleted == false).ToList();

            return View(); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ViewBag.Genders = _context.Genders.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Sizes = _context.Sizes.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Colors = _context.Color.Where(x => x.IsDeleted == false).ToList();

            if (!ModelState.IsValid) return RedirectToAction("error", "errors");

            if (!_context.Genders.Any(x => x.Id == product.GenderId))
            {
                ModelState.AddModelError("GenderId", "Gender not found");
                return View();
            }

            product.ProductImages = new List<ProductImage>();

            //images
            if (product.PosterFile == null)
            {
                ModelState.AddModelError("PosterFile", "PosterFile is definitely required");
                return View();
            }
            else
            {
                if (product.PosterFile.Length > 2097152) 
                {
                    ModelState.AddModelError("PosterFile", "PosterFile max size is 2MB");
                    return View();
                }
                if (product.PosterFile.ContentType != "image/jpeg" && product.PosterFile.ContentType != "image/png" && product.PosterFile.ContentType != "image/webp") 
                {
                    ModelState.AddModelError("PosterFile", "ContentType incorrect");
                    return View();
                }

                ProductImage posterimage = new ProductImage
                {
                    Image = FIleManager.Save(_web.WebRootPath, "uploads/products", product.PosterFile),
                    Product = product,
                    PosterStatus = true
                };

                product.ProductImages.Add(posterimage);
            }

            if (product.HoverPosterFile == null)
            {
                ModelState.AddModelError("HoverPosterFile", "HoverPosterFile file is required");
                return View();
            }
            else
            {
                if (product.HoverPosterFile.Length > 2097152)
                {
                    ModelState.AddModelError("HoverPosterFile", "HoverPosterFile max size is 2MB");
                    return View();
                }
                if (product.HoverPosterFile.ContentType != "image/jpeg" && product.HoverPosterFile.ContentType != "image/png" && product.HoverPosterFile.ContentType != "image/webp") 
                {
                    ModelState.AddModelError("HoverPosterFile", "ContentType must be image/jpeg or image/png");
                    return View();
                }

                ProductImage posterimage = new ProductImage
                {
                    Image = FIleManager.Save(_web.WebRootPath, "uploads/products", product.HoverPosterFile),
                    Product = product,
                    PosterStatus = false
                };

                product.ProductImages.Add(posterimage);
            }

            if (product.ImageFiles != null) 
            {
                foreach (IFormFile item in product.ImageFiles)
                {
                    if (item.Length > 2097152)
                    {
                        ModelState.AddModelError("ImageFiles", "ImageFile max size is 2MB");
                        return View();
                    }
                    if (item.ContentType != "image/jpeg" && item.ContentType != "image/png" && item.ContentType != "image/webp")
                    {
                        ModelState.AddModelError("ImageFiles", "ContentType must be image/jpeg or image/png");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Product = product,
                        Image = FIleManager.Save(_web.WebRootPath, "uploads/products", item),
                        PosterStatus = null
                    };

                    _context.ProductImages.Add(productImage);
                }
            }
            //images

            if (product.SizeIds != null) 
            {
                product.ProductSizes = new List<ProductSize>();

                foreach (int sizeId in product.SizeIds)
                {
                    ProductSize productSize = new ProductSize
                    {
                        SizeId = sizeId,
                        Product = product
                    };

                    product.ProductSizes.Add(productSize);
                }
            }

            if (product.ColorIds != null) 
            {
                product.ProductColors = new List<ProductColor>();

                foreach (int colorId in product.ColorIds)
                {
                    ProductColor productColor = new ProductColor
                    {
                        ColorId = colorId,
                        Product = product
                    };

                    product.ProductColors.Add(productColor);
                }
            }

            _context.Product.Add(product);
            _context.SaveChanges();

            return RedirectToAction("index", "product");
        }
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _context.Product.Include(x => x.ProductImages)
                .Include(x => x.Gender)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false) ;

            if (product == null) return RedirectToAction("notfound", "errors");

            ViewBag.Genders = _context.Genders.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Sizes = _context.Sizes.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Colors = _context.Color.Where(x => x.IsDeleted == false).ToList();

            product.ColorIds = product.ProductColors.Select(x => x.ColorId).ToList();
            product.SizeIds = product.ProductSizes.Select(x => x.SizeId).ToList();

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product)
        {
            Product existproduct = await _context.Product.Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size)
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == product.Id && x.IsDeleted == false);


            if (existproduct==null) { return RedirectToAction("notfound", "errors"); }

            //images control

            if (product.PosterFile != null)
            {
                if (product.PosterFile.Length> 2097152)
                    ModelState.AddModelError("PosterFile", "Max size is 2MB");
                if (product.PosterFile.ContentType != "image/jpeg" && product.PosterFile.ContentType != "image/png" && product.PosterFile.ContentType != "image/webp") 
                    ModelState.AddModelError("PosterFile", "Content type is not true");
            }

            if (product.HoverPosterFile != null)
            {
                if (product.HoverPosterFile.Length > 2097152)
                    ModelState.AddModelError("HoverPosterFile", "Max size is 2MB");
                if (product.HoverPosterFile.ContentType != "image/jpeg" && product.HoverPosterFile.ContentType != "image/png" && product.HoverPosterFile.ContentType != "image/webp")
                    ModelState.AddModelError("HoverPosterFile", "Content type is not true");
            }

            if (product.ImageFiles != null) 
            {
                foreach (IFormFile image in product.ImageFiles)
                {
                    if (image.Length > 2097152)
                    {
                        ModelState.AddModelError("ImageFiles", "Max size is 2MB");
                        break;
                    }
                    if (image.ContentType != "image/jpeg" && image.ContentType != "image/png" && image.ContentType != "image/webp")
                    {
                        ModelState.AddModelError("ImageFiles", "Content type is not true");
                        break;
                    }
                }
            }


            if (!ModelState.IsValid) return RedirectToAction("error", "errors");


            if (product.PosterFile != null)
            {
                ProductImage currentPoster = existproduct.ProductImages.FirstOrDefault(x => x.PosterStatus == true);

                if (currentPoster == null) return RedirectToAction("notfound", "errors");
                _setProductImage(currentPoster, product.PosterFile);
            }

            if (product.HoverPosterFile != null)
            {
                ProductImage currentPoster = existproduct.ProductImages.FirstOrDefault(x => x.PosterStatus == false);

                if (currentPoster == null)
                {
                    return RedirectToAction("notfound", "errors");
                }

                _setProductImage(currentPoster, product.HoverPosterFile);
            }

            if (product.ImageFiles != null)
            {
                foreach (ProductImage image in existproduct.ProductImages.Where(x=>x.PosterStatus==null))
                {
                    FIleManager.Delete(_web.WebRootPath, "uploads/products", image.Image);
                    _context.ProductImages.Remove(image);
                }
                foreach (IFormFile imagefile in product.ImageFiles)
                {
                    ProductImage productImage = new ProductImage
                    {
                        ProductId = product.Id,
                        PosterStatus = null,
                        Image = FIleManager.Save(_web.WebRootPath, "uploads/products", imagefile)
                    };

                    _context.ProductImages.Add(productImage);
                }
            }

            _setProducrData(existproduct, product);

            existproduct.ProductColors.RemoveAll(x => !product.ColorIds.Contains(x.ColorId));
            existproduct.ProductSizes.RemoveAll(x => !product.SizeIds.Contains(x.SizeId));

            foreach (int colorid in product.ColorIds.Where(x => !existproduct.ProductColors.Any(pc => pc.ColorId == x)))
            {
                ProductColor productColor = new ProductColor
                {
                    ProductId = product.Id,
                    ColorId = colorid
                };

                existproduct.ProductColors.Add(productColor);
            }

            foreach (int sizeid in product.SizeIds.Where(x => !existproduct.ProductSizes.Any(pc => pc.SizeId == x)))
            {
                ProductSize productSize = new ProductSize
                {
                    ProductId = product.Id,
                    SizeId = sizeid,
                };

                existproduct.ProductSizes.Add(productSize);
            }

            for (int i = 0; i < product.SizeIds.Count(); i++)
            {
                ProductSize existProductSize = _context.ProductSizes.Include(x => x.Size)
                    .FirstOrDefault(x => x.SizeId == product.SizeIds[i] && x.ProductId == product.Id);

                if (existProductSize == null) { break; }
                existProductSize.SizeCount = product.SizeCountList[i];
            }

            _context.SaveChanges();

            return RedirectToAction("index", "product");
        }
        public IActionResult Delete(int id)
        {
            Product product = _context.Product.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (product == null) 
            {
                return RedirectToAction("notfound", "errors");
            }

            product.IsDeleted = true;
            _context.SaveChanges();

            return RedirectToAction("index", "product");
        }
        public IActionResult Comments(int productId)
        {
            List<ProductComment> comments = _context.ProductComments.Include(x => x.Product).Where(x => x.ProductId == productId).ToList();
            return View(comments);
        }
        public IActionResult DeleteComment(int id)
        {
            ProductComment comment = _context.ProductComments.FirstOrDefault(x => x.Id == id);

            if (comment == null) return RedirectToAction("notfound", "errors");

            _context.ProductComments.Remove(comment);

            TempData["Error"] = "Yorumunuz kabul edilmedi";

            _context.SaveChanges();

            return Ok();
        }
        public IActionResult InfoComment(int id)
        {
            ProductComment comment = _context.ProductComments.Include(x => x.Product).FirstOrDefault(x => x.Id == id);

            if (comment == null) return RedirectToAction("notfound", "errors");

            return View(comment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AcceptComment(int id)
        {
            ProductComment comment = _context.ProductComments.FirstOrDefault(x => x.Id == id);

            if (comment == null) return RedirectToAction("notfound", "errors");

            comment.Status = true;


            TempData["Success"] = "Yorumunuz kabul edild";

            _context.SaveChanges();

            return RedirectToAction("index");
        }
        private void _setProducrData(Product existProduct, Product product)
        {
            existProduct.Code = product.Code;
            existProduct.CostPrice = product.CostPrice;
            existProduct.SalePrice = product.SalePrice;
            existProduct.Name = product.Name;
            existProduct.MarkaName = product.MarkaName;
            existProduct.IsFeatured = product.IsFeatured;
            existProduct.IsNew = product.IsNew;
            existProduct.DiscountPercent = product.DiscountPercent;
            existProduct.Desc = product.Desc;
            existProduct.GenderId = product.GenderId;
        }
        private void _setProductImage(ProductImage image, IFormFile file)
        {
            string newfilename = FIleManager.Save(_web.WebRootPath, "uploads/products", file);

            FIleManager.Delete(_web.WebRootPath, "uploads/products", image.Image);

            image.Image = newfilename;
        }
    }
}
