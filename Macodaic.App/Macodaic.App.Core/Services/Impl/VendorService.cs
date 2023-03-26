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


        /// <summary>
        ///  For each vendor, calls SetPrices, then collects all vendor pices
        ///  into a dictionary
        /// </summary>
        public void Tick()
        {
            decimal wholesalePrice = _wholesalerService.GetWholesaleOrgangePrice();
            for (int i = 0; i < vendorAgents.Count; i++)
            {
                vendorAgents[i].PurchaseOranges(wholesalePrice);

                vendorAgents[i].SetCurrentPrice();
            }
        }
    }
}
