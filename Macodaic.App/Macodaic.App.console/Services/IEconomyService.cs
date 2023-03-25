using Macodaic.App.console.Models;

namespace Macodaic.App.console.Services
{
    internal interface IEconomyService
    {
        List<ConsumerAgent> consumerAgents { get; }
        Economy TheEconomy { get; }
        List<VendorAgent> vendorAgents { get; }
        
        void Load(int vendorCount, int consumerCount);
    }
}