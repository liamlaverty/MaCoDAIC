namespace Macodaic.App.Core.Services
{
    public interface IGameService
    {

        void Load(int numberVendors, int numberConsumers);
        void Start();
        void Stop();
    }
}
