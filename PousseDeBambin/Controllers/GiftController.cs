using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PousseDeBambin.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using PousseDeBambin.ViewModels;

namespace PousseDeBambin.Controllers
{
    public class GiftController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Gift/

        public ActionResult Index()
        {
            return View(db.Gifts.ToList());
        }

        //
        // GET: /Gift/Details/5

        public ActionResult Details(int id = 0)
        {
            Gift gift = db.Gifts.Find(id);
            if (gift == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(gift);
        }

        //
        // GET: /Gift/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Gift/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Gift gift)
        {
            if (ModelState.IsValid)
            {
                db.Gifts.Add(gift);
                db.SaveChanges();
                return RedirectToAction("CreatePartTwo", "List", gift.ListID);
            }

            return View(gift);
        }

        public ActionResult CreatePartial(int listID = 0)
        {
            Gift gift = new Gift { ListID = listID };
            return PartialView("_CreateGift", gift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePartial(Gift gift)
        {
            if (ModelState.IsValid)
            {
                db.Gifts.Add(gift);
                db.SaveChanges();

                ViewBag.Success = "L'objet a correctement été ajouté !";
                return RedirectToAction("CreatePartial", new { listID = gift.ListID });
            }
            return PartialView("_CreateGift", gift);
        }

        public ActionResult CreatePartialTwo(int listID = 0)
        {
            Gift gift = new Gift { ListID = listID };
            return PartialView("_CreateGiftTwo", gift);
        }

        [HttpPost]
        public ActionResult CreatePartialTwo(Gift gift)
        {
            if (ModelState.IsValid)
            {
                db.Gifts.Add(gift);
                db.SaveChanges();

                ViewBag.Success = "L'objet a correctement été ajouté !";
                ModelState.Clear();
            }
            return PartialView("_CreateGiftTwo", gift);
        }

        //
        // GET: /Gift/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Gift gift = db.Gifts.Find(id);
            if (gift == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(gift);
        }

        //
        // POST: /Gift/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Gift gift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gift);
        }

        public ActionResult EditPartial(int giftId = 0)
        {
            Gift gift = db.Gifts.Find(giftId);
            if (gift == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return PartialView("_EditPartial", gift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartial(Gift gift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gift).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_EditPartial", gift);
            }

            ViewBag.Success = "L'objet a correctement été modifié !";
            return PartialView("_EditPartial", gift);
        }

        //
        // GET: /Gift/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Gift gift = db.Gifts.Find(id);
            if (gift == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(gift);
        }

        //
        // POST: /Gift/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gift gift = db.Gifts.Find(id);
            db.Gifts.Remove(gift);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DeletePartial(int id)
        {
            Gift gift = db.Gifts.Find(id);

            // If the gift has a giftState related, (Todo: ask if the user really wants to delete it) delete also the giftState
            GiftState giftState = db.GiftsStates.Find(id);
            if (giftState != null)
            {
                db.GiftsStates.Remove(giftState);
            }

            db.Gifts.Remove(gift);
            db.SaveChanges();

            ViewBag.Success = "L'objet a correctement été supprimé !";
        }

        /* ------ JQuery Grid ------ */
        [HttpPost]
        public ActionResult UpdateGift(Gift gift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gift).State = EntityState.Modified;
                db.SaveChanges();
                return Json("true");
            }
            return Json("false");
        }

        /*------------ KENDO ---------------*/
        public ActionResult Gifts_Read([DataSourceRequest] DataSourceRequest request, int listId)
        {
            var gifts = db.Lists.Find(listId).Gifts;
            DataSourceResult result = gifts.ToDataSourceResult(request, g => new GiftViewModel()
            {
                Name = g.Name,
                Description = g.Description,
                GiftId = g.GiftId,
                ImageUrl = g.ImageUrl,
                Price = g.Price,
                WebSite = g.WebSite,
                ListID = g.ListID
            });
            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Gifts_Create([DataSourceRequest] DataSourceRequest request, Gift gift)
        {
            if (gift != null && ModelState.IsValid)
            {
                db.Lists.Find(gift.ListID).Gifts.Add(gift);
            }

            return Json(new[] { gift }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Gifts_Update([DataSourceRequest] DataSourceRequest request, Gift gift)
        {
            // Revoir l'édition
            if (gift != null && ModelState.IsValid)
            {
                var target = db.Gifts.Find(gift.GiftId);
                if (target != null)
                {
                    db.Entry(target).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Gifts_Destroy([DataSourceRequest] DataSourceRequest request, Gift gift)
        {
            Gift giftToDelete = db.Gifts.Find(gift.GiftId);
            if (giftToDelete != null)
            {
                // On supprime d'abord son état
                // TODO: Warning si objet acheté
                
                db.Gifts.Remove(giftToDelete);
                db.SaveChanges();
            }

            return Json(ModelState.ToDataSourceResult());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}