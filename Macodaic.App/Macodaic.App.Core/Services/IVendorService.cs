using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services
{
    public interface IVendorService
    {
        void Load(int vendorCount);
        List<VendorOffer> GetPriceList();
        void Tick();
        bool CanSatisfyRequest(ConsumerPurchaseRequest request);
        void SatisfyRequest(ConsumerPurchaseRequest request);
    }

}
