using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.ViewModels
{
    public class ColorViewModel
    {
        public List<Color> Colors { get; set; }
        public PagenatedList<Color> PagenationColor { get; set; }
    }
}
