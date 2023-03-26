using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services.Impl
{
    public class ConsumerService : IConsumerService
    {
        private readonly ITickService _tickService;
        private readonly IReportService _reportService;

        internal List<ConsumerAgent> consumerAgents { get; private set; }

        public ConsumerService(ITickService tickService,
            IReportService reportService)
        {
            _tickService = tickService;
            _reportService = reportService;
            consumerAgents = new List<ConsumerAgent>();

        }
        public void Load(int consumerCount)
        {
            consumerAgents = new List<ConsumerAgent>(new ConsumerAgent[consumerCount]);
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
