namespace Macodaic.App.console.Models
{
    internal class ConsumerAgent : Agent
    {
        public Guid Id { get; } = new Guid();
        internal int Oranges { get; private set; }

        private decimal MarginalUtility { get; set; }

        /// <summary>
        /// tracks the amount of money this consumer has
        /// </summary>
        internal decimal AvailableFunds { get; private set; }

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
            Console.WriteLine("Ticking <ConsumerAgent>");
            RestoreMarginalUtility();
            ConsumeOranges();

            base.Tick();
        }

        public override void Report()
        {
            Console.WriteLine($"Consumer has {MarginalUtility} marginal utility");
            Console.WriteLine($"Consumer has {AvailableFunds} available funds");
        }

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
