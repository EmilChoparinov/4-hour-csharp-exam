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
using Activity.Models;
using Activity.Factories;
namespace Activity.Controllers
{
    public class ActivityController : Controller
    {
        private readonly UserFactory _userFactory;
        private readonly ActivityFactory _activityFactory;
        private readonly ParticipantFactory _participantFactory;
        public ActivityController()
        {
            _userFactory = new UserFactory();
            _activityFactory = new ActivityFactory();
            _participantFactory = new ParticipantFactory();
        }

        [HttpGet, Route("home")]
        public IActionResult Index(){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            ViewData["Message"] = "Welcome!";
            ViewBag.User = _userFactory.GetUserByID((int) GetId(HttpContext));
            ViewBag.Activities = _activityFactory.GetAllActivities();
            return View("Index");
        }

        [HttpGet, Route("new-activity")]
        public IActionResult NewActivity(){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            ViewData["Message"] = "Create an Activity!";
            return View("NewActivity");
        }

        [HttpPost, Route("new-activity-db")]
        public IActionResult NewActivityDB(ActivityViewModel model){
            if(!ModelState.IsValid){
                ViewData["Message"] = "Something went wrong, please try again";
                return View("NewActivity");
            }
            _activityFactory.AddActivity(model, (int) GetId(HttpContext));
            var activity = _activityFactory.GetLastActivity();
            return Redirect($"activity/{activity.Id}");
        }

        [HttpGet, Route("activity/{Id}")]
        public IActionResult ViewActivity(int Id){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            ViewBag.Activity = _activityFactory.GetActivityById(Id);
            ViewBag.User = _userFactory.GetUserByID((int) GetId(HttpContext));
            return View("ViewActivity");
        }

        [HttpGet, Route("delete/{Id}")]
        public IActionResult DeleteActivity(int Id){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            var act = _activityFactory.GetActivityById(Id);
            if(act.CoordinatorId == (int) GetId(HttpContext)){
                _activityFactory.DeleteActivityById(Id);
            }
            return RedirectToAction("Index");
        }

        [HttpGet, Route("join/{Id}")]
        public IActionResult JoinActivity(int Id){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            _participantFactory.AddParticipant((int)GetId(HttpContext), Id);
            return RedirectToAction("Index");
        }

        [HttpGet, Route("leave/{Id}")]
        public IActionResult LeaveActivity(int Id){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            _participantFactory.removeParticipant((int) GetId(HttpContext), Id);
            return RedirectToAction("Index");
        }
        public int? GetId(HttpContext context)
        {
            int? Id = context.Session.GetInt32("Id");
            return Id;
        }
    }
}