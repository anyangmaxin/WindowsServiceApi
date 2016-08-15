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
                  "D:\\WorkSapce\\Test\\Microsoft.Web.Administrator\\WebAdministratorService\\bin\\Debug", false);
            if (test != null)
            {
                ViewBag.Status = test.Status;
                ViewBag.ServiceName = test.ServiceName;
            }
            return View();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StartService(string value)
        {
            ViewBag.result = WindowsServiceInvest.ConfigureTest.ServiceControllerExtension.StartService("ServiceTest");
            ViewBag.args = value;
            return View();
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <returns></returns>
        public ActionResult UninstallService()
        {

            ViewBag.Status = "已卸载";
            ViewBag.ServiceName = "不存在";
            if (WindowsServiceInvest.ConfigureTest.ServiceControllerExtension.DeleteService("ServiceTest"))
            {

            }
            return View("GetData");

        }

    }
}