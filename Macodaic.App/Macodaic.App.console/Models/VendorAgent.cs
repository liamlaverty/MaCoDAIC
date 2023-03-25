namespace Macodaic.App.console.Models
{
    internal class VendorAgent : Agent {

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
            Console.WriteLine($"Ticking <{nameof(VendorAgent)}>");
            base.Report();
        }
    
    }
}
