namespace Macodaic.App.Core.Services
{
    public interface IWholesalerService
    {
        void Tick();
        
        void SetWholesaleOrgangePrice();

        decimal GetWholesaleOrgangePrice();
    }
}
