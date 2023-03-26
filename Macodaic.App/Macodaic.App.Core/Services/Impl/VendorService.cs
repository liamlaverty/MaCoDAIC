using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services.Impl
{
    public class VendorService : IVendorService
    {
        private readonly IReportService _reportService;
        private readonly IWholesalerService _wholesalerService;

        internal List<VendorAgent> vendorAgents { get; private set; }


        public VendorService(IReportService reportService,
            IWholesalerService wholesalerService)
        {
            _reportService = reportService;

            vendorAgents = new List<VendorAgent>();
            _wholesalerService = wholesalerService;
        }


        public void Load(int vendorCount)
        {
            vendorAgents = new List<VendorAgent>(new VendorAgent[vendorCount]);
            for (int i = 0; i < vendorAgents.Count; i++)
            {
                var vendor = new VendorAgent(25);
                vendorAgents[i] = vendor;
                _reportService.RegisterReportableEntity(vendor);
            }
        }

        public List<VendorOffer> GetPriceList()
        {
            return vendorAgents.Select(
                c => new VendorOffer(c.Id, c.OrangeInventory, c.GetCurrentPrice()))
                .ToList();
        }


        /// <summary>
        ///  For each vendor, calls SetPrices, then collects all vendor pices
        ///  into a dictionary
        /// </summary>
        public void Tick()
        {
            decimal wholesalePrice = _wholesalerService.GetWholesaleOrgangePrice();
            for (int i = 0; i < vendorAgents.Count; i++)
            {
                vendorAgents[i].Tick();
                vendorAgents[i].PurchaseOrangesFromWholesaler(wholesalePrice);

                vendorAgents[i].SetCurrentPrice();
            }
        }

        public bool CanSatisfyRequest(ConsumerPurchaseRequest request)
        {
            var vendor = vendorAgents.First(c => c.Id == request.VendorId);
            if (vendor == null)
            {
                throw new ArgumentNullException($"{nameof(vendor)} with ID {request.VendorId} was null");
            }
            if (vendor.OrangeInventory < request.NumberOfOrangesRequested)
            {
                return false;
            }
            return true;
        }

        public void SatisfyTransactionOnVendorSide(ConsumerPurchaseRequest request)
        {
            var vendor = vendorAgents.First(c => c.Id == request.VendorId);
            if (vendor == null)
            {
                throw new ArgumentNullException($"{nameof(vendor)} with ID {request.VendorId} was null");
            }
            vendor.VendOranges(request.NumberOfOrangesRequested, request.ExpectedTotalPriceOfTransaction);
        }
    }
}
