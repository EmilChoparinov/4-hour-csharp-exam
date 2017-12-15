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

        /// <summary>
        /// GET : // home 
        /// route for main page rendering
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet, Route("home")]
        public IActionResult Index(){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            ViewData["Message"] = "Welcome!";
            ViewBag.User = _userFactory.GetUserByID((int) GetId(HttpContext));
            ViewBag.Activities = _activityFactory.GetAllActivities();
            return View("Index");
        }

        /// <summary>
        /// GET : // new-activity
        /// route for rending the page to see the form for adding a new activity
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet, Route("new-activity")]
        public IActionResult NewActivity(){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            ViewData["Message"] = "Create an Activity!";
            return View("NewActivity");
        }

        /// <summary>
        /// POST : // new-activity-db
        /// route for adding new activity data to MySQL database
        /// </summary>
        /// <param name="model">json binder of the incomming data to the method</param>
        /// <returns>IActionResult</returns>
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

        /// <summary>
        /// GET : // activity/{Id}
        /// route for displaying a specified Activity Id
        /// </summary>
        /// <param name="Id">Id of an existing activity</param>
        /// <returns>IActionResult</returns>
        [HttpGet, Route("activity/{Id}")]
        public IActionResult ViewActivity(int Id){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            ViewBag.Activity = _activityFactory.GetActivityById(Id);
            ViewBag.User = _userFactory.GetUserByID((int) GetId(HttpContext));
            return View("ViewActivity");
        }

        /// <summary>
        /// GET : // delete/{Id}
        /// get request for deleting an activity. must be of the sessions owner
        /// </summary>
        /// <param name="Id">Id of Activity to delete</param>
        /// <returns>IActionResult</returns>
        [HttpGet, Route("delete/{Id}")]
        public IActionResult DeleteActivity(int Id){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            var act = _activityFactory.GetActivityById(Id);
            if(act.CoordinatorId == (int) GetId(HttpContext)){
                _activityFactory.DeleteActivityById(Id);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET : // join/{Id}
        /// get request to join an activity. the one being joined is of session
        /// </summary>
        /// <param name="Id">Id of Activity to join</param>
        /// <returns>IActionResult</returns>
        [HttpGet, Route("join/{Id}")]
        public IActionResult JoinActivity(int Id){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            _participantFactory.AddParticipant((int)GetId(HttpContext), Id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET : //leave{Id}
        /// get request to leave an activity. session is the user
        /// </summary>
        /// <param name="Id">Id of activity to leave</param>
        /// <returns>IActionResult</returns>
        [HttpGet, Route("leave/{Id}")]
        public IActionResult LeaveActivity(int Id){
            if (GetId(HttpContext) == null) return RedirectToAction("Index", "LoginReg");
            _participantFactory.removeParticipant((int) GetId(HttpContext), Id);
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
    }
}