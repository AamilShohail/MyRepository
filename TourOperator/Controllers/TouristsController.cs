using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourOperator.Models;
using System.Data.Entity.Infrastructure;
using TourOperator.Common;
using MailKit.Net.Smtp;
using System.Configuration;
using MimeKit;
using System.Web.Routing;
using PagedList;
using System.IO;
using System.Web.Script.Serialization;


namespace TourOperator.Controllers
{   
    

    public class TouristsController : Controller
    {
        
        private DreamTourContext db = new DreamTourContext();


        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            
            string CountryCodeInUrl = "", redirectUrl = "";
            var countryCode = CookieSettings.ReadCookie();
            if (countryCode == "")
            {
                countryCode = "gb";
            }

            if (System.Web.HttpContext.Current.Request.RawUrl.Length >= 2)
            {
                CountryCodeInUrl = System.Web.HttpContext.Current.Request.RawUrl.Substring(1, 2);
            }

            if (countryCode != CountryCodeInUrl)
            {
                if (System.Web.HttpContext.Current.Request.RawUrl.Length >= 2)
                {
                    if (System.Web.HttpContext.Current.Request.RawUrl.Substring(1, 2) != "")
                    {
                        countryCode = System.Web.HttpContext.Current.Request.RawUrl.Substring(1, 2);
                    }
                }

                if (!System.Web.HttpContext.Current.Request.RawUrl.Contains(countryCode))
                {
                    redirectUrl = string.Format("/{0}{1}", countryCode, System.Web.HttpContext.Current.Request.RawUrl);
                }
                else
                {
                    redirectUrl = System.Web.HttpContext.Current.Request.RawUrl;
                }
                CookieSettings.SaveCookie(countryCode);
                System.Web.HttpContext.Current.Response.RedirectPermanent(redirectUrl);
            }

        }

        public class CookieSettings
        {
            public static void SaveCookie(string data)
            {
                var _CookieValue = new HttpCookie("CountryCookie");
                _CookieValue.Value = data;
                _CookieValue.Expires = DateTime.Now.AddDays(300);
                System.Web.HttpContext.Current.Response.Cookies.Add(_CookieValue);
            }

            public static string ReadCookie()
            {
                var _CookieValue = "";
                if (System.Web.HttpContext.Current.Request.Cookies["CountryCookie"] != null)
                    _CookieValue = System.Web.HttpContext.Current.Request.Cookies["CountryCookie"].Value;
                return _CookieValue;
            }
        }
        
        public void Country(object selectCountry = null)
        {
            List<string> CountryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo(CInfo.LCID);
                if (!(CountryList.Contains(R.EnglishName)))
                {
                    CountryList.Add(R.EnglishName);
                }
            }
            CountryList.Sort();
            ViewBag.CountryList = CountryList;
        }
   
        public ActionResult Index(string SearchBy, string Search, int? page, string sortBy)
        {
            
            if (SearchBy == "Gender")
            {
                return View(db.Tourists.Where(model => model.Gender == Search || Search == null).ToList().ToPagedList(page ?? 1, 3));
            }
            else if(SearchBy == "Name")
            {
                return View(db.Tourists.Where(model => model.LastName.StartsWith(Search) || model.FirstName.StartsWith(Search) || Search == null).ToList().ToPagedList(page ?? 1, 3));
            }
            else if(SearchBy == "Country")
            {
                return View(db.Tourists.Where(model => model.Country.StartsWith(Search) || Search == null).ToList().ToPagedList(page ?? 1, 3));
            }
            else
            {
                return View(db.Tourists.Where(model => model.TouristType == Search || Search == null).ToList().ToPagedList(page ?? 1, 3));
            }
            
        }

   
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tourist tourist = db.Tourists.Find(id);
            if (tourist == null)
            {
                return HttpNotFound();
            }
            return View(tourist);
        }

        public ActionResult Create()
        {
            Country();
            return View();
        }

        // POST: Tourists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Exclude =  "IsEmailVerified,ActivationCode")] Tourist tourist)
        {
            if (ModelState.IsValid)
            {
                db.SystemUsers.Add(tourist);
                var IsExist = IsEmailExist(tourist.Email);
                if (IsExist)
                {
                    ModelState.AddModelError("EmailExist","E-mail already exist");
                    return View(tourist);
                }
                tourist.ActivationCode = Guid.NewGuid();
                tourist.Password = Crypto.Hash(tourist.Password);
                db.SaveChanges();
                string sendEmail = ConfigurationManager.AppSettings["SendVerificationLinkEmail"];
                if(sendEmail.ToLower() == "true")
                {
                    SendVerificationLinkEmail(tourist.Email, tourist.ActivationCode.ToString());
                }
                return RedirectToAction("Index");
            }

            return View(tourist);
        }


        private bool IsEmailExist(string emailId)
        {
            
            {
                var v = db.Tourists.Where(model => model.Email == emailId).FirstOrDefault();
                return v != null;
            }
        }

        [HttpPost]
        public ActionResult Email(string activeCode)
        {
            bool Status = false;
            var v = db.Tourists.Where(model => model.ActivationCode == new Guid(activeCode)).FirstOrDefault();
            if(v != null)
            {
                db.SaveChanges();
                Status = true;
            }
            else
            {
                ViewBag.Message = "Invalid Request";
            }
            ViewBag.Status = Status;
            return View();
        }
        [NonAction]
        public void SendVerificationLinkEmail(string email , string activCode , string Emailfor = "Email")
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DreamTour Tour Operator", "shohail.17@itfac.mrt.ac.lk"));
            message.To.Add(new MailboxAddress("Shohail", email));
            var verifyUrl = "/tourists/"+Emailfor+"/" + activCode;
      
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            if(Emailfor == "Email")
            {
                message.Subject = "An E-mail from CodeDream Team to reset your password";
                message.Body = new TextPart("HTML")
                {
                    Text = "We are excited to tell you that your dotnet awesome account is successfully created. " +
                    "Please click on the below link to verify your account" +
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

        // GET: Tourists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tourist tourist = db.Tourists.Find(id);
           
            if (tourist == null)
            {
                return HttpNotFound();
            }
            return View(tourist);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Gender,DateOfBirth,Email,Password,ContactNumber,TouristType,IsEmailVerified,ActivationCode,Country,DisplayPicture")] Tourist tourist)
        {
            if (ModelState.IsValid)
            {
                db.SystemUsers.Add(tourist);
                db.Entry(tourist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tourist);
        }
        [HttpPost]
        public ActionResult DeleteMultiple(IEnumerable<int> TouristIDs)
        {
            List <Tourist> tourists = db.Tourists.Where(model => TouristIDs.Contains(model.UserID)).ToList();
            
            foreach(Tourist item in tourists)
            {
                db.Tourists.Remove(item);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tourist tourist = db.Tourists.Find(id);
            if (tourist == null)
            {
                return HttpNotFound();
            }
            return View(tourist);
        }

        // POST: Tourists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tourist tourist = db.Tourists.Find(id);
            db.SystemUsers.Remove(tourist);
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
