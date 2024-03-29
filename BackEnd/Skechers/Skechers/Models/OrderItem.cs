﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class OrderItem:BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercent { get; set; }
        public int Count { get; set; }

        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
