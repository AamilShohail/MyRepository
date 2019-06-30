using MailKit.Net.Smtp;
using MimeKit;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TourOperator.Common;
using TourOperator.Models;


namespace TourOperator.Controllers
{
    public class SystemUsersController : Controller
    {
        private DreamTourContext db = new DreamTourContext();

        // GET: SystemUsers
        [NonAction]
        public void SendVerificationLinkEmail(string email, string activCode, string Emailfor = "ResetPassword")
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DreamTour Tour Operator", "shohail.17@itfac.mrt.ac.lk"));
            message.To.Add(new MailboxAddress("Shohail", email));
            var verifyUrl = "/SystemUsers/" + Emailfor + "/" + activCode;

            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            if (Emailfor == "ResetPassword")
            {
                message.Subject = "An E-mail from CodeDream Team";
                message.Body = new TextPart("HTML")
                {
                    Text = "We got request for reset your account password." +
                    " Please click on the below link to reset your password" +
                    "'<a href = '" + link + "'>" + link + "</a>'"
                };
            }
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("shohail.17@itfac.mrt.ac.lk", "950771054V");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string message = "";

            var accountT = db.Tourists.Where(model => model.Email == EmailID).FirstOrDefault();
            var accountA = db.Administrations.Where(model => model.AdminUsername == EmailID).FirstOrDefault();

            if (accountT != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(accountT.Email, resetCode, "ResetPassword");
                accountT.ResetPasswordCoded = resetCode;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "Reset password link has been sent to your e-mail";
            }
            else if(accountA != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(accountA.Email, resetCode, "ResetPassword");
                accountA.ResetPasswordCoded = resetCode;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "Reset password link has been sent to your e-mail";
            }
            else
            {
                message = "Account not found. Please Check your E-mail";
            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            var tourist = db.Tourists.Where(model => model.ResetPasswordCoded == id).FirstOrDefault();
            var admin = db.Administrations.Where(model => model.ResetPasswordCoded == id).FirstOrDefault();
            if (tourist != null || admin != null)
            {
                ResetPassword rp = new ResetPassword();
                rp.ResetCode = id;
                return View(rp);
            }
            else
            {
                return HttpNotFound();

            }
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPassword resetPassword)
        {
            var message = "";


            var tourist = db.Tourists.Where(model => model.ResetPasswordCoded == resetPassword.ResetCode).FirstOrDefault();
            var admin = db.Administrations.Where(model => model.ResetPasswordCoded == resetPassword.ResetCode).FirstOrDefault();



            if (tourist != null)
            {
                tourist.Password = Crypto.Hash(resetPassword.NewPassword);
                tourist.ResetPasswordCoded = "";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "You have successfully changed the Password";
                
                return RedirectToAction("Login");
            }
            else if(admin != null)
            {
                admin.Password = Crypto.Hash(resetPassword.NewPassword);
                admin.ResetPasswordCoded = "";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "You have successfully changed the Password";
                return RedirectToAction("Login");
            }
            else
            {
                message = "Changes can not be made";
            }
            ViewBag.Message = message;
            return View(resetPassword);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin userLogin, string ReturnUrl = "")
        {
            string message = "";
            var tourist = db.Tourists.Where(model => model.Email == userLogin.EmailID).FirstOrDefault();
            var admin = db.Administrations.Where(model => model.AdminUsername == userLogin.EmailID).FirstOrDefault();

            if (tourist != null)
            {
                if (string.Compare(Crypto.Hash(userLogin.Password), tourist.Password) == 0)
                {
                    int timeout = userLogin.RememberMe ? 525600 : 20;
                    var ticket = new FormsAuthenticationTicket(userLogin.EmailID, userLogin.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "AuthorizedTourist");
                    }
                }
                else
                {
                    message = "Invalid Credentials provided";
                }
            }
           
            else if (admin != null)
            {
                if (string.Compare(Crypto.Hash(userLogin.Password), admin.Password) == 0)
                {
                    int timeout = userLogin.RememberMe ? 525600 : 20;
                    var ticket = new FormsAuthenticationTicket(userLogin.EmailID, userLogin.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "AuthorizedAdmin");
                    }
                }
                else
                {
                    message = "Invalid Credentials provided";
                }
            }
            else
            {
                message = "Invalid Credentials provided";
            }

            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Index(string SearchBy, string Search, int? page)
        {
            if (SearchBy == "Gender")
            {
                return View(db.SystemUsers.Where(model => model.Gender == Search || Search == null).ToList().ToPagedList(page ?? 1, 3));
            }
            else if (SearchBy == "Name")
            {
                return View(db.SystemUsers.Where(model => model.FirstName.StartsWith(Search) || Search == null).ToList().ToPagedList(page ?? 1, 3));

            }
            else
            {
                return View(db.SystemUsers.Where(model => model.LastName.StartsWith(Search) || Search == null).ToList().ToPagedList(page ?? 1, 3));
            }
        }

        // GET: SystemUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,FirstName,LastName,Gender,Email,Password,ContactNumber")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                db.SystemUsers.Add(systemUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(systemUser);
        }

        // GET: SystemUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Gender,Email,Password,ContactNumber")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SystemUser systemUser = db.SystemUsers.Find(id);
            db.SystemUsers.Remove(systemUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
