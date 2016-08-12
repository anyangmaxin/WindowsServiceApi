using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Administrator.Lib;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // ViewBag.Sites = new WebAdministrator().GetSites();
            return View();
        }

        [HttpPost]
        public ActionResult GetData()
        {
            var test = WindowsServiceInvest.ConfigureTest.ServiceControllerExtension.CreateService("ServiceTest", "ServiceTest",
                  "D:\\WorkSapce\\Test\\Microsoft.Web.Administrator\\WebAdministratorService\\bin\\Debug", true);
            ViewBag.Status = test.Status;
            ViewBag.ServiceName = test.ServiceName;
            return View();
        }
    }
}