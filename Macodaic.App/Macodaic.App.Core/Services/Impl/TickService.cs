using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services.Impl
{
    public class TickService : ITickService
    {
        private readonly IConsumerService _consumerService;
        private readonly IVendorService _vendorService;

        public Guid Id => new Guid();
        public int CurrentTick { get; private set; } = 0;
        public List<ITickableEntity> TickableEntities { get; private set; }
        

        public TickService(IConsumerService consumerService, 
            IVendorService vendorService)
        {
            TickableEntities = new List<ITickableEntity>();
            _consumerService = consumerService;
            _vendorService = vendorService;
        }
        public void TickAllEntities()
        {
            for (int i = 0; i < TickableEntities.Count; i++)
            {
                TickableEntities[i].Tick();
            }
        }

        public void UpdateTick()
        {
            CurrentTick++;
        }

        public void TickServices()
        {
            _vendorService.Tick();
            _consumerService.Tick();

        }

        public void RegisterTickableEntity(ITickableEntity tickableEntity)
        {
            TickableEntities.Add(tickableEntity);
        }
    }
}
