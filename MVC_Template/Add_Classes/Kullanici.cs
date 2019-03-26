using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Template.Add_Classes
{
    public class Kullanici
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
    }
}