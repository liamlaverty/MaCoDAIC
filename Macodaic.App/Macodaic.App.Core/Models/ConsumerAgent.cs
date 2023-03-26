using Macodaic.App.Core.Helpers;

namespace Macodaic.App.Core.Models

{
    internal class ConsumerAgent : Agent
    {
        internal int Oranges { get; private set; }

        private decimal MarginalUtilityOfOranges { get; set; }

        /// <summary>
        /// tracks the amount of money this consumer has
        /// </summary>
        private decimal AvailableFunds { get; set; }

        /// <summary>
        /// Tracks the amount of utility this consumer has
        /// </summary>
        internal decimal Utility { get; private set; }



        public ConsumerAgent()
        {
            MarginalUtilityOfOranges = 1m;
            AvailableFunds = 50m;
            Oranges = 3;
        }


        public override void Report()
        {
            Console.WriteLine($"{nameof(ConsumerAgent)}:{Id} | {nameof(Oranges)}:{Oranges} | {nameof(MarginalUtilityOfOranges)}:{MarginalUtilityOfOranges} | {nameof(Utility)}:{Utility} | {nameof(AvailableFunds)}:${AvailableFunds}");
            base.Report();
  
        }

        /// <summary>
        /// A method at the start of each tick which slightly re-increases
        /// marginal utility before the next round begins. 
        /// 
        /// Uses LERPing to increase by 50% each time
        /// </summary>
        public void RestoreMarginalUtility()
        {
            MarginalUtilityOfOranges = MarginalUtilityOfOranges.Lerp(lerpTo: 1, lerpBy: 0.25m);
            if (MarginalUtilityOfOranges > 0.99m) { MarginalUtilityOfOranges = 1; }
        }

        /// <summary>
        ///  Reduces the Agen's marginal utility of oranges by 0.2
        /// </summary>
        private void DepleteMarginalUtilityOfOranges() 
        {
            MarginalUtilityOfOranges -= (decimal)0.2;
            if (MarginalUtilityOfOranges < 0) { MarginalUtilityOfOranges = 0; }
        }

        /// <summary>
        /// Gradually depletes the agent's utility
        /// </summary>
        private void DepleteUtility()
        {
            Utility.Lerp(lerpTo: 0, lerpBy: 0.1m);
        }
        private void IncreaseUtility()
        {
            Utility += MarginalUtilityOfOranges;
        }


        /// <summary>
        /// The consumer picks from the list of vendors the 
        /// 
        /// </summary>
        /// <param name="vendorPriceList"></param>
        /// <returns></returns>
        public List<ConsumerPurchaseRequest> GenerateTransactionPreferences(List<VendorOffer> vendorPriceList)
        {
            var result = new List<ConsumerPurchaseRequest>();
            if (AvailableFunds > 0)
            {
                if (vendorPriceList.Any(c => c.PricePerOrange < AvailableFunds))
                {
                    int quantityToPurchase = 1;
                    var vendorOfferPreference = vendorPriceList.First(c => c.PricePerOrange < AvailableFunds);
                    result.Add(new ConsumerPurchaseRequest(
                        Id, 
                        vendorOfferPreference.VendorId,
                        quantityToPurchase,
                        quantityToPurchase * vendorOfferPreference.PricePerOrange));
                }
            }
            return result;
        }



        /// <summary>
        /// Consumes oranges, reduces the amount of marginal utility gained from 
        /// each additional orange consumed. If marginal utility is at or below 0, 
        /// </summary>
        public void ConsumeOranges()
        {
            while (Oranges > 0)
            {
                // Utility+= (Utility * MarginalUtility);
                IncreaseUtility();
                DepleteMarginalUtilityOfOranges();
                Oranges--;

                if (MarginalUtilityOfOranges <= 0)
                {
                    MarginalUtilityOfOranges = 0.00001m;
                    break;
                }
            }
        }



        /// <summary>
        /// Completes a transaction, subtracts AvailableFunds, and adds 
        /// to the number of oranges
        /// </summary>
        /// <param name="request"></param>
        internal void SatisfyTransaction(ConsumerPurchaseRequest request)
        {
            AvailableFunds -= request.ExpectedPrice;
            if (AvailableFunds < 0)
            {
                throw new Exception("Transaction took a consumer below $0.00 in funds");
            }
            Oranges += request.NumberOfOrangesRequested;
        }
    }
}
