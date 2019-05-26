using Microsoft.AspNet.Identity;
using MVCFilmSatis.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cart
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string uid = User.Identity.GetUserId();
                Customer c = db.Users.Find(uid);

                if (c.ShoppingCart == null)
                {
                    c.ShoppingCart = new ShoppingCart();
                    c.ShoppingCart.Movies = new List<Movie>();
                }

                return View(c.ShoppingCart);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Checkout()
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);

            ViewBag.Total = c.ShoppingCart.SubTotal;
            ViewBag.CartNo = c.ShoppingCart.ShoppingCartId;
            return View();
        }

        [HttpPost]
        public ActionResult PayBankTransfer(int? approve)
        {
            if (approve.HasValue && approve.Value == 1)
            {
                BankTransferPayment p1 = new BankTransferPayment();
                p1.IsApproved = false;
                p1.NameSurname = User.Identity.GetNameSurname();
                p1.TC = User.Identity.GetTC();

                BankTransferService service = new BankTransferService();

                bool isPaid = service.MakePayment(p1);

               
                if (isPaid) {
                    CreateOrder(isPaid);
                    ResetShoppingCart();
                }
                   

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Checkout");
        }

        public void ResetShoppingCart()
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);
            c.ShoppingCart.Movies.Clear();
            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Order CreateOrder(bool isPaid)
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);

            Order order = new Order();
            order.Date = DateTime.Now;
            order.Customer = c;
            order.IsPaid = isPaid;
            order.OrderItems = new List<OrderItem>();
            foreach (var item in c.ShoppingCart.Movies)
            {
                OrderItem oi = new OrderItem();
                oi.Movie = item;
                oi.Count = 1;
                oi.Price = item.Price;
                order.OrderItems.Add(oi);
            }
            order.SubTotal = c.ShoppingCart.SubTotal;
            db.Orders.Add(order);
            db.SaveChanges();
            return order;
        }

        public ActionResult Delete(int id)
        { //int id : sepetten kaldırılacak film id si
            //silinecek filmi bul
            Movie toBeDeleted = db.Movies.Find(id);

            //giriş yapmış kişinin idsi
            string uid = User.Identity.GetUserId();
            //giriş yapmış kişinin tüm bilgileri
            Customer c = db.Users.Find(uid);
            //kişinin sepetinden ilgili filmi kaldır
            c.ShoppingCart.Movies.Remove(toBeDeleted);
            //kişiyi değişti olarak işaretle
            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home", new { error = "Login to buy movies." });

            //Giriş yapmış kişinin id si
            string uid = User.Identity.GetUserId();

            //Giriş yapmış kişinin tüm bilgileri
            Customer c = db.Users.Find(uid);

            //null reference exception önlemleri
            if (c.ShoppingCart == null)
                c.ShoppingCart = new ShoppingCart();

            if (c.ShoppingCart.Movies == null)
                c.ShoppingCart.Movies = new List<Movie>();

            if (c.ShoppingCart.Movies.Any(x => x.MovieId == id))
            {   //seçilen film zaten sepette var
                return RedirectToAction("Index", "Home", new { error = "You already have this movie in your cart." });
            }
            else
            { //film sepette yok, işleme devam ediyoruz
              //seçilmiş olan film
                Movie chosenMovie = db.Movies.Find(id);
                c.ShoppingCart.Movies.Add(chosenMovie);

                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
    }
}