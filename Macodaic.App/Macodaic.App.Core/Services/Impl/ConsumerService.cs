using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services.Impl
{
    public class ConsumerService : IConsumerService
    {
        private readonly IReportService _reportService;

        internal List<ConsumerAgent> consumerAgents { get; private set; }

        public ConsumerService(IReportService reportService)
        {
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
                _reportService.RegisterReportableEntity(consumer);
            }
        }

        public void Tick()
        {
            for (int i = 0; i < consumerAgents.Count; i++)
            {
                consumerAgents[i].Tick();
            }
        }
    }
}
