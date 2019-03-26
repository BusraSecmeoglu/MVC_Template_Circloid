using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Template.Models;

namespace MVC_Template.Controllers
{
    [Authorize]
    public class TedarikciController : Controller
    {
        NorthwindContext ctx = new NorthwindContext();

        // GET: Tedarikci
        public ActionResult Index()
        {
            List<Supplier> ktg = ctx.Suppliers.ToList();
            return View(ktg);
        }

        public ActionResult TedarikciEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TedarikciEkle(Supplier ted)
        {
            ctx.Suppliers.Add(ted);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Guncelle(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Supplier ted = ctx.Suppliers.Find(id);

            if (ted == null)
                return HttpNotFound();

            return View(ted);
        }

        [HttpPost]
        public ActionResult Guncelle(Supplier td)
        {
            Supplier ted = ctx.Suppliers.Find(td.SupplierID);

            if (ted == null)
                return HttpNotFound();

            ted.CompanyName = td.CompanyName;
            ted.ContactName = td.ContactName;
            ted.ContactTitle = td.ContactTitle;


            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public string Sil(int id)
        {
            Supplier s = ctx.Suppliers.FirstOrDefault(x => x.SupplierID == id);

            try
            {
                ctx.Suppliers.Remove(s);
                ctx.SaveChanges();
                return "OK";
            }
            catch (Exception ex)
            {
                return "Hata " + ex.ToString();
            }
        }
    }
}