using Macodaic.App.console.Models;

namespace Macodaic.App.console.Services.Impl
{
    internal class TickService : ITickService
    {
        public Guid Id => new Guid();
        public int Ticks { get; private set; }
        public List<TickableEntity> TickableEntities { get; private set; }


        public TickService()
        {
            Ticks = 0;
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
