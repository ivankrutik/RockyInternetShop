using Braintree;

namespace RockyUtility.BrainTreeDomain
{
    public interface IBrainTreeGate
    {
        public IBraintreeGateway CreateGateWay();

        public IBraintreeGateway GetGateWay();
    }
}
