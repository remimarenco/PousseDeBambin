using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PousseDeBambin.Models;

namespace PousseDeBambin.Controllers
{
    public class ComingSoonController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /ComingSoon/

        public ActionResult Index()
        {
            return View();
        }

        // POST: /ComingSoon/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Prospect prospect)
        {
            if (ModelState.IsValid)
            {
                db.Prospects.Add(prospect);
                db.SaveChanges();
            }

            return View(prospect);
        }

    }
}
