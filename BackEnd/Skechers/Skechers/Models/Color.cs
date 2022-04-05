using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class Color:BaseEntity
    {
        [StringLength(maximumLength: 25)]
        public string Name { get; set; }
        public List<ProductColor> ProductColors { get; set; }
    }
}
