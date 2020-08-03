using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using CWB.Api.Models;
using Refit;

namespace CWB.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContactsController : ApiController
    {
        // GET api/<controller>
        public async Task<IEnumerable<Contact>> Get()
        {
            var service = RestService.For<IDynamicsService>("https://nsappdev03.crm.dynamics.com");
            var token = await Utils.AadHelpers.GetAccessTokenForScopeOnBehalfOfLoggedInUser("https://nsappdev03.api.crm.dynamics.com/user_impersonation");
            var result = await service.GetContacts($"Bearer {token}");
            return result.value;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}