using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using FileStorage.Models;


namespace FileStorage.Controllers
{
    public class HomeController : Controller
    {
        FileContext db = new FileContext();



        public ActionResult Index()
        {
            return View(db.Files);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Models.File file, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid && uploadFile != null && uploadFile.ContentLength<= 20971520)
            {
                byte[] Data = null;

                var claimsIdentity = User.Identity as ClaimsIdentity;

                if (claimsIdentity != null)
                {
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    if (userIdClaim != null)
                    {
                        var userIdValue = userIdClaim.Value;
                        ViewBag.UserName = User.Identity.Name;
                    }
                }
                    using (var binaryReader = new BinaryReader(uploadFile.InputStream))
                {
                    Data = binaryReader.ReadBytes(uploadFile.ContentLength);
                }
                file.UserName = User.Identity.Name;
                file.FileSize = (Data.Length/1024).ToString()+"KB";
                file.NewFile = Data;
                file.Name = uploadFile.FileName;
                file.Date = (DateTime.Now).ToString();
                file.FileExt = Path.GetExtension(file.Name).ToUpper(); 
                db.Files.Add(file);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(file);
        }

        [Authorize]
        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            byte[] fileData;
            string fileName;
            string fileExt;
            var record = from p in db.Files
                         where p.Id == id
                         select p;

            fileData = (byte[])record.First().NewFile.ToArray();
            fileName = record.First().Name;
            fileExt = record.First().FileExt;
            return File(fileData, fileExt, fileName);

        }
    }
}