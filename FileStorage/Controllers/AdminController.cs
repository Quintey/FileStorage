using FileStorage.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace FileStorage.Controllers
{
    public class AdminController : Controller
    {
        FileContext fdb = new FileContext();
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "admin")]
        public ActionResult Administr()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public string GetData()
        {
            return JsonConvert.SerializeObject(db.Users.ToList());
        }


       

    }
}