using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.ViewModels
{
    public class CheckoutViewModel
    {
        public List<BasketItemViewModel> BasketItemsVM { get; set; }
        public List<CheckoutItemViewModel> CheckoutItems { get; set; }
        public Order Order { get; set; }
    }
}
