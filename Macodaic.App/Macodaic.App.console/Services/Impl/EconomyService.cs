using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Macodaic.App.console.Models;

namespace Macodaic.App.console.Services.Impl
{

    internal class EconomyService : IEconomyService
    {
        private readonly ITickService _tickService;


        public Economy TheEconomy { get; private set; }

        public List<VendorAgent> vendorAgents { get; private set; }
        public List<ConsumerAgent> consumerAgents { get; private set; }

        public EconomyService(ITickService tickService)
        {
            _tickService = tickService;
            
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
                var vendor = new VendorAgent();
                vendorAgents[i] = vendor;
                _tickService.AddTickableEntities(vendor);

            }
            for (int i = 0; i < consumerAgents.Count; i++)
            {
                var consumer = new ConsumerAgent();
                consumerAgents[i] = consumer;
                _tickService.AddTickableEntities(consumer);
            }
        }
    }
}
