using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class Product:BaseEntity
    {
        public int GenderId { get; set; }
        [Required]
        [StringLength(maximumLength:100)]
        public string MarkaName { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercent { get; set; }
        public bool IsFeatured { get; set; }  //indirimde olan productlar ucundu
        public bool IsNew { get; set; }
        [StringLength(maximumLength: 50)]
        public string Code { get; set; }
        [StringLength(maximumLength: 500)]
        public string Desc { get; set; }
        public Gender Gender { get; set; }
        [NotMapped]
        public IFormFile PosterFile { get; set; }
        [NotMapped]
        public IFormFile HoverPosterFile { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; }
        [NotMapped]
        public List<int> SizeIds { get; set; } = new List<int>(); 
        [NotMapped]
        public List<int> ColorIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> OfferIds { get; set; } = new List<int>();
        public List<ProductColor> ProductColors { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<WishList> Wishlists { get; set; }
        public List<OfferProduct> OfferProducts { get; set; }
        [NotMapped]
        public List<int> SizeCountList { get; set; } = new List<int>();
    }
}
