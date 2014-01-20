using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PousseDeBambin.Models;
using PousseDeBambin.ViewModels;
using System.Web.Security;
using System.Net;

namespace PousseDeBambin.Controllers
{
    public class ListController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /List/

        public ActionResult Index(string searchString)
        {
            var lists = from s in db.Lists
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                lists = lists.Where(s => s.UserProfile.UserName.ToUpper().Contains(searchString.ToUpper()));
            }
            return View(lists.ToList());
        }

        //
        // GET: /List/Details/5

        public ActionResult Details(int id = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        //
        // GET: /List/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /List/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List list)
        {
            try
            {
                list.UserProfile = db.Users.FirstOrDefault(u => u.UserName == "Anonyme");
            }
            catch (DataException dex)
            {
                ViewBag.Error = dex.Message;
            }

            TryUpdateModel(list);

            // On met la date du jour
            list.BeginningDate = DateTime.Now;
            TryUpdateModel(list);

            if (ModelState.IsValid)
            {
                db.Lists.Add(list);
                db.SaveChanges();
                return RedirectToAction("CreatePartTwo", new { id = list.ListId });
            }

            return View(list);
        }

        // Get after the creation of the list
        // TODO: Finir cette fonction => Probleme avec le nom...
        public ActionResult CreatePartTwo(int id = 0, int giftID = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound("La liste n'a pas été trouvée");
            }

            if (!list.UserProfile.UserName.Equals("Anonyme"))
            {
                if (!UserIsAdminOfTheList(list))
                {
                    return HttpNotFound("La liste ne vous appartient pas");
                }
            }
            

            // Manage a gift passes in parameter
            if (giftID != 0)
            {
                Gift gift = db.Gifts.Find(giftID);
                if(gift.ListID == list.ListId)
                {
                    ViewBag.GiftId = gift.GiftId;
                } 
            }

            return View(list);

            

        }

        // TODO: CreatePartTwo POST is only accessed on submit
        [HttpPost]
        public ActionResult CreatePartTwo(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var list = db.Lists.Find(id);
                if (list == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        if (!Request.IsAuthenticated)
                        {
                            FormsAuthentication.RedirectToLoginPage();
                        }
                        return RedirectToAction("Share", new { id = list.ListId });
                    }
                    return RedirectToAction("CreatePartTwo", id);
                }
            }
        }

        //
        // GET: /List/Edit/5

        public ActionResult Edit(int id = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        //
        // POST: /List/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List list)
        {
            if (ModelState.IsValid)
            {
                db.Entry(list).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(list);
        }

        //
        // GET: /List/Delete/5

        public ActionResult Delete(int id = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        //
        // POST: /List/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List list = db.Lists.Find(id);
            db.Lists.Remove(list);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Validate(int id = 0)
        {
            List list = db.Lists.Find(id);
            // On vérifie que cette liste n'appartient pas déjà à quelqu'un
            if (list.UserProfile.Id != db.Users.FirstOrDefault(u => u.UserName.Equals("Anonyme")).Id
                && !list.UserProfile.Id.Equals(Membership.GetUser().ProviderUserKey.ToString()))
            {
                return HttpNotFound("La liste ne vous appartient pas");
            }
            // On associe l'utilisateur authentifié à la liste
            list.UserProfile.Id = Membership.GetUser().ProviderUserKey.ToString();
            db.SaveChanges();
            return RedirectToAction("Share", new { id = list.ListId });
        }

        public ActionResult GiftsList(int id = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }

            if(UserIsAdminOfTheList(list))
            {
                ViewBag.AdminConnected = true;
            }

            return PartialView("_GiftsList", list);
        }

        [Authorize]
        public ActionResult Share(int id = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        public ActionResult Consult(int id = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound("Liste non trouvée");
            }
            return View(list);
        }

        [ChildActionOnly]
        public ActionResult DisplayListsGifts(int id = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound("Liste non trouvée");
            }
            return PartialView("_DisplayListsGifts", list);
        }

        public ActionResult Search()
        {
            // Un formulaire pour renseigner le nom et le prénom de la personne
            return View();
        }

        [HttpPost]
        public ActionResult Search(string firstName, string lastName)
        {
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;

            List<PousseDeBambin.Models.List> foundedLists = db.Lists.Where(l =>
                l.UserProfile.FirstName.ToUpper().Equals(firstName.ToUpper())).ToList();

            return View(foundedLists);
        }

        private bool UserIsAdminOfTheList(List list)
        {
            if (!list.UserProfile.UserName.Equals("Anonyme"))
            {
                // If the user connected is not the same as the list's user
                MembershipUser user = Membership.GetUser(User.Identity.Name);
                if (user == null || user.ProviderUserKey.ToString().Equals(list.UserProfile.Id))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}