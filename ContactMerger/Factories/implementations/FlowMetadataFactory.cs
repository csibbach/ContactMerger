using ContactMerger.Utility;

namespace ContactMerger.Factories.implementations
{
    public class FlowMetadataFactory: IFlowMetadataFactory
    {
        public AppFlowMetadata CreateFlowMetadata()
        {
            return new AppFlowMetadata();
        }
    }
}