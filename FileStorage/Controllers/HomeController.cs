using FileStorage.Models;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileStorage.Controllers
{
    public class HomeController : Controller
    {
        FileContext db = new FileContext();


        public ActionResult Index(string sortOrder, string searchString)
        {

            if (ModelState.IsValid)
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                ViewBag.ExtSortParm = sortOrder == "ext" ? "ext_desc" : "ext";
                ViewBag.UserSortParm = sortOrder == "user" ? "user_desc" : "user";
                ViewBag.SizeSortParm = sortOrder == "size" ? "size_desc" : "size";
                var files = from s in db.Files
                            select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    files = files.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.UserName.ToUpper().Contains(searchString.ToUpper())
                        || s.FileExt.ToUpper().Contains(searchString.ToUpper()
                    ));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        files = files.OrderByDescending(s => s.Name);
                        break;
                    case "Date":
                        files = files.OrderBy(s => s.Date);
                        break;
                    case "date_desc":
                        files = files.OrderByDescending(s => s.Date);
                        break;
                    case "ext_desc":
                        files = files.OrderByDescending(s => s.FileExt);
                        break;
                    case "ext":
                        files = files.OrderBy(s => s.FileExt);
                        break;
                    case "size":
                        files = files.OrderBy(s => s.FileSize);
                        break;
                    case "size_desc":
                        files = files.OrderByDescending(s => s.FileSize);
                        break;
                    case "user":
                        files = files.OrderBy(s => s.UserName);
                        break;
                    case "user_desc":
                        files = files.OrderByDescending(s => s.UserName);
                        break;
                    default:
                        files = files.OrderByDescending(s => s.Date);
                        break;
                }
                return View(files.ToList());
            }

            return View();

        }



        [Authorize]
        [HttpPost]
        public ActionResult Create(Models.File file, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid && uploadFile != null /*&& uploadFile.ContentLength <= 20971520*/)
            {
                byte[] Data = null;

                using (var binaryReader = new BinaryReader(uploadFile.InputStream))
                {
                    Data = binaryReader.ReadBytes(uploadFile.ContentLength);
                }

                var upd = (from s in db.Files
                           where s.Name == uploadFile.FileName
                           select s).FirstOrDefault();

                if (upd != null)
                {

                    upd.Name = uploadFile.FileName;
                    upd.UserName = User.Identity.GetUserName();
                    upd.FileExt = Path.GetExtension(uploadFile.FileName).ToUpper();
                    upd.FileSize = (Data.Length / 1024).ToString() + "Kb";
                    upd.NewFile = Data;
                    upd.Date = (DateTime.Now).ToString();

                    db.SaveChanges();

                }
                else
                {
                    file.UserName = User.Identity.GetUserName();
                    file.FileSize = (Data.Length / 1024).ToString() + "Kb";
                    file.NewFile = Data;
                    file.Name = uploadFile.FileName;
                    file.Date = (DateTime.Now).ToString();
                    file.FileExt = Path.GetExtension(file.Name).ToUpper();
                    db.Files.Add(file);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", file);
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


        [Authorize(Roles = "admin")]
        public ActionResult DeleteFile(int id)
        {

            if (ModelState.IsValid)
            {
                var del = (from c in db.Files
                           where c.Id == id
                           select c).FirstOrDefault();

                db.Files.Remove(del);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

    }
}