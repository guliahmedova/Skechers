using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.ViewModels
{
    public class GenderViewModel
    {
        public List<Gender>  Genders { get; set; }
        public PagenatedList<Gender> PagenatedGenders { get; set; }
    }
}
