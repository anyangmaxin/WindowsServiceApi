using System.Collections.Generic;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Administrator.Lib
{
    public sealed class WebAdministrator
    {

        public List<string> GetSites()
        {
            List<string> siteNames = new List<string>();
            ServerManager serverManager = new ServerManager();
            SiteCollection sites = serverManager.Sites;
            foreach (Site site in sites)
            {
                siteNames.Add(site.Name);
            }
            return siteNames;
        }
    }
}
