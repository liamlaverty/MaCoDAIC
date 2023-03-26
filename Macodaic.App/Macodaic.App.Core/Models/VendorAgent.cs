namespace Macodaic.App.Core.Models
{
    internal class VendorAgent : Agent
    {
        internal int OrangeInventory { get; private set; }
        internal decimal MarginalCostOfOperations { get; }
        internal decimal AvailableFunds { get; private set; }

        
 

        public VendorAgent(decimal initialFunds)
        {
            AvailableFunds = initialFunds;
            MarginalCostOfOperations = (decimal)0.25;
        }

        public override void Tick()
        {

            base.Tick();
        }

      

        /// <summary>
        /// 
        /// </summary>
        public override void Report()
        {
            Console.WriteLine($"{nameof(VendorAgent)}:{Id} | {nameof(AvailableFunds)}:${AvailableFunds} | {nameof(OrangeInventory)}:{OrangeInventory}");
            base.Report();
        }


    }
}
