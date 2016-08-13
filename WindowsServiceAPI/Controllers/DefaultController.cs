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
        // GET: api/Default
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Default/5
        public string Get(int id)
        {
            var result = JsonConvert.SerializeObject(new { id });
            return result;

        }

        // POST: api/Default
        public string Post([FromBody]string value)
        {
            return JsonConvert.SerializeObject(value);
        }

        [HttpPost]
        [Route("Test")]
        public string Test(string value)
        {
            return JsonConvert.SerializeObject(value);
        }


        [HttpPost]
        [Route("PostInstall")]
        public string PostInstall([FromBody]string option)
        {
            return JsonConvert.SerializeObject(option == "install" ? "安装请求" : "卸载请求");
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
