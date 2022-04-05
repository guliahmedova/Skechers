using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public ProductComment Comment { get; set; }
        public List<Product> Products { get; set; }
        public List<Color> Colors { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductColor> ProductColors { get; set; }
    }
}
