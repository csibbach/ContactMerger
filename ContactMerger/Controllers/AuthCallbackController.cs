using System.Diagnostics.CodeAnalysis;
using ContactMerger.Factories.contracts;

namespace ContactMerger.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// This class I'm excluding as it's just Google's code
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        private readonly IFlowMetadataFactory _flowMetadataFactory;

        public AuthCallbackController(IFlowMetadataFactory flowMetadataFactory)
        {
            _flowMetadataFactory = flowMetadataFactory;
        }

        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData => _flowMetadataFactory.CreateFlowMetadata();
    }
}