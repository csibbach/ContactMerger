using ContactMerger.Utility;

namespace ContactMerger.Factories.contracts
{
    public interface IFlowMetadataFactory
    {
        AppFlowMetadata CreateFlowMetadata();

        void RequestNewAccount();
    }
}
