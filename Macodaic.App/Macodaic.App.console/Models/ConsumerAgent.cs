namespace Macodaic.App.console.Models
{
    internal class ConsumerAgent : Agent
    {
        internal int Oranges { get; private set; }

        private decimal MarginalUtility { get; set; }

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
            MarginalUtility = 1;
        }


        public override void Tick()
        {
            RestoreMarginalUtility();
            ConsumeOranges();

            base.Tick();
        }

        public override void Report()
        {
            Console.WriteLine($"{nameof(ConsumerAgent)}:{Id} | {nameof(MarginalUtility)}:{MarginalUtility} | {nameof(AvailableFunds)}:${AvailableFunds}");
            base.Report();
  
        }

        /// <summary>
        /// A method at the end of each tick which slightly re-increases
        /// marginal utility before the next round begins
        /// </summary>
        private void RestoreMarginalUtility()
        {
            MarginalUtility += (decimal)0.3;
            if (MarginalUtility > 1) { MarginalUtility = 1; }
        }


        /// <summary>
        /// Consumes oranges, reduces the amount of marginal utility gained from 
        /// each additional orange consumed. If marginal utility is at or below 0, 
        /// </summary>
        public void ConsumeOranges()
        {
            while (Oranges > 0)
            {
                Utility+= (Utility * MarginalUtility);
                MarginalUtility -= (decimal)0.2;
                Oranges--;

                if (MarginalUtility <= 0)
                {
                    break;
                }
            }
        }       

       
    }
}
