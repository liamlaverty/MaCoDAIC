namespace Macodaic.App.Core.Models
{
    internal class VendorAgent : Agent
    {
        internal int OrangeInventory { get; private set; }
        internal decimal MarginalCostOfOperations { get; }
        internal decimal AvailableFunds { get; private set; }

        private decimal _currentPrice=0;

        /// <summary>
        ///  list of prices of oranges from the wholesaler
        /// </summary>
        private List<decimal> _recentWholesalePriceOfOranges;

        
 

        public VendorAgent(decimal initialFunds)
        {
            AvailableFunds = initialFunds;
            MarginalCostOfOperations = (decimal)0.25;
            _recentWholesalePriceOfOranges = new List<decimal>();
        }

        public override void Tick()
        {

            base.Tick();
        }

        public void PurchaseOranges (decimal wholesalerPrice)
        {
            _recentWholesalePriceOfOranges.Add(wholesalerPrice);
            if (AvailableFunds > wholesalerPrice)
            {

            }
        }



        /// <summary>
        ///  Sets the current price of oranges
        /// </summary>
        public void SetCurrentPrice()
        {
            _currentPrice = MarginalCostOfOperations + _recentWholesalePriceOfOranges.Last();
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
