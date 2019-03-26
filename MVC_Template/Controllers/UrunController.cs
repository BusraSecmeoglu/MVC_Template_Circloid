using MVC_Template.Add_Classes;
using MVC_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Template.Controllers
{
    [Authorize]
    public class UrunController : Controller
    {
        NorthwindContext ctx = new NorthwindContext();
        // GET: Urun
        public ActionResult Index()
        {
            List<Product> prd = ctx.Products.ToList();
            return View(prd);
        }

        public ActionResult UrunEkle()
        {
            ViewBag.Kategoriler = ctx.Categories.ToList();
            ViewBag.Tedarikciler = ctx.Suppliers.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult UrunEkle(Product prd)
        {
            ctx.Products.Add(prd);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult UrunSil(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Product product = ctx.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            ctx.Products.Remove(product);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        //public ActionResult UrunSil(int? ProductID)
        //{
        //    if (ProductID == null)
        //        return RedirectToAction("Index");

        //    Product product = ctx.Products.Find(ProductID);

        //    if (product == null)
        //        return HttpNotFound();

        //    ctx.Products.Remove(product);
        //    ctx.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult UrunSorSil(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == id);
            if (prd == null)
                return HttpNotFound();

            return View(prd);
        }

        [HttpPost]
        public ActionResult UrunSorSil(Product p)
        {
            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == p.ProductID);
            ctx.Products.Remove(prd);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Guncelle(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Product product = ctx.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            ViewBag.Kategoriler = ctx.Categories.ToList();
            ViewBag.Tedarikciler = ctx.Suppliers.ToList();

            return View(product);
        }

        [HttpPost]
        public ActionResult Guncelle(Product prd)
        {
            Product product = ctx.Products.Find(prd.ProductID);

            if (product == null)
                return HttpNotFound();

            product.ProductName = prd.ProductName;
            product.UnitPrice = prd.UnitPrice;
            product.UnitsInStock = prd.UnitsInStock;
            product.CategoryID = prd.CategoryID;
            product.SupplierID = prd.SupplierID;

            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult UrunDetay()
        {
            //talebin query stringlerinden ismi id olanı getir.
            int id = Convert.ToInt32(Request.QueryString["id"].ToString());
            //talebin query stringlerinden ismi adi olanı getir.
            string adi = Request.QueryString["adi"].ToString();

            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == id);
            //model yöntemi ile veriyi gönderiyoruz.
            return View(prd);
        }

        //Elimizde bir sepet nesnesi olmalı içinde ürün listesi olmalı bunu için bir sepet sınıfı oluşturalım ve içerisine ürün listesi tanımlayalım
        [HttpPost]
        public int SepeteEkle(int id)
        {
            //varolan bir sepet varsa o sepete eklesin yoksa yeni bir sepet oluştursun
            Sepet s;
            if (Session["AktifSepet"] == null)
            {
                s = new Sepet();
                s.Urunler = new List<SepetDetay>();
            }
            else
            {
                s = (Sepet)Session["AktifSepet"];
            }
            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == id);
            
            if (s.Urunler.Any(c => c.Urun.ProductID == prd.ProductID))
            {
                s.Urunler.FirstOrDefault(c => c.Urun.ProductID == prd.ProductID).Adet++;
            }
            else
            {
                s.Urunler.Add(new SepetDetay() { Adet = 1, Urun = prd });
            }
            Session["AktifSepet"] = s;
            return s.Urunler.Sum(c=>c.Adet);
        }

        [HttpPost]
        public void SepettenUrunCikart(int id)
        {
            //List<Product> urunler = new List<Product>();
            if (Session["AktifSepet"] != null)
            {
                Sepet s = (Sepet)Session["AktifSepet"];

                SepetDetay detay = s.Urunler.Find(x => x.Urun.ProductID == id);

                s.Urunler.Remove(detay);

                Session["AktifSepet"] = s;
                //urunler = s.Urunler;                
            }
        }

        [HttpPost]
        public int SepetCount()
        {
            Sepet s = (Sepet)Session["AktifSepet"];
            return s.Urunler.Count();
        }

        public int SepetCountGet()
        {
            Sepet s = (Sepet)Session["AktifSepet"];
            return s.Urunler.Count();
        }

        public void test(List<Product>productList)
        {
            //Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == 1);
            //prd.ProductName += " modified";

            foreach(var item in productList)
            {
                ctx.Products.Add(item);
              
            }

            ctx.SaveChanges();
        }


    }
}