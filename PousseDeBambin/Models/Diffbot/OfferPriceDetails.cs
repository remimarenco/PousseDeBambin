using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PousseDeBambin.Models.Diffbot
{
    public class OfferPriceDetails
    {
        public double Amount { get; set; }
        public string Text { get; set; }
        public string  Symbol { get; set; }
        
        /*
         * "offerPriceDetails": {
        "amount": 139,
        "text": "EUR139,00",
        "symbol": "EUR"
      },*/
    }
}