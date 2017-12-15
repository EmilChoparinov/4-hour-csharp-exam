using System.Data;
using Activity.Models;
using Factory;
using Dapper;
using System.Linq;
using LoginReg.Models;
using System.Collections.Generic;

namespace Activity.Factories
{
    public class ActivityFactory : SQLConnect, IFactory<Activity.Models.Activity>
    {
        /// <summary>
        /// adds an activity to the MySQL database
        /// </summary>
        /// <param name="model">incomming json binder</param>
        /// <param name="coordinator">user id of the incomming coordinator</param>
        public void AddActivity(ActivityViewModel model, int coordinator)
        {
            var activity = new Activity.Models.Activity
            {
                ActivityName = model.ActivityName,
                Description = model.Description,
                ActivityDate = model.DateValues.Date.Add(model.DateValues.Time.TimeOfDay),
                Duration = $"{model.DurationInt} {model.TimeType}",
                CoordinatorId = coordinator
            };
            using (IDbConnection db = Connection)
            {
                string query = "INSERT INTO activities (ActivityName, Description, ActivityDate, Duration, CoordinatorId) VALUES (@ActivityName, @Description, @ActivityDate, @Duration, @CoordinatorId)";
                db.Open();
                db.Execute(query, activity);
            }
        }

        /// <summary>
        /// Gets the last activity added to the database
        /// </summary>
        /// <returns>Activity.Models.Activity</returns>
        public Activity.Models.Activity GetLastActivity()
        {
            using (IDbConnection db = Connection)
            {
                string query = "SELECT * FROM activities ORDER BY Id DESC LIMIT 1";
                db.Open();
                return db.Query<Activity.Models.Activity>(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets an activity by id
        /// </summary>
        /// <param name="Id">Id of requested activity</param>
        /// <returns>Activity.Models.Activity</returns>
        public Activity.Models.Activity GetActivityById(int Id)
        {
            using (IDbConnection db = Connection)
            {
                //Joins the Users table to the Activities table on the coordinators Id
                //End result is the information of the coordinator to the database on one activity
                string cordQuery = $"SELECT * FROM Activities JOIN Users ON Activities.CoordinatorId = Users.Id WHERE Activities.Id = {Id};";
                db.Open();
                var act = db.Query<Activity.Models.Activity, User, Activity.Models.Activity>(cordQuery, (activity, user) =>
                {
                    activity.Coordinator = user;
                    return activity;
                }).FirstOrDefault();

                //Joins the Users table to the Participants table on the users id
                //End result is the information of the users on the participants table where
                //the only participants involved are the ones who have the activity of the given Id
                string partQuery = $"SELECT * FROM participants JOIN users ON users.Id = participants.UserId WHERE participants.ActivityId = {Id};";

                //the reason why we return a User and not a Participant is that we do not need that 
                //data besides the selection. we do later need to instantiate a Participante to 
                //wrap the user but to avoid a complex query we can just ignore that data for now
                var part = db.Query<User>(partQuery).ToList();

                //iterate through all the participants from the query
                foreach (var participant in part)
                {
                    //only ever add a participant to Participants list if
                    //the id is not that of the coordinator
                    //used to be a problem but is most likely safe to remove this
                    if (participant.Id != act.CoordinatorId)
                    {
                        //add the participant to the list
                        //since the Participants list requires
                        //a Participants object we wrap our User Object
                        // in the a Participant object 
                        act.Participants.Add(new Participant
                        {
                            User = participant
                        });
                    }
                }
                return act;
            }
        }

        /// <summary>
        /// Grabs all activities in the database and presents it
        /// </summary>
        /// <returns>IEnumerable of Activity.Models.Activity</returns>
        public IEnumerable<Activity.Models.Activity> GetAllActivities()
        {
            using (IDbConnection db = Connection)
            {
                //Joins the Users table the the Activities table on the Coordinators Id
                //It is then ordered by ascending date order to show oldest first (when taking into
                //account the WHERE clause the older will be today) 
                //Objective of this is to get the coordinators data to come with the activity data
                string cordQuery = $"SELECT * FROM Activities JOIN Users ON Activities.CoordinatorId = Users.Id WHERE Activities.ActivityDate >= CURDATE() ORDER BY Activities.ActivityDate ASC;";
                db.Open();
                var act = db.Query<Activity.Models.Activity, User, Activity.Models.Activity>(cordQuery, (activity, user) =>
                {
                    activity.Coordinator = user;
                    return activity;
                });

                //Join the Users table to the Participants table on the users Id
                //Objective of this is to get all user info on the participants data to ready to add to the last statement
                string partQuery = $"SELECT * FROM participants JOIN users ON users.Id = participants.UserId;";
                var part = db.Query<Participant, User, Participant>(partQuery, (parti, user) =>
                {
                    parti.User = user;
                    return parti;
                }).ToList();

                //iterate through every activity taken
                foreach (var activity in act)
                {
                    //iterate throught the entire participants table
                    foreach (var parti in part)
                    {
                        //whenever the current activities id is the same as the participants id,
                        //that means that that User is going to the even
                        if (activity.Id == parti.ActivityId)
                        {
                            //add them to the list of participants for this activity
                            activity.Participants.Add(parti);
                        }
                    }
                }
                return act;
            }
        }

        /// <summary>
        /// Removes an activity by the given Id
        /// </summary>
        /// <param name="Id">Id of the activity requested to be removed</param>
        public void DeleteActivityById(int Id)
        {
            using (IDbConnection db = Connection)
            {
                string execute = $"DELETE FROM participants WHERE ActivityId = {Id};DELETE FROM activities WHERE Id = {Id};";
                db.Execute(execute);
            }
        }

        /// <summary>
        /// Gets all activities based by the given id
        /// </summary>
        /// <param name="UserId">Id of the user</param>
        /// <returns>User</returns>
        public User GetActivitiesByUserId(int UserId){
            using(IDbConnection db = Connection){
                User user = db.Query<User>($"SELECT * FROM users WHERE Id = {UserId}").FirstOrDefault();
                if(user != null){
                    //since there is no foriegn key in the Users table, we must reverse search
                    var allActivities = GetAllActivities();
                    
                    //iterate through all the activities
                    foreach(var activity in allActivities){

                        //iterate through all the participants in the activity 
                        foreach(var p in activity.Participants){

                            //if that participant is that user, then that means the user is going
                            //to this event
                            if(p.UserId == user.Id){

                                //add the user to the users Activies list
                                user.Activities.Add(activity);
                            }
                        }
                    }
                }
                return user;
            }
        }
    }
}