namespace Macodaic.App.console.Models
{
    internal class Agent : ITickableEntity, IReportableEntity
    {
        public virtual void Report()
        {
            //Console.WriteLine("Reoprting <Agent>");

        }

        public virtual void Tick()
        {
            //Console.WriteLine("Ticking <Agent>");
        }
    }
}
