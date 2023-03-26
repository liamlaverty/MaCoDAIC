using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services.Impl
{
    public class VendorService : IVendorService
    {
        private readonly ITickService _tickService;
        private readonly IReportService _reportService;

        internal List<VendorAgent> vendorAgents { get; private set; }


        public VendorService(ITickService tickService,
            IReportService reportService)
        {
            _tickService = tickService;
            _reportService = reportService;

            vendorAgents = new List<VendorAgent>();
        }


        public void Load(int vendorCount)
        {
            vendorAgents = new List<VendorAgent>(new VendorAgent[vendorCount]);
            for (int i = 0; i < vendorAgents.Count; i++)
            {
                var vendor = new VendorAgent(500);
                vendorAgents[i] = vendor;
                _tickService.RegisterTickableEntity(vendor);
                _reportService.RegisterReportableEntity(vendor);
            }
        }


    }
}
