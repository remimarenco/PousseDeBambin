using PousseDeBambin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PousseDeBambin.ViewModels
{
    public class ListGifts
    {
        public List List { get; set; }
        public Gift SelectedGift { get; set; }
    }
}