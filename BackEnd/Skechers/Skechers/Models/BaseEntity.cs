using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Models
{
    public class BaseEntity
    {
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
}
