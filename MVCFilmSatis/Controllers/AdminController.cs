using MVCFilmSatis.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Slider()
        {
            return View(db.Sliders.ToList());
        }
        [HttpGet]
        public ActionResult SliderCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SliderCreate(HttpPostedFileBase imagefile)
        {
            if (imagefile != null && imagefile.ContentLength != 0)
            {
                //exe dosyası değil IIS üzerinden hizmet verdiğimiz için sadece relativepath kullanamıyoruz. MapPath çalışan siteye göre absolute path getirir
                string path = Server.MapPath("/Uploads/Sliders/");
                string thumbpath = path + "thumb/";
                string largepath = path + "large/";

                imagefile.SaveAs(largepath + imagefile.FileName);

                Image i = Image.FromFile(largepath + imagefile.FileName);

                Size s = new Size(380, 100);

                Image small = Helper.ResizeImage(i, s);
                small.Save(thumbpath + imagefile.FileName);

                i.Dispose();

                Slider slider = new Slider();
                //img src içinde göstereceğimiz için relative path kaydediyoruz
                slider.LargeImageURL = "/Uploads/Sliders/large/" + imagefile.FileName;

                slider.ThumbnailURL = "/Uploads/Sliders/thumb/" + imagefile.FileName;

                db.Sliders.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Slider");
            }
          return View();
        }
        
        public ActionResult DeleteSlider(int id)
        {
            Slider s = db.Sliders.Find(id);
            //ana dizinin absolute pathi
            var path = Server.MapPath("/");
            var lg = path + s.LargeImageURL;
            var sm = path + s.ThumbnailURL;

            System.IO.File.Delete(lg);
            System.IO.File.Delete(sm);

            db.Sliders.Remove(s);
            db.SaveChanges();

            return RedirectToAction("Slider");
        }
    }
}