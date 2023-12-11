using Braintree;
using Microsoft.Extensions.Options;

namespace RockyUtility.BrainTreeDomain
{
    public class BrainTreeGate : IBrainTreeGate
    {
        private readonly BrainTreeSettings _settings;
        private IBraintreeGateway _gateway;

        public BrainTreeGate(IOptions<BrainTreeSettings> settings)
        {
            _settings = settings.Value;
        }

        public IBraintreeGateway CreateGateWay()
        {
            return new BraintreeGateway(_settings.Environment, _settings.MerchantId, _settings.PublicKey, _settings.PrivateKey);
        }

        public IBraintreeGateway GetGateWay()
        {
            if (_gateway == null)
            {
                _gateway = CreateGateWay();
            }
            return _gateway;
        }
    }
}
