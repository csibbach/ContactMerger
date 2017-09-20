using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Engines.contracts;
using ContactMerger.Models;
using Microsoft.AspNet.Identity;

namespace ContactMerger.Controllers
{
    public class ContactController : ApiController
    {
        private readonly IContactProvider _contactProvider;
        private readonly IGoogleCredentialProvider _googleCredentialProvider;
        private readonly IContactMatchingEngine _contactMatchingEngine;

        public ContactController(IContactProvider contactProvider, 
            IGoogleCredentialProvider googleCredentialProvider,
            IContactMatchingEngine contactMatchingEngine)
        {
            _contactProvider = contactProvider;
            _googleCredentialProvider = googleCredentialProvider;
            _contactMatchingEngine = contactMatchingEngine;
        }

        [Authorize]
        public async Task<ContactSet> Get()
        {
            // Get all the registered account credentials
            var credentials = await _googleCredentialProvider.GetCredentials(User.Identity.GetUserName());

            // Grab the account emails out of them
            var emails = credentials.Keys;

            // Going to get a little tricky here, and retrieve all the contacts in parallel
            var results = await Task.WhenAll(emails.Select(email => 
                _contactProvider.GetContacts(User.Identity.Name, email)));
            
            // Got a list of contact results! Now I send them through the merging engine.
            return await _contactMatchingEngine.MergeContactLists(results.ToList());
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