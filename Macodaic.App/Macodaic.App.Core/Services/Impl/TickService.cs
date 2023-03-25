using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services.Impl
{
    public class TickService : ITickService
    {
        public Guid Id => new Guid();
        public int Ticks { get; private set; } = 0;
        public List<ITickableEntity> TickableEntities { get; private set; }


        public TickService()
        {
            TickableEntities = new List<ITickableEntity>();

        }
        public void TickAllEntities()
        {
            Ticks++;
            for (int i = 0; i < TickableEntities.Count; i++)
            {
                TickableEntities[i].Tick();
            }
        }

        public void RegisterTickableEntity(ITickableEntity tickableEntity)
        {
            TickableEntities.Add(tickableEntity);
        }
    }
}
