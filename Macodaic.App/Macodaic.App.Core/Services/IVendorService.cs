using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services
{
    public interface IVendorService
    {
        void Load(int vendorCount, decimal vendorInitialFunds);
        List<VendorOffer> GetPriceList();
        void Tick();
        bool CanSatisfyRequest(ConsumerPurchaseRequest request);
        void SatisfyTransactionOnVendorSide(ConsumerPurchaseRequest request);
    }

}
