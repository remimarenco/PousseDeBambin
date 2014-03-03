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
using Microsoft.AspNet.Identity;
using System.Collections;

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
                return RedirectToAction("NotFound", "Error");
            }
            return View(list);
        }

        public ActionResult Creation()
        {
            return View("Manage");
        }

        public ActionResult Manage(int id)
        {
            List list = db.Lists.Find(id);

            return View(list);
        }

        public ActionResult DisplayGifts(int id)
        {
            var list = db.Lists.Find(id);

            return PartialView("_DisplayGifts", list);
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

        // On récupère la page de création 
        public ActionResult CreatePartialOnlyBirth()
        {
            return PartialView("_CreateOnlyBirth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOnlyBirth(OnlyBirthListViewModel model)
        {
            // On créé la liste temporaire puis on affiche la partial d'ajout de cadeaux
            List list = new List();
            list.Gifts = new List<Gift>();

            try
            {
                list.UserProfile = db.Users.FirstOrDefault(u => u.UserName == "Anonyme");
            }
            catch (DataException dex)
            {
                ViewBag.Error = dex.Message;
            }

            list.Name = "Pas de nom";
            list.Description = "Aucune description";

            // On met la date du jour
            list.BeginningDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                // On ajoute la date de naissance du model
                list.BirthDate = model.BirthDate;

                db.Lists.Add(list);
                db.SaveChanges();
                return RedirectToAction("Manage", new { id = list.ListId });
            }

            return PartialView("_CreateOnlyBirth");
        }

        // Get after the creation of the list
        // TODO: Finir cette fonction => Probleme avec le nom...
        public ActionResult CreatePartTwo(int id = 0, int giftID = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            if (!list.UserProfile.UserName.Equals("Anonyme") && !UserIsAdminOfTheList(list))
            {
                return RedirectToAction("Unauthorized", "Error");
            }


            // Manage a gift passes in parameter
            if (giftID != 0)
            {
                Gift gift = db.Gifts.Find(giftID);
                if (gift.ListID == list.ListId)
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
                return RedirectToAction("NotFound", "Error");
            }
            else
            {
                var list = db.Lists.Find(id);
                if (list == null)
                {
                    return RedirectToAction("NotFound", "Error");
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
                return RedirectToAction("NotFound", "Error");
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
                return RedirectToAction("NotFound", "Error");
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
                && !list.UserProfile.Id.Equals(User.Identity.GetUserId()))
            {
                return RedirectToAction("NotFound", "Error");
            }
            // On associe l'utilisateur authentifié à la liste
            string UserName = User.Identity.GetUserName();
            list.UserProfile = db.Users.FirstOrDefault(u => u.UserName.Equals(UserName));
            //list.UserProfile.Id = User.Identity.GetUserId();
            db.SaveChanges();
            return RedirectToAction("Share", new { id = list.ListId });
        }

        public ActionResult GiftsList(int id = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            if (UserIsAdminOfTheList(list))
            {
                ViewBag.AdminConnected = true;
            }

            return PartialView("_GiftsList", list);
        }

        public ActionResult GiftsListTwo(int listId = 0)
        {
            List list = db.Lists.Find(listId);

            return PartialView("_GiftsList", list);
        }

        [Authorize]
        public ActionResult Share(int id = 0)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(list);
        }

        public ActionResult Consult(string sortOrder, int id = 0)
        {
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "Price_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";

            List list = db.Lists.Find(id);
            if (list == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var orderedGifts = list.Gifts;

            switch (sortOrder)
            {
                case "Name":
                    orderedGifts = orderedGifts.OrderBy(g => g.Name).ToList();
                    break;
                case "Name_desc":
                    orderedGifts = orderedGifts.OrderByDescending(g => g.Name).ToList();
                    break;
                case "Price":
                    orderedGifts = orderedGifts.OrderBy(g => g.Price).ToList();
                    break;
                case "Price_desc":
                    orderedGifts = orderedGifts.OrderByDescending(g => g.Price).ToList();
                    break;
                default:
                    orderedGifts = orderedGifts.OrderBy(g => g.Price).ToList();
                    break;
            }

            list.Gifts = orderedGifts;

            return View(list);
        }

        [ChildActionOnly]
        public ActionResult DisplayListsGifts(List list, string nameSortParm, string priceSortParm)
        {
            ViewBag.NameSortParm = nameSortParm;
            ViewBag.PriceSortParm = priceSortParm;

            if (list == null)
            {
                return RedirectToAction("NotFound", "Error");
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
            IDictionary<string, List<PousseDeBambin.Models.List>> foundedLists = null;

            if (String.IsNullOrWhiteSpace(firstName))
            {
                ModelState.AddModelError("prenom", "Le prénom est obligatoire :)");
            }
            else if(String.IsNullOrWhiteSpace(lastName))
            {
                ModelState.AddModelError("nom", "Le nom est obligatoire :)");
            }
            else
            {
                ViewBag.FirstName = firstName;
                ViewBag.LastName = lastName;

                String firstNameUp = firstName.ToUpper();
                String lastNameUp = lastName.ToUpper();

                foundedLists = db.Users.Where(u => u.FirstName.ToUpper().Equals(firstNameUp))
                    .Where(u => u.LastName.ToUpper().Equals(lastNameUp))
                    .ToDictionary(u => u.Id, u => u.Lists.ToList());
                
                return PartialView("_SearchReturn", foundedLists);

            }

            return View();
        }

        public ActionResult InfosListe(int id)
        {
            List list = db.Lists.Find(id);

            InfosListViewModel infosModel = new InfosListViewModel();
            infosModel.ListId = id;
            infosModel.Name = list.Name;
            infosModel.Description = list.Description;

            if (list == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(infosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InfosListe(InfosListViewModel model)
        {
            // Un formulaire pour renseigner le nom et le prénom de la personne
            if (ModelState.IsValid)
            {
                List list = db.Lists.Find(model.ListId);
                if (list == null)
                {
                    return RedirectToAction("NotFound", "Error");
                }

                list.Name = model.Name;
                list.Description = model.Description;

                db.Entry(list).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Validate", new { id = list.ListId });
            }
            return View(model);
        }

        public ActionResult ValidationModule(int id)
        {
            List list = db.Lists.Find(id);

            if (list != null && list.UserProfile.UserName.Equals("Anonyme") && list.Gifts.Count != 0)
            {
                return PartialView("_ValidationModule", list);
            }

            return Content("");
        }

        private bool UserIsAdminOfTheList(List list)
        {
            // Si l'utilisateur n'est pas connecté
            if (!list.UserProfile.UserName.Equals("Anonyme"))
            {
                // If the user connected is not the same as the list's user
                if (list.UserProfile.Id.Equals(User.Identity.GetUserId()))
                {
                    return true;
                }
                else
                {
                    return false;
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