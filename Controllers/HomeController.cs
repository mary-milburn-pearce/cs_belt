using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cs_belt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace cs_belt.Controllers
{
    public class HomeController : Controller
    {
        private Context dbContext;

        public HomeController(Context context) 
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(UserViewModel vm)
        {
            User user = vm.regUser;
            if(ModelState.IsValid) {
                User dbUser = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
                if (dbUser != null) {
                    ModelState.AddModelError("regUser.Email", "Email address already registered");
                    return View("Index", vm);
                }
                if (user.Confirm != user.Password) {
                    ModelState.AddModelError("regUser.Password", "Password and confirmation do not match");
                    return View("Index", vm);
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                user.CreatedAt = DateTime.Now;
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetString("UserFirst", user.FirstName);
                HttpContext.Session.SetString("UserLast", user.LastName);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return Redirect("dashboard");
            }
            else {
                return View("Index", vm);
            }
        }

        [Route("login")]
        [HttpGet]
        public IActionResult show_login()
        {
            return View("Index");
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(UserViewModel vm)
        {
            LoginUser lUser = vm.liUser;
            if (ModelState.IsValid) {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == lUser.liEmail);
                if (userInDb == null) {
                    ModelState.AddModelError("liUser.liEmail", "There is no registration for this email address");
                    return View("Index", vm);
                }               
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(lUser, userInDb.Password, lUser.liPassword);
                if (result == 0) {
                    ModelState.AddModelError("liUser.liEmail", "Invalid password");
                    return View("Index", vm);
                }
                HttpContext.Session.SetString("UserFirst", userInDb.FirstName);
                HttpContext.Session.SetString("UserLast", userInDb.LastName);
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);            
                return Redirect("dashboard");
            }
            return View("Index", vm);
        }
         
        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

    }
}
