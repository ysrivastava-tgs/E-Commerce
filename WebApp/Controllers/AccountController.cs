using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _securityManager;
        private readonly SignInManager<ApplicationUser> _loginManager;
        //2
        public AccountController(UserManager<ApplicationUser> secMgr, SignInManager<ApplicationUser> loginManager)
        {
            _securityManager = secMgr;
            _loginManager = loginManager;
           
        }
        public string AccessDenied(string url)
        {
            return "Access Denied";
        }
        //3
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        //4

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            using (var context = new AppDbContext())
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Name,
                        Email = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.Phone
                    };
                    var result = await _securityManager.CreateAsync(user, model.Password);

                    /* var roleStore = new RoleStore<IdentityRole>(context);
                     var roleManager = new RoleManager<IdentityRole>(roleStore);
                    var roleStore = new RoleStore<IdentityRole>(context);
                 var roleManager = new RoleManager<IdentityRole>(roleStore);

                 var userStore = new UserStore<ApplicationUser>(context);
                 var userManager = new UserManager<ApplicationUser>(userStore);
                     var userStore = new UserStore<ApplicationUser>(context);
                     var userManager = new UserManager<ApplicationUser>(userStore);*/

                    var identity_result = result.Errors.ToList();
                   
                    if (identity_result.Count > 0)
                    {
                        ViewBag.Error = identity_result;
                        return View();
                    }

                    if (result.Succeeded)
                    {
                        await _securityManager.AddToRoleAsync(user, "Admin");
                        Console.WriteLine("Hello world");
                        await _loginManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction(nameof(LaptopController.Index), "Laptop");

                    }
                 

                }
            }

                return View(model);
        }

        //5
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("Login");
        }

        //6
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _loginManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                   // var user = await this._securityManager.GetUserAsync(User);
                    
                    return RedirectToReturnUrl(returnUrl);
                }
                if(!result.Succeeded)
                {
                    ViewBag.Failed = "Invalid Credentials";
                    return View();
                }
             

            }


            return View(model);
        }

        //7
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _loginManager.SignOutAsync();

            return RedirectToAction(nameof(LaptopController.Index), "Laptop");
        }
        //8
        private IActionResult RedirectToReturnUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(LaptopController.Index), "Laptop");
            }
        }
    }
}
