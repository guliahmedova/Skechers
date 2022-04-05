using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.ViewModels
{
    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }
        public List<Product> Products { get; set; }
    }
}
