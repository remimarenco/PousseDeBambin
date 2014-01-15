using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PousseDeBambin.Models.Diffbot
{
    public class Media
    {
        public bool Primary { get; set; }
        public string Link { get; set; }
        public string Caption { get; set; }
        public string Type { get; set; }
        public string Xpath { get; set; }
        

        /*
         * "media": [
        {
          "primary": true,
          "link": "http://g-ecx.images-amazon.com/images/G/08/kindle/dp/2013/KS/ks-slate-01-lg-noVid._V355466431_.jpg",
          "caption": "Kindle Fire HD",
          "type": "image",
          "xpath": "/html[1]/body[1]/div[4]/div[3]/form[1]/table[2]/tbody[1]/tr[3]/td[1]/div[1]/div[1]/img[1]"
        }
         * */
    }
}
