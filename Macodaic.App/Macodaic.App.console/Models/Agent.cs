namespace Macodaic.App.console.Models
{
    internal class Agent : TickableEntity {
        public override void Tick()
        {
            Console.WriteLine("Ticking <Agent>");
            base.Tick();
        }

    }
}
