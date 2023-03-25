using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services.Impl
{

    public class EconomyService : IEconomyService
    {
        private readonly ITickService _tickService;
        private readonly IReportService _reportService;


        public Economy TheEconomy { get; private set; }

        internal List<VendorAgent> vendorAgents { get; private set; }
        internal List<ConsumerAgent> consumerAgents { get; private set; }

        public EconomyService(ITickService tickService,
            IReportService reportService)
        {
            _tickService = tickService;
            _reportService = reportService;


            TheEconomy = new Economy();
            vendorAgents = new List<VendorAgent>();
            consumerAgents = new List<ConsumerAgent>();
        }

        public void Load(int vendorCount = 2, int consumerCount = 10)
        {
            vendorAgents = new List<VendorAgent>(new VendorAgent[vendorCount]);
            consumerAgents = new List<ConsumerAgent>(new ConsumerAgent[consumerCount]);

            for (int i = 0; i < vendorAgents.Count; i++)
            {
                var vendor = new VendorAgent(500);
                vendorAgents[i] = vendor;
                _tickService.RegisterTickableEntity(vendor);
                _reportService.RegisterReportableEntity(vendor);

            }
            for (int i = 0; i < consumerAgents.Count; i++)
            {
                var consumer = new ConsumerAgent();
                consumerAgents[i] = consumer;
                _tickService.RegisterTickableEntity(consumer);
                _reportService.RegisterReportableEntity(consumer);

            }
        }
    }
}
