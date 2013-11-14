using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SampleWebApp.Models;

namespace SampleWebApp.Controllers
{
    public class HomeController : Controller
    {
        public RegistrationRepository Repository { get; set; }

        public HomeController()
        {
            this.Repository = new RegistrationRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AttendeeRegistration registration)
        {
            var id = this.Repository.Regist(registration);
            return RedirectToAction("Complete", new { registration.Id });
        }

        public ActionResult Complete(Guid id)
        {
            var registration = this.Repository.Find(id);
            return View(registration);
        }
    }
}