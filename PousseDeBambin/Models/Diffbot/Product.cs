using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PousseDeBambin.Models.Diffbot
{
    public class Product
    {
        public string Title { get; set; }
        public string  Description { get; set; }
        public string OfferPrice { get; set; }
        public OfferPriceDetails OfferPriceDetails { get; set; }
        public IEnumerable<Media> Media { get; set; }
        public int ProductId { get; set; }
        public bool Availability { get; set; }
        
        /* Exemple de contenu de products
         * "products": [
    {
      "title": "Kindle Fire HD 7\" (17 cm)",
      "description": "Un écran HD stupéfiant (1280 x 800) de 216 ppp et un processeur double cœur rapide de 1,5 GHz\nFacile à utiliser : touchez une seule fois l'écran pour commencer à lire, écouter, naviguer ou jouer\nParfait pour les enfants et les parents grâce au large choix de livres, jeux et applications pour enfants ainsi que le contrôle parental facile d'utilisation\nNavigation web ultra-rapide via la connexion Wi-Fi intégrée, et prise en charge des solutions de messagerie et calendrier telles que Gmail, Outlook et plus encore\nLéger et durable : conçu pour résister aux chocs et aux rayures accidentels grâce à son écran 20 fois plus solide et 30 fois plus dur que le plastique\nStockage gratuit et illimité dans le Cloud pour tous vos contenus Amazon",
      "offerPrice": "EUR139,00",
      "offerPriceDetails": {
        "amount": 139,
        "text": "EUR139,00",
        "symbol": "EUR"
      },
      "media": [
        {
          "primary": true,
          "link": "http://g-ecx.images-amazon.com/images/G/08/kindle/dp/2013/KS/ks-slate-01-lg-noVid._V355466431_.jpg",
          "caption": "Kindle Fire HD",
          "type": "image",
          "xpath": "/html[1]/body[1]/div[4]/div[3]/form[1]/table[2]/tbody[1]/tr[3]/td[1]/div[1]/div[1]/img[1]"
        }
      ],
      "productId": "405320",
      "availability": true
    }
  ],*/
    }
}