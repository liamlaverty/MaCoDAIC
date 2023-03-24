namespace Macodaic.App.console.Models
{

    public class Economy
    {
        public IEnumerable<VendorAgent> vendorAgents { get; private set; }
        public IEnumerable<ConsumerAgent> consumerAgents { get; private set; }

        public Economy(int countVendors, int countConsumers)
        {
            vendorAgents = new List<VendorAgent>(new VendorAgent[countVendors]);
            consumerAgents = new List<ConsumerAgent>(new ConsumerAgent[countConsumers]); 
        }
    }
}
