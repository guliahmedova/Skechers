using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class ProductImage:BaseEntity
    {
        public int ProductId { get; set; }
        [StringLength(maximumLength: 100)]
        public string Image { get; set; }
        public bool? PosterStatus { get; set; }
        public Product Product { get; set; }
    }
}
