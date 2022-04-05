using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class Setting:BaseEntity
    {
        [Required]
        [StringLength(maximumLength:100)]
        public string Key { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Value { get; set; }
    }
}
