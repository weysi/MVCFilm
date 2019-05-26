using Microsoft.AspNet.Identity;
using MVCFilmSatis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page, string error)
        {
            ViewBag.Error = error;

            //Kişi giriş yaptıysa
            if (User.Identity.IsAuthenticated)
            {
                //Giriş yapmış kişinin Id si
                //GetUserId -> Microsoft.AspNet.Identity
                string uid = User.Identity.GetUserId();

                //kişinin bütün bilgileri (sepet dahil)
                Customer loggedInUser = db.Users.Find(uid);

                //kişinin sepeti yoksa 0 film var
                if (loggedInUser.ShoppingCart == null)
                    ViewBag.CartMovieCount = 0;
                else
                { //kişinin sepeti varsa 
                    var l =loggedInUser.ShoppingCart.Movies;
                    // l = sepetteki filmler
                    //liste oluşmadıysa 0 oluştuysa listedeki eleman sayısı
                    var c = l == null ? 0 : l.Count;
                    ViewBag.CartMovieCount = c;
                }
            }


            List<Movie> list = new List<Movie>();
            if (page.HasValue)
            {
                int a = (page.Value - 1) * Settings.moviePerPage;
                list = db.Movies
                        .OrderBy(x => x.MovieId)
                        .Skip(a)
                        .Take(Settings.moviePerPage)
                        .ToList();
            } else
                list = db.Movies.Take(Settings.moviePerPage).ToList();

            float MovieCount = db.Movies.Count();
            double PageCount = Math.Ceiling(MovieCount / Settings.moviePerPage);

            int current = page.HasValue ? page.Value : 1;


            ViewBag.Start = current > 2 ? current - 2 : 1;
            ViewBag.End = current < PageCount - 2 ? current +2 : PageCount;
            ViewBag.CurrentPage = current;

            ViewBag.PrevVisible = current > 1;
            ViewBag.NextVisible = current < PageCount;

            HomeViewModel hvm = new HomeViewModel();
            hvm.Movies = list;
            hvm.Sliders = db.Sliders.ToList();

            return View(hvm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}