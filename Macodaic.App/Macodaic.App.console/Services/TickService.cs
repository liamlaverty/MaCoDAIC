using Macodaic.App.console.Models;

namespace Macodaic.App.console.Services
{
    internal class TickService
    {
        public int Ticks { get; private set; }
        public List<TickableEntity> TickableEntities { get; private set; } 

        public TickService()
        {
            this.Ticks = 0;
            TickableEntities = new List<TickableEntity>();

        }
        public void UpdateTick()
        {
            Ticks++;
            for (int i = 0; i < TickableEntities.Count; i++)
            {
                TickableEntities[i].Tick();
            }
        }

        public void AddTickableEntities(TickableEntity tickableEntity)
        {
            TickableEntities.Add(tickableEntity);
        }
    }
}
