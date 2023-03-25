using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services
{
    public interface ITickService    {
        
        List<ITickableEntity> TickableEntities { get; }
        int Ticks { get; }

        void RegisterTickableEntity(ITickableEntity tickableEntity);
        void TickAllEntities();
    }
}
