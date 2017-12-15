using System.Data;
using Activity.Models;
using Factory;
using Dapper;
using System.Linq;
using LoginReg.Models;
using System.Collections.Generic;
using System;

namespace Activity.Factories
{
    public class ParticipantFactory : SQLConnect, IFactory<Participant>
    {
        /// <summary>
        /// Add a participant
        /// </summary>
        /// <param name="UserId">Id of the participant</param>
        /// <param name="ActivityId">Id of the activity</param>
        public void AddParticipant(int UserId, int ActivityId)
        {
            using (IDbConnection db = Connection)
            {
                string execute = $"INSERT INTO participants (UserId, ActivityId) VALUES ({UserId}, {ActivityId})";
                db.Open();
                if (ParticipationPossible(UserId, ActivityId))
                {
                    db.Execute(execute);
                }
            }
        }

        /// <summary>
        /// Removes a participant
        /// </summary>
        /// <param name="UserId">Id of the user</param>
        /// <param name="ActivityId">Id of the activity</param>
        public void removeParticipant(int UserId, int ActivityId)
        {
            using (IDbConnection db = Connection)
            {
                string execute = $"DELETE FROM participants WHERE UserId = {UserId} AND ActivityId = {ActivityId}";
                db.Open();
                db.Execute(execute);
            }
        }

        /// <summary>
        /// Ignore please!
        /// </summary>
        /// <param name="UserId">Id of the user</param>
        /// <param name="ActivityId">Id of the activity</param>
        /// <returns>Boolean if activity is possible to participate with</returns>
        public bool ParticipationPossible(int UserId, int ActivityId)
        {
            ActivityFactory activityFactory = new ActivityFactory();
            var activity = activityFactory.GetActivityById(ActivityId);
            DateTime endActivityDate = activity.ActivityDate;
            if (activity.Duration.EndsWith("Days"))
            {
                endActivityDate = activity.ActivityDate.AddDays((double)double.Parse(activity.Duration.Substring(0, activity.Duration.IndexOf(' '))));
            }

            if (activity.Duration.EndsWith("Hours"))
            {
                endActivityDate = activity.ActivityDate.AddHours((double)double.Parse(activity.Duration.Substring(0, activity.Duration.IndexOf(' '))));
            }
            if (activity.Duration.EndsWith("Minutes"))
            {
                endActivityDate = activity.ActivityDate.AddMinutes((double)double.Parse(activity.Duration.Substring(0, activity.Duration.IndexOf(' '))));
            }
            if (activity.Duration.EndsWith("Seconds"))
            {
                endActivityDate = activity.ActivityDate.AddSeconds((double)double.Parse(activity.Duration.Substring(0, activity.Duration.IndexOf(' '))));
            }
            var usersCurrentActivities = activityFactory.GetActivitiesByUserId(UserId);

            foreach (var userActivity in usersCurrentActivities.Activities)
            {
                DateTime givenDate = userActivity.ActivityDate;
                DateTime EndDate = givenDate;
                if (userActivity.Duration.EndsWith("Days"))
                {
                    EndDate = givenDate.AddDays((double)double.Parse(userActivity.Duration.Substring(0, userActivity.Duration.IndexOf(' '))));
                }

                if (userActivity.Duration.EndsWith("Hours"))
                {
                    EndDate = givenDate.AddHours((double)double.Parse(userActivity.Duration.Substring(0, userActivity.Duration.IndexOf(' '))));
                }
                if (userActivity.Duration.EndsWith("Minutes"))
                {
                    EndDate = givenDate.AddMinutes((double)double.Parse(userActivity.Duration.Substring(0, userActivity.Duration.IndexOf(' '))));
                }
                if (userActivity.Duration.EndsWith("Seconds"))
                {
                    EndDate = givenDate.AddSeconds((double)double.Parse(userActivity.Duration.Substring(0, userActivity.Duration.IndexOf(' '))));
                }
                if (givenDate <= activity.ActivityDate && activity.ActivityDate <= EndDate)
                {
                    return false;
                }
                if (givenDate <= endActivityDate && endActivityDate <= EndDate)
                {
                    return false;
                }

            }
            return true;
        }
    }
}