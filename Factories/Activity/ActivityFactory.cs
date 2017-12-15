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

        public Activity.Models.Activity GetLastActivity()
        {
            using (IDbConnection db = Connection)
            {
                string query = "SELECT * FROM activities ORDER BY Id DESC LIMIT 1";
                db.Open();
                return db.Query<Activity.Models.Activity>(query).FirstOrDefault();
            }
        }

        public Activity.Models.Activity GetActivityById(int Id)
        {
            using (IDbConnection db = Connection)
            {
                // string query = $"SELECT * FROM Activities JOIN Participants ON Participants.ActivityId = Activities.Id JOIN Users ON Participants.UserId = Users.Id WHERE Activities.Id = {Id};";
                string cordQuery = $"SELECT * FROM Activities JOIN Users ON Activities.CoordinatorId = Users.Id WHERE Activities.Id = {Id};";
                db.Open();
                var act = db.Query<Activity.Models.Activity, User, Activity.Models.Activity>(cordQuery, (activity, user) =>
                {
                    activity.Coordinator = user;
                    return activity;
                }).FirstOrDefault();

                string partQuery = $"SELECT * FROM participants JOIN users ON users.Id = participants.UserId WHERE participants.ActivityId = {Id};";
                var part = db.Query<User>(partQuery).ToList();
                foreach (var participant in part)
                {
                    if (participant.Id != act.CoordinatorId)
                    {
                        act.Participants.Add(new Participant
                        {
                            User = participant
                        });
                    }
                }
                return act;
            }
        }

        public IEnumerable<Activity.Models.Activity> GetAllActivities()
        {
            using (IDbConnection db = Connection)
            {
                string cordQuery = $"SELECT * FROM Activities JOIN Users ON Activities.CoordinatorId = Users.Id WHERE Activities.ActivityDate >= CURDATE() ORDER BY Activities.ActivityDate ASC;";
                db.Open();
                var act = db.Query<Activity.Models.Activity, User, Activity.Models.Activity>(cordQuery, (activity, user) =>
                {
                    activity.Coordinator = user;
                    return activity;
                });

                string partQuery = $"SELECT * FROM participants JOIN users ON users.Id = participants.UserId;";
                var part = db.Query<Participant, User, Participant>(partQuery, (parti, user) =>
                {
                    parti.User = user;
                    return parti;
                }).ToList();

                foreach (var activity in act)
                {
                    foreach (var parti in part)
                    {
                        if (activity.Id == parti.ActivityId)
                        {
                            activity.Participants.Add(parti);
                        }
                    }
                }
                return act;
            }
        }

        public void DeleteActivityById(int Id)
        {
            using (IDbConnection db = Connection)
            {
                string execute = $"DELETE FROM participants WHERE ActivityId = {Id};DELETE FROM activities WHERE Id = {Id};";
                db.Execute(execute);
            }
        }

        // public IEnumerable<Activity.Models.Activity> GetActivitiesByUserId(int UserId)
        // {
        //     using (IDbConnection db = Connection)
        //     {
        //         string cordQuery = $"SELECT * FROM Activities JOIN Users ON Activities.CoordinatorId = Users.Id WHERE Users.Id = {UserId} ORDER BY Activities.ActivityDate ASC;";
        //         db.Open();
        //         var act = db.Query<Activity.Models.Activity, User, Activity.Models.Activity>(cordQuery, (activity, user) =>
        //         {
        //             activity.Coordinator = user;
        //             return activity;
        //         });

        //         string partQuery = $"SELECT * FROM participants JOIN users ON users.Id = participants.UserId;";
        //         var part = db.Query<Participant, User, Participant>(partQuery, (parti, user) =>
        //         {
        //             parti.User = user;
        //             return parti;
        //         }).ToList();

        //         foreach (var activity in act)
        //         {
        //             foreach (var parti in part)
        //             {
        //                 if (activity.Id == parti.ActivityId)
        //                 {
        //                     activity.Participants.Add(parti);
        //                 }
        //             }
        //         }
        //         return act;
        //     }
        // }
        public User GetActivitiesByUserId(int UserId){
            using(IDbConnection db = Connection){
                User user = db.Query<User>($"SELECT * FROM users WHERE Id = {UserId}").FirstOrDefault();
                if(user != null){
                    var allActivities = GetAllActivities();
                    foreach(var activity in allActivities){
                        foreach(var p in activity.Participants){
                            if(p.UserId == user.Id){
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