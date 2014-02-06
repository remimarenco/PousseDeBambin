using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PousseDeBambin.Models;
using System.Data.Entity.Validation;

namespace PousseDeBambin.Controllers
{
    [Authorize(Roles="Admin")]
    public class ProspectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Prospect/

        public ActionResult Index()
        {
            return View(db.Prospects.ToList());
        }

        //
        // GET: /Prospect/Details/5

        public ActionResult Details(int id = 0)
        {
            Prospect prospect = db.Prospects.Find(id);
            if (prospect == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(prospect);
        }

        //
        // GET: /Prospect/Create
        
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Prospect/Create

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Prospect prospect)
        {
            if (ModelState.IsValid)
            {
                db.Prospects.Add(prospect);

                db.SaveChanges();

                TempData["Success"] = "Merci pour votre commentaire ! Nous ne manquerons pas de vous recontacter ;)";
                return RedirectToAction("Index", "Home", null);
            }

            return View(prospect);
        }

        //
        // GET: /Prospect/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Prospect prospect = db.Prospects.Find(id);
            if (prospect == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(prospect);
        }

        //
        // POST: /Prospect/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Prospect prospect)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prospect).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prospect);
        }

        //
        // GET: /Prospect/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Prospect prospect = db.Prospects.Find(id);
            if (prospect == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(prospect);
        }

        //
        // POST: /Prospect/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prospect prospect = db.Prospects.Find(id);
            db.Prospects.Remove(prospect);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}