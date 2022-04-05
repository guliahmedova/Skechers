using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class Offer:BaseEntity
    {
        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        [StringLength(maximumLength: 250)]
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public List<OfferProduct> OfferProducts { get; set; }
    }
}
