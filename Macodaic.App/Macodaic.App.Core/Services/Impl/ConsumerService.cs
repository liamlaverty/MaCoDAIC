using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services.Impl
{
    public class ConsumerService : IConsumerService
    {
        private readonly IReportService _reportService;
        private readonly IVendorService _vendorService;

        internal List<ConsumerAgent> consumerAgents { get; private set; }

        public ConsumerService(IReportService reportService, IVendorService vendorService)
        {
            _reportService = reportService;
            consumerAgents = new List<ConsumerAgent>();
            _vendorService = vendorService;
        }
        public void Load(int consumerCount)
        {
            consumerAgents = new List<ConsumerAgent>(new ConsumerAgent[consumerCount]);
            for (int i = 0; i < consumerAgents.Count; i++)
            {
                var consumer = new ConsumerAgent();
                consumerAgents[i] = consumer;
                _reportService.RegisterReportableEntity(consumer);
            }
        }

        public void Tick()
        {
            List<VendorOffer> vendorPriceList = _vendorService.GetPriceList();

            for (int i = 0; i < consumerAgents.Count; i++)
            {
                consumerAgents[i].Tick();
                List<ConsumerPurchaseRequest> consumerTransactionPreferences = 
                    consumerAgents[i].GenerateTransactionPreferences(vendorPriceList);

                foreach (var request in consumerTransactionPreferences)
                {
                    bool satisfiable = _vendorService.CanSatisfyRequest(request);
                    if (satisfiable)
                    {
                        _vendorService.SatisfyTransactionOnVendorSide(request);
                        consumerAgents[i].SatisfyTransactionOnConsumerSide(request);
                    }
                    else
                    {
                        throw new Exception($"A {nameof(ConsumerPurchaseRequest)} could not be satisfied");
                    }
                }


                consumerAgents[i].ConsumeOranges();
                consumerAgents[i].RestoreMarginalUtility();
            }
        }
    }
}
