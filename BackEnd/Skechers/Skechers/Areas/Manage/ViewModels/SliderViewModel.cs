using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.ViewModels
{
    public class SliderViewModel
    {
        public List<Product> Products { get; set; }
        public PagenatedList<Product> PagenationProducts { get; set; }
    }
}
