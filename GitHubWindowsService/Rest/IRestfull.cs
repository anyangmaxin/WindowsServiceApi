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
    [ServiceContract]
    public interface IRestfull
    {
        [OperationContract]
        [WebGet]
        void SuperGet(string id);

        [OperationContract]
        [WebInvoke(Method = "POST")]
        Stream SuperPost(Stream instance);
    }
}
