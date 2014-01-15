using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerciLaCigogne.Models;
using MerciLaCigogne.DAL;

namespace MerciLaCigogne.Controllers
{
    public class UsersListsController : Controller
    {
        private MlcDBContext db = new MlcDBContext();

        public ActionResult GetUsersLists(string username)
        {
            UserProfile userProfile = db.UserProfiles.FirstOrDefault(u => u.UserName == username);
            
            return PartialView("_UsersLists", userProfile);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}