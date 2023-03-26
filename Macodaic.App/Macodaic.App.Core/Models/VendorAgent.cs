namespace Macodaic.App.Core.Models
{
    internal class VendorAgent : Agent
    {
        internal int OrangeInventory { get; private set; }
        internal decimal MarginalCostOfOperations { get; }
        internal decimal AvailableFunds { get; private set; }

        private decimal _currentPrice = 0;
        private decimal _recentPriceOfOranges = 0;

        
 

        public VendorAgent(decimal initialFunds)
        {
            AvailableFunds = initialFunds;
            MarginalCostOfOperations = (decimal)0.25;
        }

        public override void Tick()
        {

            base.Tick();
        }

        public void PurchaseOranges (decimal wholesalerPrice)
        {

        }

        /// <summary>
        ///  Sets the current price of oranges
        /// </summary>
        public void SetCurrentPrice()
        {
            _currentPrice = MarginalCostOfOperations + _recentPriceOfOranges;
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
