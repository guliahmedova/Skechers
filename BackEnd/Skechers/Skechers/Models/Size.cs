using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class Size:BaseEntity
    {
        [Column(TypeName = "decimal(18,2)")]
        [Range(32,45)]
        public decimal Name { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
    }
}
