namespace Macodaic.App.console.Services
{
    internal class TickService
    {
        public int Ticks { get; private set; }

        public TickService()
        {
            this.Ticks = 0;
        }
        public void UpdateTick()
        {
            Ticks++;
        }
    }
}
