using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_Template.Controllers
{
    [Authorize]
    public class RolController : Controller
    {
        // GET: Rol
        public ActionResult Index()
        {
            List<string> roller = Roles.GetAllRoles().ToList();
            return View(roller);
        }

        public ActionResult RolEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RolEkle(string RoleName)
        {
            Roles.CreateRole(RoleName);
            return RedirectToAction("Index");
        }

        // /Kullanici/Sil/John seklinde kullanildiginda.
        //[HttpPost]
        //public void Sil(string id)
        //{
        //     Roles.DeleteRole(id);
        //}

        [HttpPost]
        public void Sil(string RoleName)
        {
             Roles.DeleteRole(RoleName);
        }
    }
}