﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerciLaCigogne.Models;
using MerciLaCigogne.DAL;

namespace MerciLaCigogne.Controllers
{
    public class ListAndGiftsController : Controller
    {
        private MlcDBContext db = new MlcDBContext();
        
        public ActionResult ValidateList(List list)
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}