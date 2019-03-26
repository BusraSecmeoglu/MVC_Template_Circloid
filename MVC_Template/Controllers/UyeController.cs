using MVC_Template.Add_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_Template.Controllers
{
    //eğer bir actionın kendi attribüte ü varsa o geçerlidir.yoksa controllerın attibüteleri geçerlidir

    //bu controlera autorise propertisi tanımladık fakat kullanıcın giriş yapması için buraya gelmesi gerekmektedir çünkü giriş sayfası buradadır.O yüzden giriş yap actionunun viewi çalıştıran get ve viewdan değer alan post actionlarına authenticate olmayı zorunlu kılmamız lazım

    //Authorize yazmazsak AllowAnonymous yazmamıza gerek aklmaz
    [Authorize]
    public class UyeController : Controller
    {
        //Bu iki actionın başına yazalım ki bu actionlar için bu kullanıcı için bu actionlar için authenticate olmasını zorunlu kılmayalım.
        [AllowAnonymous]
        public ActionResult GirisYap()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GirisYap(Kullanici k, string Hatirla)
        {
            bool sonuc = Membership.ValidateUser(k.UserName, k.Password);
            if (sonuc)
            {
                // Bunu çalışması için web.config de bazı ayarların düzenlenmesi gerekir. Yani bu web sitesine üye giriş yapılabileceğine dair ayar yapmamız lazım
                if (Hatirla == "on")
                    //beni hatırla on'sa ikinci parametrenin true olmasıyla bilgileri tutup sayfaya yönlendiriyor
                    FormsAuthentication.RedirectFromLoginPage(k.UserName, true);
                else
                    //benihatırla onsa ikinci parametrenin false olmasıyla bilileri tutmadan sayfaya yönlendiriyo
                    FormsAuthentication.RedirectFromLoginPage(k.UserName, false);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı adı veya parola hatalı!";
                return View();
            }
        }

        public ActionResult CikisYap()
        {
            //Bır oncekı sayfa chrome tarafından cache'lendıgı ıcın normalde erısımımız olmayan bır sayfa cache'ten cagırılıyor. Bunun onune gecmek ıcın sayfayı Home'un Index'ıne yonlendırıyoruz
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }

        [AllowAnonymous]
        public ActionResult ParolamiUnuttum()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ParolamiUnuttum(Kullanici k)
        {
            MembershipUser mu = Membership.GetUser(k.UserName);
            if (mu.PasswordQuestion == k.PasswordQuestion)
            {
                string pwd = mu.ResetPassword(k.PasswordAnswer);
                mu.ChangePassword(pwd, k.Password);
                return RedirectToAction("GirisYap");
            }
            else
            {
                ViewBag.Mesaj = "Girilen bilgiler yanlistir!";
                return View();

            }
        }
    }
}