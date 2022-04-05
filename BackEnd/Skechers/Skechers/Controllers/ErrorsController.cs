using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult Notfound()
        {
            return View();
        }
    }
}
