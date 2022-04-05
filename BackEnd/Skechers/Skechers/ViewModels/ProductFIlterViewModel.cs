using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.ViewModels
{
    public class ProductFIlterViewModel
    {
        public List<Gender> Genders { get; set; }
        public PagenatedList<Product> ProductsPagenated { get; set; }
    }
}
