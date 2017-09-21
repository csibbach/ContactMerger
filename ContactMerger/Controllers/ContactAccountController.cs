using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using ContactMerger.DataProviders.contracts;
using ContactMerger.Factories.contracts;
using ContactMerger.Utility;
using Google.Apis.Auth.OAuth2.Mvc;
using Microsoft.AspNet.Identity;

namespace ContactMerger.Controllers
{
    public class ContactAccountController : Controller
    {
        private readonly IFlowMetadataFactory _flowMetadataFactory;
        private readonly IGoogleCredentialProvider _googleCredentialProvider;

        public ContactAccountController(IFlowMetadataFactory flowMetadataFactory,
            IGoogleCredentialProvider googleCredentialProvider)
        {
            _flowMetadataFactory = flowMetadataFactory;
            _googleCredentialProvider = googleCredentialProvider;
        }

        // This endpoint exists only to walk the user through giving us authentication
        // with google to access their data. Each time you call this it stores the credential
        // under the authorized user in the credential provider, so that it can be used in other
        // calls.
        [Authorize]
        [RequireHttps]
        public async Task<ActionResult> AddContactAccount(CancellationToken cancellationToken)
        {
            // Check the referring URL. If it is from localhost, we are going to start over.
            // Request a new account on the metadata factory and everything should sync up.
            // There's no security from referrer spoofing, but I don't think that constitutes
            // a security vulnerability in this case, as things will either just break or the
            // credential will not be properly claimed.
            if (Request.Url != null && 
                Request.UrlReferrer != null && 
                Request.UrlReferrer.Host == Request.Url.Host) 
            {
                // We came from the same page, must be a new Add Account request
                _flowMetadataFactory.RequestNewAccount();
            }

            // Create a new Authorization Flow- should have a factory for this for testing purposes
            var result = await new AuthorizationCodeMvcApp(this, _flowMetadataFactory.CreateFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            // Redirect if necessary
            if (result.Credential == null)
                return new RedirectResult(result.RedirectUri);

            // Save the credential
            await _googleCredentialProvider.SaveCredential(User.Identity.GetUserName(), result.Credential);
            
            // Go back to home and resume spa-like behavior
            return new RedirectResult("/");
        }

        [Authorize]
        [HttpGet]
        [RequireHttps]
        public async Task<ActionResult> GetAccounts()
        {
            var credentials = await _googleCredentialProvider.GetCredentials(User.Identity.GetUserName());

            return new JsonCamelCaseResult(credentials.Keys.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}