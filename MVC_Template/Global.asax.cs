using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_Template
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start()
        {
            int sayac;
            if (Application["AktifKullanici"] == null)
            {
                sayac = 1;
            }
            else
            {
                sayac = (int)Application["AktifKullanici"];
                sayac++;
            }
            Application["AktifKullanici"] = sayac;

            int sayac2;
            if(Application["ToplamZiyaretci"]==null)
            {
                sayac2 = 1;
                Application["ToplamZiyaretci"] = sayac2;
            }
            else
            {
                sayac2 = (int)Application["ToplamZiyaretci"];
                sayac2++;
            }
            Application["ToplamZiyaretci"] = sayac2;
        }
        protected void Session_End()
        {
            int sayac = (int)Application["AktifKullanici"];
            sayac--;
            Application["AktifKullanici"] = sayac;
        }
    }
}
