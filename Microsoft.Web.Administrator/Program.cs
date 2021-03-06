﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using Microsoft.Web.Administrator;

namespace Microsoft.Web.Administrator
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            ServerManager serverManager = new ServerManager();
            //所有iis站点
            SiteCollection siteCollection = serverManager.Sites;
            Binding removeBinding=null;
            foreach (Site site in siteCollection)
            {
                //retrieve the State of the Site
                stringBuilder.AppendFormat(" site.Name:{0},site.State:{1},site.Id:{2}\r\n", site.Name, site.State, site.Id);

                //Get the Binding objects for this Site
                BindingCollection bindingCollection = site.Bindings;
                foreach (Binding binding in bindingCollection)
                {
                    //put code here to work with each Binding
                    stringBuilder.AppendFormat("\t binding.Host:{0} \r\n", binding.Host);
                    stringBuilder.AppendFormat("\t binding.Protocol:{0} \r\n", binding.Protocol);
                    stringBuilder.AppendFormat("\t binding.CertificateHash:{0} \r\n", binding.CertificateHash);
                    stringBuilder.AppendFormat("\t binding.BindingInformation:{0} \r\n", binding.BindingInformation);
                    stringBuilder.AppendFormat("\t binding.IsIPPortHostBinding:{0} \r\n", binding.IsIPPortHostBinding);
                    stringBuilder.AppendFormat("\t ------------------------------------------- \r\n");
                    if (binding.Host == "www.baidu.com")
                    {
                        removeBinding = binding;
                   
                    }
                }

                //Get the list of all Applications for this Site
                ApplicationCollection applicationCollection = site.Applications;
                foreach (Application application in applicationCollection)
                {
                    //put code here to work with each Application
                    // stringBuilder.AppendFormat("\t\t application.ApplicationPoolName：{0},application.Schema:{1}\r\n", application.ApplicationPoolName, application.Schema);
                }

                ConfigurationAttributeCollection attributeCollection = site.Attributes;
                foreach (ConfigurationAttribute configurationAttribute in attributeCollection)
                {
                    //put code here to work with each ConfigurationAttribute
                }

                if (removeBinding != null)
                {
                   // site.Bindings.Remove(removeBinding);
                }
          

            }

            Console.WriteLine(stringBuilder.ToString());
            Console.WriteLine("输入需要绑定域名的站点索引,从1开始");
            string read = Console.ReadLine();
            int id = Convert.ToInt32(read);
            var a = siteCollection.SingleOrDefault(m => m.Id == id);
            // a.Stop();

            //绑定域名
            a.Bindings.Add("*:80:test.baidu.com", "http");
            //*:80:admin.localtgjs.com
           // serverManager.CommitChanges();



            var sites= siteCollection.SingleOrDefault(m => m.Name == "test.site.com");
            //获取该站点上已经绑定的域名
            var domains = sites.Bindings;
            Binding aaa = domains.SingleOrDefault(o => o.Host == "www.baidu.com");
            sites.Bindings.Remove(aaa);
            serverManager.CommitChanges();



            Console.ReadKey();




        }
    }
}
