using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace MCMContactManagement.API.Controllers
{
    [AllowAnonymous]
    public class OrderApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public OrderApiController()
        {
        }
        [HttpGet]
        public string Get(string id)
        {
            return "Test";
        }
        [HttpGet]
        [ActionName("GetById")]
        public string GetById(string Id)
        {
            return "Test";
        }
    }
}

