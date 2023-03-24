namespace Macodaic.App.console.Models
{
    internal class ConsumerAgent : Agent {

        public override void Tick()
        {
            Console.WriteLine("Ticking <ConsumerAgent>");
            base.Tick();
        }
    }
}
