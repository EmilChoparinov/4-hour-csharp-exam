using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.DbConnection;
using LoginReg.Models;
using LoginReg.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
namespace LoginReg.Controllers
{
    public class LoginRegController : Controller
    {
        private readonly UserFactory _userFactory;
        public LoginRegController()
        {
            _userFactory = new UserFactory();
        }
        [HttpGet, Route("")]
        public IActionResult Index()
        {
            if (GetId(HttpContext) != null) return RedirectToAction("Index", "Activity");
            ViewData["Message"] = "Login or Register here";
            return View("Index");
        }

        [HttpPost, Route("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Something went wrong, please try again";
                return View("Index");
            }
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            Models.User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };
            user.Password = hasher.HashPassword(user, model.Password);
            _userFactory.AddUser(user);
            HttpContext.Session.SetInt32("Id", _userFactory.GetUserByEmail(model.Email).Id);
            return RedirectToAction("Index");
        }

        [HttpPost, Route("login")]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Something went wrong, please try again";
                return View("Index");
            }
            HttpContext.Session.SetInt32("Id", _userFactory.GetUserByEmail(model.LogInfo.Email).Id);
            return RedirectToAction("Index");
        }
        public int? GetId(HttpContext context)
        {
            int? Id = context.Session.GetInt32("Id");
            return Id;
        }

        [HttpGet, Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
