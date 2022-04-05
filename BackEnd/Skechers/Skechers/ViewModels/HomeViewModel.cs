using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.ViewModels
{
    public class HomeViewModel
    {
        public List<Offer> Offers { get; set; }
        public List<Product> MostSaleProducts { get; set; }
        public List<Product> Products { get; set; }
        public List<Product> NewProducts { get; set; }
        public List<Product> IndirimProduct { get; set; }
    }
}
