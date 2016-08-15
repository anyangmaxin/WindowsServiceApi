using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace GitHubWindowsService.Rest
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Restfull : IRestfull
    {
        public void SuperGet(string id)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Location", "http://google.com/" + id);
        }

        public Stream SuperPost(Stream instance)
        {
            string body = Conversion.ToApp(instance);
            return Conversion.ToWeb(body);
        }
    }
}
