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

        /// <summary>
        /// GET : // 
        /// main route
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet, Route("")]
        public IActionResult Index()
        {
            if (GetId(HttpContext) != null) return RedirectToAction("Index", "Activity");
            ViewData["Message"] = "Login or Register here";
            return View("Index");
        }

        /// <summary>
        /// POST : // register
        /// post request that will register a user to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>IActionResult</returns>
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

        /// <summary>
        /// POST : // login
        /// post request that will login a user using current data
        /// </summary>
        /// <param name="model">json binder to the incoming data to the method</param>
        /// <returns>IActionResult</returns>
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

        /// <summary>
        /// Method that returns the current session id
        /// </summary>
        /// <param name="context">HttpContext. The container of the http headers and session key values</param>
        /// <returns>int? id</returns>
        public int? GetId(HttpContext context)
        {
            int? Id = context.Session.GetInt32("Id");
            return Id;
        }

        /// <summary>
        /// GET : // logout
        /// get request to the logout, clears session
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet, Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
