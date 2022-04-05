using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.ViewModels
{
    public class CookieBasketItemViewModel
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal SizeName { get; set; }
        public int SizeId { get; set; }
        public string PosterImage { get; set; }
        public string MarkaName { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercent { get; set; }

    }
}
