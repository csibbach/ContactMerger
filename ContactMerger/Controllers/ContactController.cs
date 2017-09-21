using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Engines.contracts;
using ContactMerger.Utility;
using Microsoft.AspNet.Identity;

namespace ContactMerger.Controllers
{

    [Authorize]
    public class ContactController : Controller
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

        public async Task<ActionResult> GetContactSet()
        {
            // Get all the registered account credentials
            var credentials = await _googleCredentialProvider.GetCredentials(User.Identity.GetUserName());

            // Grab the account emails out of them
            var emails = credentials.Keys;

            // Going to get a little tricky here, and retrieve all the contacts in parallel
            var results = await Task.WhenAll(emails.Select(email => 
                _contactProvider.GetContacts(User.Identity.Name, email)));
            
            // Got a list of contact results! Now I send them through the merging engine.
            var contactSet = await _contactMatchingEngine.MergeContactLists(results.ToList());

            // Convert to JSON and on our way
            return new JsonCamelCaseResult(contactSet, JsonRequestBehavior.AllowGet);
        }
    }
}