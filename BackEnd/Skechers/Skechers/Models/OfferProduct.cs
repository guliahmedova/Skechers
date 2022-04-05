using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class OfferProduct:BaseEntity
    {
        public int ProductId { get; set; }
        public int OfferId { get; set; }
        public Product Product { get; set; }
        public Offer Offer { get; set; }
    }
}
