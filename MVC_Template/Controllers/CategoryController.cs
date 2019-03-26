using MVC_Template.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Template.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        NorthwindContext ctx = new NorthwindContext();

        // GET: Category
        public ActionResult Index()
        {
            List<Category> ktg = ctx.Categories.ToList();
            return View(ktg);
        }

        // GET
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ekle([Bind(Include = "CategoryName, Description")] Category ktg, HttpPostedFileBase Picture)
        {
            if (Picture == null)
                return View();

            ktg.Picture = ConvertToBytes(Picture);

            if (ModelState.IsValid)
            {
                ctx.Categories.Add(ktg);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // Category Picture nesnesi Database'de byte[] şeklinde tutulduğu için seçilen resmi byte[]'e çevrilmesini sağlayan method.
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes(image.ContentLength);
            byte[] bytes = new byte[imageBytes.Length + 78];
            Array.Copy(imageBytes, 0, bytes, 78, imageBytes.Length);
            return bytes;
        }

        public ActionResult Guncelle(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Category ktg = ctx.Categories.Find(id);

            if (ktg == null)
                return HttpNotFound();

            return View(ktg);
        }

        [HttpPost]
        public ActionResult Guncelle([Bind(Include = "CategoryID, CategoryName, Description")] Category ktg, HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
                Category k = ctx.Categories.Find(ktg.CategoryID);

                k.CategoryName = ktg.CategoryName;
                k.Description = ktg.Description;

                if (Picture != null)
                {
                    k.Picture = ConvertToBytes(Picture);
                }

                //ctx.Entry(k).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public string Sil(int id)
        {
            Category k = ctx.Categories.FirstOrDefault(x=>x.CategoryID == id);

            if (k.Products.Count() > 0)
            {
                return "Kategoriye ait "+ k.Products.Count() + " ürün var.";
            }


            ctx.Categories.Remove(k);
            ctx.SaveChanges();

            return String.Empty;
        }

    }
}