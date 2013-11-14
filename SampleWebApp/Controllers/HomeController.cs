using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
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

            var smtpConfig = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            var smtpClinet = new SmtpClient();
            smtpClinet.Send(
                from: smtpConfig.From,
                recipients: registration.Email,
                subject: "Registration Comfirmation",
                body: "Hi " + registration.Name + ",\r\n Thank you for your attend!"
                );

            return RedirectToAction("Complete", new { registration.Id });
        }

        public ActionResult Complete(Guid id)
        {
            var registration = this.Repository.Find(id);
            return View(registration);
        }
    }
}