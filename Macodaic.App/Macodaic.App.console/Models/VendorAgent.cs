namespace Macodaic.App.console.Models
{
    internal class VendorAgent : Agent 
    {
        internal int Oranges { get; private set; }

        private decimal MarginalCostOfOperations { get; }
        private decimal AvailableFunds { get; set; }

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
            Console.WriteLine($"{nameof(VendorAgent)}:{Id} | {nameof(AvailableFunds)}:${AvailableFunds}");
            base.Report();
        }
    
    }
}
