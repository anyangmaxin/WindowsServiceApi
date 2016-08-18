using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;


namespace WindowsServiceAPI.Controllers
{
    [RoutePrefix("api/Default")]
    public class DefaultController : ApiController
    {
        #region Get

        // GET: api/Default
        public IEnumerable<string> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // GET: api/Default/5
        public string Get(int id)
        {
           // return id.ToString();
         return  WindowsServiceInvest.ConfigureTest.ServiceControllerExtension.StartService("ServiceTest", new string[] { "PSA8001", "www.baidu.com" });
         
        }

        #endregion


        // POST: api/Default
        public string Post([FromBody]string value)
        {
            return JsonConvert.SerializeObject(value);
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}
