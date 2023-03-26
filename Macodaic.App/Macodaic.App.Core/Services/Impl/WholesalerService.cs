namespace Macodaic.App.Core.Services.Impl
{
    public class WholesalerService : IWholesalerService
    {
        private decimal _wholesaleOrgangePrice = 3.36m;
        public WholesalerService()
        {
            
        }

        public void Tick()
        {
            SetWholesaleOrgangePrice();
        }

        /// <summary>
        /// sets the wholesale price of organges in the economy
        /// </summary>
        public void SetWholesaleOrgangePrice()
        {
            _wholesaleOrgangePrice= _wholesaleOrgangePrice * 1;
        }

        public decimal GetWholesaleOrgangePrice()
        {
            return _wholesaleOrgangePrice;
        }
    }
}
