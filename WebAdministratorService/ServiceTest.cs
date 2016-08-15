using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;


namespace WebAdministratorService
{
    public partial class ServiceTest : ServiceBase
    {
        public string ParamTest { get; set; }

        public ServiceTest(string paramTest)
        {
            InitializeComponent();
            ParamTest = paramTest;

        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            if (args.Length > 0)
            {
                ParamTest = args[0];
            }
            ServerManager serverManager = new ServerManager();
            using (System.IO.StreamWriter sw = new StreamWriter("D:\\log.txt", true))
            {
                SiteCollection siteCollection = serverManager.Sites;
                foreach (Site site in siteCollection)
                {
                    sw.WriteLine(site.Name);
                }

                var testSite = siteCollection.SingleOrDefault(m => m.Id == 1);
                if (testSite != null)
                {
                    //绑定域名
                    testSite.Bindings.Add("*:80:test.mydomain.com", "http");
                    sw.WriteLine("1绑定域名成功");
                }
                else
                {
                    sw.WriteLine("id=1的站点未发现");
                }

                var testSite2 = siteCollection.SingleOrDefault(m => m.Id == 100);
                if (testSite2 != null)
                {
                    testSite2.Bindings.Add("*:80:test.mydomain100.com", "http");

                    sw.WriteLine("100绑定域名成功");
                }
                else
                {
                    sw.WriteLine("id=100的站点未发现");
                }
                sw.WriteLine("这里是参数：{0}",ParamTest);
                serverManager.CommitChanges();
            }

        }

        /// <summary>
        /// 停止
        /// </summary>
        protected override void OnStop()
        {
            using (System.IO.StreamWriter sw = new StreamWriter("D:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "Stop.");
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        protected override void OnPause()
        {

        }

        /// <summary>
        /// 继续
        /// </summary>
        protected override void OnContinue()
        {

        }


    }
}
