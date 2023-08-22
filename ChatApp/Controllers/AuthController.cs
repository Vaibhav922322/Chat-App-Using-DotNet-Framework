using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;

namespace ChatApp.Controllers
{

    public class AuthController : Controller
    {
        //FormCollection u1 = new FormCollection();
        private static UserModel u1 = new UserModel();
        

        private static long usersCount = 0;

        // GET: Login
        [System.Web.Mvc.HttpGet]
        public ActionResult Login() { 
            return View(new UserLoginModel());
        }

        // POST: Login
        [System.Web.Mvc.HttpPost]
        public ActionResult Login([FromBody] UserLoginModel user){
            if(!ModelState.IsValid)
                return View(user);

            user.email_Id = user.email_Id.ToUpper();
            using (ChatAppDBContext dBContext = new ChatAppDBContext())
            {
                string password = EncryptBase64(user.password);
                UserModel validUser = dBContext.userDBSet.Where(x => x.email_Id == user.email_Id 
                                                      && x.password == password).SingleOrDefault();
                if (validUser == null)
                {
                    ViewBag.error = "User doesn't exist.. Register first";
                    return View(user);
                }
                System.Web.Security.FormsAuthentication.SetAuthCookie(validUser.Id, false);
            }


            return RedirectToAction(actionName:"Home",controllerName:"Home");

        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Authorize]
        public ActionResult LoginNew(){
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction(actionName: "Login", controllerName: "Auth");
        }

        // GET: Register
        [System.Web.Mvc.HttpGet]
        public ActionResult Register(){
            return View(new UserRegisterModel());
        }

        // POST: Register
        [System.Web.Mvc.HttpPost]
        public ActionResult Register([FromBody] UserRegisterModel user)
        {
            if (!ModelState.IsValid)
                return View(user);

            user.email_Id = user.email_Id.ToUpper();
            using (ChatAppDBContext dBContext = new ChatAppDBContext())
            {
                if (dBContext.userDBSet.Where( x => x.email_Id == user.email_Id ).Count() != 0)
                {
                    ViewBag.error = "User already exist.. user different email id";
                    return View(user);
                }

                UserModel newUser = new UserModel();
                newUser.name = user.name;
                newUser.email_Id = user.email_Id;
                newUser.password = EncryptBase64(user.password);
                newUser.Id =EncryptBase64(usersCount.ToString());
                usersCount++;

                dBContext.userDBSet.Add(newUser);
                try
                {
                    dBContext.SaveChanges();
                }
                catch
                {
                    ViewBag.error = "Some error occured while saving into data";
                    return View(user);
                }

                System.Web.Security.FormsAuthentication.SetAuthCookie(newUser.Id, false);
            }

            return RedirectToAction(actionName: "Home", controllerName: "Home");
        }
    
        [System.Web.Mvc.HttpGet]
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction(actionName: "Login", controllerName: "Auth");
        }


        private static string EncryptBase64(string text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private static string DecryptBase64(string base64EncodedData)
         {
             var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
             return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
         }
    }
}
