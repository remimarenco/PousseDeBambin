using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PousseDeBambin.Models;
using Postal;
using System.Threading.Tasks;

namespace MerciLaCigogne.Controllers
{
    public class GiftStateController : AsyncController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult DisplayGiftState(int id = 0)
        {
            GiftState giftState = db.GiftsStates.Find(id);
            // If we do not find it, and it is not id 0, we add it in the giftState db
            if (giftState == null && id != 0)
            {
                giftState = db.GiftsStates.Add(new GiftState
                {
                    GiftID = id,
                    State = State.NOT_BOUGHT,
                    BuyerName = "Anonyme",
                });
                giftState.Gift = db.Gifts.FirstOrDefault(g => g.GiftId == giftState.GiftID);
                db.SaveChanges();
            }
            else if (id == 0)
            {
                return HttpNotFound();
            }

            if (giftState.State == State.NOT_BOUGHT)
            {
                ViewBag.Bought = false;
            }
            else
            {
                ViewBag.Bought = true;
            }

            return PartialView("_DisplayGiftState", giftState);
        }

        [HttpPost]
        public int ObjectBought(string buyerName, string buyerText, int id = 0)
        {
            GiftState giftState = db.GiftsStates.Find(id);

            if (buyerName.Equals(String.Empty))
            {
                buyerName = "Anonyme";
            }

            if (giftState == null && id != 0)
            {
                giftState = db.GiftsStates.Add(new GiftState
                {
                    GiftID = id,
                    State = State.BOUGHT,
                    BuyerName = buyerName,
                });
                db.SaveChanges();
            }
            else
            {
                giftState.BuyerName = buyerName;
                giftState.BuyerText = buyerText;
                giftState.State = State.BOUGHT;
                db.Entry(giftState).State = EntityState.Modified;
                db.SaveChanges();
            }

            //// Send email to list's owner
            sendMail(giftState, buyerName, buyerText);


            return giftState.GiftID;
        }

        private void sendMail(GiftState giftState, String buyerName, String buyerText)
        {
            // We get first the informations about the owner of the list : EmailAddress, FirstName, listName, giftName
            string giftName = giftState.Gift.Name;
            string listName = giftState.Gift.List.Name;
            int listId = giftState.Gift.List.ListId;
            string firstName = giftState.Gift.List.UserProfile.FirstName;
            string emailAddress = giftState.Gift.List.UserProfile.EmailAddress;

            // Then we send him the email
            dynamic email = new Email("GiftBought");
            email.To = emailAddress;
            email.Subject = "[MerciLaCigogne] " + buyerName + " vient d'acheter un objet !";
            email.BuyerName = buyerName;
            email.FirstName = firstName;
            email.GiftName = giftName;
            email.ListName = listName;
            email.Message = buyerText;
            email.ListId = listId;
            email.UrlList = Url.Action("Consult", "List", new { Id = listId }, Request.Url.Scheme);
            Task.Run(() => { email.Send(); });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}