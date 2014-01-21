using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PousseDeBambin.Models;

namespace PousseDeBambin.Controllers
{
    public class UsersListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetUsersLists(string username)
        {
            ApplicationUser userProfile = db.Users.FirstOrDefault(u => u.UserName == username);
            
            return PartialView("_UsersLists", userProfile);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}