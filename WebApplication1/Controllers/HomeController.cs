using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



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



        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StartService(FormCollection collection)
        {

            ViewBag.result = WindowsServiceInvest.ConfigureTest.ServiceControllerExtension.StartService("ServiceTest", new string[] { collection[0], collection[1] });
            ViewBag.args = string.Join(",",collection);
            return View();
        }



    }
}