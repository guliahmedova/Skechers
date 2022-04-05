using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class BasketItem:BaseEntity
    {
        public int SizeId { get; set; }
        public int ProductId { get; set; }
        public string AppUserId { get; set; }
        public int Count { get; set; }
        public Size Size { get; set; }
        public AppUser AppUser { get; set; }
        public Product Product { get; set; }
    }
}
