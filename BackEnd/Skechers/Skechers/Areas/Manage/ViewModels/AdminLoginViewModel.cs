﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required]
        [StringLength(maximumLength: 28, MinimumLength = 3)]
        public string UserName { get; set; }
        [Required]
        [StringLength(maximumLength:25,MinimumLength =8)]
        public string Password { get; set; }
    }
}
