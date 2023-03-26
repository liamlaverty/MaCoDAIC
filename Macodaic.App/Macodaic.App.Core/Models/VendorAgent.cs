namespace Macodaic.App.Core.Models
{
    internal class VendorAgent : Agent
    {
        internal int OrangeInventory { get; private set; }
        internal int TotalOrangesSold { get; private set; }
        internal decimal MarginalCostOfOperations { get; private set; }
        internal decimal BaseMarginalCostOfOperations { get; }
        internal decimal AvailableFunds { get; private set; }

        private decimal _currentPrice=0;

        /// <summary>
        ///  list of prices of oranges from the wholesaler
        /// </summary>
        internal List<decimal> _recentWholesalePriceOfOranges;


        /// <summary>
        /// 
        /// </summary>
        public override void Report()
        {
            Console.WriteLine($"{nameof(VendorAgent)}:{Id} | {nameof(AvailableFunds)}:${AvailableFunds} | {nameof(_currentPrice)}:${_currentPrice.ToString("0.##")} | {nameof(OrangeInventory)}:{OrangeInventory} | {nameof(TotalOrangesSold)}:{TotalOrangesSold}");
            base.Report();
        }

        public VendorAgent(decimal initialFunds)
        {
            AvailableFunds = initialFunds;
            BaseMarginalCostOfOperations = (decimal)0.25;
            MarginalCostOfOperations = BaseMarginalCostOfOperations;
            _recentWholesalePriceOfOranges = new List<decimal>();
        }

        public override void Tick()
        {
            Random r = new Random();
            // moves the marginal cost of operations slightly for each vendor
            decimal random = (decimal)(r.Next(90, 110) * 0.01);
            MarginalCostOfOperations = BaseMarginalCostOfOperations * random;
            base.Tick();
        }


        /// <summary>
        ///  purchases the maximum number of oranges possible from the 
        ///  wholesaler
        /// </summary>
        /// <param name="wholesalerPrice"></param>
        public void PurchaseOrangesFromWholesaler (decimal wholesalerPrice)
        {
            _recentWholesalePriceOfOranges.Add(wholesalerPrice);
            if (AvailableFunds > wholesalerPrice)
            {
                int numOrangesToPurchase = Convert.ToInt32(Math.Floor(AvailableFunds / wholesalerPrice));
                //if ( numOrangesToPurchase > 50) {
                //    numOrangesToPurchase = 50;
                //}
                AvailableFunds -= numOrangesToPurchase * wholesalerPrice;
                OrangeInventory += numOrangesToPurchase; 
            }
        }

       
        public void VendOranges(int numberOranges, decimal expectedTotalPriceOfTransaction)
        {
            if (numberOranges * _currentPrice != expectedTotalPriceOfTransaction)
            {
                throw new ArgumentException($"price requested ${expectedTotalPriceOfTransaction} was incorrect for the requested quantity of oranges ({numberOranges}). Calculated price was {numberOranges * _currentPrice}");
            }
            OrangeInventory -= numberOranges;
            TotalOrangesSold += numberOranges;
            AvailableFunds += expectedTotalPriceOfTransaction;
        }


        /// <summary>
        ///  Sets the current price of oranges
        /// </summary>
        public void SetCurrentPrice()
        {
            _currentPrice = MarginalCostOfOperations + _recentWholesalePriceOfOranges.Last();
        }
        public decimal GetCurrentPrice(){ return _currentPrice; }

     


    }
}
