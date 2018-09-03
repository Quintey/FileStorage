using FileStorage.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileStorage.Controllers
{
    public class AdminController : Controller
    {

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