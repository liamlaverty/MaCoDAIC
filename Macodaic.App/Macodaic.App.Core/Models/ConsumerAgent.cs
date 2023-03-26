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


        public override void Tick()
        {
            // DepleteUtility();
            ConsumeOranges();
            RestoreMarginalUtility();

            base.Tick();
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
        private void RestoreMarginalUtility()
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

       
    }
}
