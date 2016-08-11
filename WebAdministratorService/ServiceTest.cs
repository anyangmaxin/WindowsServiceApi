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
        public ServiceTest()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
          ServerManager serverManager=new ServerManager();
            using (System.IO.StreamWriter sw = new StreamWriter("D:\\log.txt", true))
            {
                SiteCollection siteCollection = serverManager.Sites;
                foreach (Site site in siteCollection)
                {
                    sw.WriteLine(site.Name);
                }
            }
        }

        protected override void OnStop()
        {
            using (System.IO.StreamWriter sw = new StreamWriter("D:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"Stop.");
            }
        }
    }
}
