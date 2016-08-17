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
        /// <summary>
        /// 需要绑定域名的站点名称
        /// </summary>
        private string SiteName { get; set; }

        /// <summary>
        /// 需要绑定的域名
        /// </summary>
        private string DomainName { get; set; }



        public ServiceTest()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            ServerManager serverManager = new ServerManager();
            using (System.IO.StreamWriter sw = new StreamWriter("D:\\log.txt", true))
            {
                if (args.Length >= 2)
                {
                    try
                    {
                        SiteName = args[0];
                        DomainName = args[1];
                        SiteCollection siteCollection = serverManager.Sites;
                        if (siteCollection.All(m => m.Name != SiteName))
                        {
                            sw.WriteLine("站点:{0}，不存在.\r\n{1}", SiteName,DateTime.Now);
                            this.OnStop();
                            return;
                        }
                        var site = siteCollection.SingleOrDefault(m => m.Name == SiteName);

                        //获取该站点上已经绑定的域名
                        var domains = site.Bindings;
                        if (domains.Any(m => m.Host == DomainName))
                        {
                            sw.WriteLine("站点:{0}，上已经绑定了域名:{1}\r\n{2}", SiteName, DomainName,DateTime.Now);
                            this.OnStop();
                            return;
                        }
                        site.Bindings.Add(string.Format("*:80:{0}", DomainName), "http");
                        serverManager.CommitChanges();
                        sw.WriteLine("站点:{0}，绑定域名:{1}，成功。\r\n{2}", SiteName, DomainName,DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        sw.WriteLine("向站点：{0}，绑定域名：{1}，时出错，错误信息：{2} \r\n{3}", SiteName,DomainName,ex.Message,DateTime.Now);
                    }
                    finally
                    {
                      
                    }

                }
                else
                {
                    sw.WriteLine("参数不符合要求：{0}。\r\n{1}", string.Join(",", args),DateTime.Now);
                
                }
                this.OnStop();

            }

        }

        /// <summary>
        /// 停止
        /// </summary>
        protected override void OnStop()
        {
            using (System.IO.StreamWriter sw = new StreamWriter("D:\\log.txt", true))
            {

                sw.WriteLine("服务停止：{0}\r\n",DateTime.Now);
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
