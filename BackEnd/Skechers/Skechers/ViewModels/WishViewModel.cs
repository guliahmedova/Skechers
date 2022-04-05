using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.ViewModels
{
    public class WishViewModel
    {
        public List<WishItemViewModel> WishItems { get; set; }
    }
    public class WishItemViewModel
    {
        public int ProductSizeId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Discount { get; set; }
        public string Name { get; set; }
        public string MarkaName { get; set; }
        public string PosterImage { get; set; }
        public decimal SizeName { get; set; }
    }
}
