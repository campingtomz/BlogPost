using blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blog.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string GetDisplayName(string userId)
        {
            var user = db.Users.Find(userId);
            return user.DisplayName;
        }
        //public string GetFullName(string userId)
        //{

        //}
        public string GetFirstName(string userId)
        {
            var user = db.Users.Find(userId);
            return user.FirstName;
        }
        public string GetLastName(string userId)
        {
            var user = db.Users.Find(userId);
            return user.LastName;
        }
        public string GetFullName(string userId)
        {
            var firstName =  db.Users.Find(userId).FirstName;
            var LastName = db.Users.Find(userId).LastName;
            var fullName = firstName + " " + LastName;
            return fullName;
        }
    }
}