namespace Macodaic.App.console.Models
{
    internal class Agent : ITickableEntity, IReportableEntity
    {
        public Guid Id { get; }

        public Agent() 
        { 
            Id = Guid.NewGuid();
        }
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
