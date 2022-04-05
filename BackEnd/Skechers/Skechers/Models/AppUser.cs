using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class AppUser:IdentityUser
    {
        [MaxLength(50)]
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
