using PousseDeBambin.Models;
using PousseDeBambin.Models.Diffbot;
using Postal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace PousseDeBambin.Controllers.API
{
    public class GiftAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string urlDiffBot = "http://api.diffbot.com/v2/product";
        private string tokenDiffBot = "2d5a54f7471c7958ae0c4d3dcdcd5592";

        // GET api/<controller>
        // TODO: Lancer le traitement qui va checker le prix des gifts
        // TODO: Restrict the access to this apiController
        public async Task<bool> Get()
        {
            List<Gift> gifts = new List<Gift>();
            // Get all the databases gift not yet bought and notifs == 1
            gifts = getAllGiftsToCheckPrices(); // TODO: A remettre correctement plus tard
            // gifts.Add(getAllGiftsToCheckPrices()); //TODO : A supprimer plus tard
            // Pass them to getDiffBotPricesResult
            List<Task<Tuple<Gift, double>>> tasks = getDiffBotPricesResult(gifts); // Returns Dictionary with, gift as key, and new price as value
            // Parse all dictionary, update gifts in database and 
            // send email to owner of the list containing the gift 
            // (v2, send only one mail to each owner with all gifts updated
            int resultDb = await updatePrices(tasks);
            
            return true;
        }

        private async Task<int> updatePrices(List<Task<Tuple<Gift, double>>> tasks)
        {
            foreach (var task in await Task.WhenAll(tasks))
            {
                // Si le prix a changé on l'upadate en db et on envoie un mail à la personne
                if (task.Item1.Price != task.Item2)
                {
                    double oldPrice = task.Item1.Price;
                    Gift gift = db.Gifts.Find(task.Item1.GiftId);
                    //Console.Out.WriteLine("On peut mettre à jour l'objet "+gift.Name+", dont le prix était de "+gift.Price+" et maintenant de "+task.Item2);
                    
                    gift.Price = task.Item2;
                    db.Entry(gift).State = EntityState.Modified;

                    // Send email to the owner
                    // TODO : Faire l'appel en asynchrone
                    sendMail(gift, oldPrice, task.Item2);
                }
            }
            // Une fois qu'ils sont tous modifiés, on sauvegarde en bdd
            return db.SaveChanges();
        }

        private void sendMail(Gift gift, double oldPrice, double newPrice )
        {
            //string urlHost = Request.Url.Host;

            dynamic email = new Email("NotfifEmail");

            // Get user's email who is the owner of the list which contains the gift
            email.To = gift.List.UserProfile.EmailAddress;
            email.OldPrice = oldPrice;
            email.NewPrice = newPrice;
            email.ListName = gift.List.Name;
            email.GiftName = gift.Name;
            email.Subject = "[Pousse De Bambin] Un prix a changé dans votre liste "+gift.List.Name+" !";
            //email.UrlHost = urlHost;
            email.Send();
        }

        private List<Gift> getAllGiftsToCheckPrices()
        {
            List<Gift> gifts = db.Gifts.ToList();
            List<Gift> giftsToProcess = new List<Gift>();
            foreach(var gift in gifts)
            {
                GiftState giftState = db.GiftsStates.Find(gift.GiftId);
                if(giftState != null && giftState.State == State.NOT_BOUGHT)
                {
                    giftsToProcess.Add(gift);
                }
            }
            return giftsToProcess;
        }

        private List<Task<Tuple<Gift, double>>> getDiffBotPricesResult(List<Gift> gifts)
        {
            // For each product, we get the price
            // If the price has changed, we put change the gift price in database

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(urlDiffBot);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // For each gift, we call diffbot => need async
            var tasks = new List<Task<Tuple<Gift,double>>>();
            foreach(var gift in gifts)
            {
                // vérifier si ok le call + attribuer le retour à l'objet + vérifier que le prix est modifié ou non
                tasks.Add(callDiffBot(client, gift));
            }
            
            return tasks;
        }

        // TODO: Verifier
        private async Task<Tuple<Gift,double>> callDiffBot(HttpClient client, Gift gift)
        {
            var response = await client.GetAsync("?token="+tokenDiffBot+"&url="+gift.WebSite);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.EnsureSuccessStatusCode(); // Throw on error code.
            
            Result diffBotResponse = await response.Content.ReadAsAsync<Result>();
            
            double newPrice = -1;

            OfferPriceDetails offerPriceD = diffBotResponse.products.First().OfferPriceDetails;
            if(offerPriceD != null)
            {
                newPrice = offerPriceD.Amount;
            }
            
            return Tuple.Create(gift, newPrice);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}