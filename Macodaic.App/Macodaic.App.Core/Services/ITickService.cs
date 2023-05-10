using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services
{
    public interface ITickService    {
        
        List<ITickableEntity> TickableEntities { get; }
        int CurrentTick { get; }

        void RegisterTickableEntity(ITickableEntity tickableEntity);
        public void UpdateTick();

        void TickAllEntities();
        void TickServices();
    }
}
