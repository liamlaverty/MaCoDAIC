namespace Macodaic.App.Core.Services.Impl
{
    public class WholesalerService : IWholesalerService
    {
        private decimal _wholesaleOrgangePrice { get; set; }

        public WholesalerService()
        {
            
        }

        public void Load(decimal initWholesaleOrgangePrice)
        {
            _wholesaleOrgangePrice = initWholesaleOrgangePrice;
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
