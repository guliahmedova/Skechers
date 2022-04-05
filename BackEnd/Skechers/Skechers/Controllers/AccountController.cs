using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Skechers.Models;
using Skechers.Services;
using Skechers.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _web;
        public AccountController(DataContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IWebHostEnvironment web)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _web = web;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(MemberRegisterViewModel memberRegisterVM)
        {
            if(!ModelState.IsValid) { return RedirectToAction("error", "errors"); }

            AppUser user = await _userManager.FindByNameAsync(memberRegisterVM.UserName);

            if (user != null)
            {
                ModelState.AddModelError("UserName", "This Username is already exist");
                return View();
            }

            if (_context.Users.Any(x=>x.NormalizedEmail == memberRegisterVM.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "This Email is already exist");
                return View();
            }

            user = new AppUser
            {
                Email = memberRegisterVM.Email,
                FullName = memberRegisterVM.FullName,
                UserName = memberRegisterVM.UserName
            };

            IdentityResult result = await _userManager.CreateAsync(user, memberRegisterVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _signInManager.SignInAsync(user, false);
            await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.PasswordSignInAsync(user, memberRegisterVM.Password, false, false);


            return RedirectToAction("login", "account");
        }
        public IActionResult Login() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(MemberLoginViewModel memberLoginViewModel)
        {
            if (!ModelState.IsValid) { return RedirectToAction("error", "errors"); }

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => !x.IsAdmin && x.NormalizedUserName == memberLoginViewModel.UserName.ToUpper());

            if (user==null)
            {
                ModelState.AddModelError("", "Password or Username is not correct"); return View();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, memberLoginViewModel.Password, true,true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password not true");
                return View();
            }

            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
        public IActionResult Forgot() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Forgot(ForgotPasswordViewModel forgotPasswordVM)
        {
            if(!ModelState.IsValid) 
            { return RedirectToAction("error", "errors"); }

            AppUser user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);

            if (user==null)
            {
                ModelState.AddModelError("Email", "User not found");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = Url.Action("resetpassword", "account", new { email = user.Email, token = token }, Request.Scheme);

            string skechersUrl = Url.Action("index", "home");

            TempData["Success"] = "Link E-Postaniza gonderildi";

            string pathToFile = _web.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplate"
                            + Path.DirectorySeparatorChar.ToString()
                            + "Email.html";

            BodyBuilder builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {

                builder.HtmlBody = SourceReader.ReadToEnd();

            }

            string messageBody = string.Format(builder.HtmlBody,url, skechersUrl);

            _emailService.Send(forgotPasswordVM.Email, "Reset Link", messageBody) ;

            return RedirectToAction("index", "home");

        }
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordView) 
        {
            if (resetPasswordView.Email == null)
            {
                return RedirectToAction("notfound", "errors");
            }

            AppUser user = await _userManager.FindByEmailAsync(resetPasswordView.Email);
            if (user == null || !(await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPasswordView.Token)))
            {
                return RedirectToAction("login", "account");
            }

            return View(resetPasswordView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordViewModel.Password)||resetPasswordViewModel.Password.Length>25)
            {
                ModelState.AddModelError("Password", "Password zorunlu ve 26 karakteri kecmemeli");
            }

            if(!ModelState.IsValid) { return View("ResetPassword", resetPasswordViewModel); }

            AppUser user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);

            if(user == null) { return RedirectToAction("login", "account"); }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Token, resetPasswordViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View("ResetPassword", resetPasswordViewModel);
            }

            TempData["Success"] = "Şifreniz değiştirildi!";

            return RedirectToAction("login");
        }
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ProfileViewModel profileVM = new ProfileViewModel
            {
                Member = new MemberUpdateViewModel
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    UserName = user.UserName
                },

                Orders = _context.Orders.Include(x => x.OrderItems)
                .ThenInclude(x => x.Product).Where(x => x.IsDeleted == false)
                .Where(x => x.AppUserId == user.Id && user.IsAdmin == false)
                .ToList()
            };
            return View(profileVM);
        }
        [HttpPost]
        //[Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(MemberUpdateViewModel memberVM)
        {
            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Order> orders = _context.Orders.Include(x => x.OrderItems)
               .ThenInclude(x => x.Product).Where(x => x.IsDeleted == false)
               .Where(x => x.AppUserId == member.Id && member.IsAdmin == false)
               .ToList();

            ProfileViewModel profileVM = new ProfileViewModel { Member = memberVM, Orders = orders };

            if (!ModelState.IsValid) { return View(profileVM); }

            if (member.Email != memberVM.Email && _userManager.Users.Any(x => x.NormalizedEmail == memberVM.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "This email has already been taken");
                return View(profileVM);
            }

            if (member.UserName != memberVM.UserName && _userManager.Users.Any(x => x.NormalizedUserName == memberVM.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "This username has already been taken");
                return View(profileVM);
            }

            member.Email = memberVM.Email;
            member.FullName = memberVM.FullName;
            member.UserName = memberVM.UserName;

            IdentityResult result = await _userManager.UpdateAsync(member);

            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(profileVM);
            }

            if (!string.IsNullOrWhiteSpace(memberVM.Password) && !string.IsNullOrWhiteSpace(memberVM.RepeatPassword))
            {
                if (memberVM.Password != memberVM.RepeatPassword)
                {
                    return View(profileVM);
                }

                if (!await _userManager.CheckPasswordAsync(member, memberVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct");
                    return View(profileVM);
                }

                IdentityResult resultpsw = await _userManager.ChangePasswordAsync(member, memberVM.CurrentPassword, memberVM.Password);

                if (!resultpsw.Succeeded)
                {
                    foreach (IdentityError item in resultpsw.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(profileVM);
                }
            }
            
            _context.SaveChanges();

            await _signInManager.SignInAsync(member, true);

            return View(profileVM);
        }
    }
}       
