using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services
{
    public interface IEconomyService
    {
        Economy TheEconomy { get; }
        
        void Load(int vendorCount, int consumerCount);
    }
}