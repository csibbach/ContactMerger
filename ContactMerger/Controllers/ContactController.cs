using System.Threading.Tasks;
using System.Web.Http;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Models;

namespace ContactMerger.Controllers
{
    public class ContactController : ApiController
    {
        private readonly IContactProvider _contactProvider;

        ContactController(IContactProvider contactProvider)
        {
            _contactProvider = contactProvider;
        }

        [Authorize]
        public async Task<ContactList> Get(string accountEmail)
        {
            return await _contactProvider.GetContacts(User.Identity.Name, accountEmail);
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