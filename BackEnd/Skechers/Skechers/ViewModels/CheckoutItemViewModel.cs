﻿using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.ViewModels
{
    public class CheckoutItemViewModel
    {
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
