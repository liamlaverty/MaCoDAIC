namespace Macodaic.App.Core.Services
{
    public interface IWholesalerService
    {
        void Load(decimal initWholesaleOrgangePrice);
        void Tick();
        
        void SetWholesaleOrgangePrice();

        decimal GetWholesaleOrgangePrice();
    }
}
