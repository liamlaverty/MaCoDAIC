namespace Macodaic.App.console.Models
{
    internal class VendorAgent : Agent {


        private decimal AvailableFunds { get; set; }

        public VendorAgent(decimal initialFunds)
        {
            AvailableFunds = initialFunds;
        }

        public override void Tick()
        {
            // Console.WriteLine($"Ticking <{nameof(VendorAgent)}>");


            base.Tick();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Report()
        {
            Console.WriteLine($"{nameof(VendorAgent)}:{Id} | {nameof(AvailableFunds)}:{AvailableFunds}");
            base.Report();
        }
    
    }
}
