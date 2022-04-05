using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.ViewModels
{
    public class BasketViewModel
    {
        public decimal TotalAmount { get; set; }
        public List<BasketItemViewModel> BasketItems { get; set; }
    }
    public class BasketItemViewModel
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string MarkaName { get; set; }
        public string PosterImage { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal SizeName { get; set; }
        public int SizeId { get; set; }
        public decimal DiscountPercent { get; set; }
        public Product Product { get; set; }
    }
}
