using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Template.Controllers
{
    using Models;
    using Add_Classes;

    //Bu controller üzerndeki action'lara erişebilmek için kullanıcının giriş yapması gerekmektedir.eğer kullanıcı authenticate değilse hiçbir action'a erişemeyecek
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.AktifKullanici = HttpContext.Application["AktifKullanici"];
            ViewBag.ToplamZiyaretci = HttpContext.Application["ToplamZiyaretci"];
            return View();
        }
        //çerez silmeyi client side tarafında yaparsak ayarlarda elle silinebilir engellenebilir.
        //browser ayarlarındaki engellemeler;
        //1.taraf bizim zyaret ettiğimiz siteden çerez oluşturma ve okumaya izin ama 3.taraf ziyaret ettiğimiz web sayfasının içindeki başka sayfaların çerezlerinin engellenmesi
        public ActionResult CookieAta()
        {
            //Cookie'ye deger atamayı saglayacagız
            return View();
        }

        [HttpPost]
        public ActionResult CookieAta(string CookieAdi, string CookieDegeri)
        {
            HttpCookie ck = new HttpCookie(CookieAdi);
            ck.Value = CookieDegeri;
            ck.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(ck);

            return View();
        }

        public ActionResult CookieOku()
        {
            string deger = Request.Cookies["SC201"].Value;
            ViewBag.Cerez = deger;
            return View();
        }

        public ActionResult Sepetim()
        {
            List<SepetDetay> urunler = new List<SepetDetay>();
            Sepet s = new Sepet();
            s.Urunler = new List<SepetDetay>();

            if (Session["AktifSepet"] != null)
            {
                 s = (Sepet)Session["AktifSepet"];
               
            }
            
            return View(s.Urunler);
        }

        [HttpPost]
        public void SepettenCikart(int id)
        {
            //List<Product> urunler = new List<Product>();
            if (Session["AktifSepet"] != null)
            {
                Sepet s = (Sepet)Session["AktifSepet"];

                SepetDetay detay = s.Urunler.Find(x => x.Urun.ProductID == id);

                detay.Adet--;

                Session["AktifSepet"] = s;
                //urunler = s.Urunler;                
            }
        }
    }
}