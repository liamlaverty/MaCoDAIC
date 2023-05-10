namespace Macodaic.App.Core.Services.Impl
{
    public class GameService : IGameService
    {
        private readonly ITickService _tickService;
        private readonly IEconomyService _economyService;
        private readonly IReportService _reportService;


        private readonly IVendorService _vendorService;
        private readonly IWholesalerService _wholesalerService;
        private readonly IConsumerService _consumerService;
        private bool __running__ = false;
        private readonly int _MAX_TICKS_ = 100;
        private readonly int _NUM_VENDORS = 5;
        private readonly int _NUM_CONSUMERS = 100;
        private readonly int _VENDOR_INIT_FUNDS_ = 1000;
        private readonly decimal _WHOLESALER_ORANGE_PRICE = 3.36m;


        public GameService(ITickService tickService, IEconomyService economyService,
            IReportService reportService, IVendorService vendorService, 
            IConsumerService consumerService, IWholesalerService wholesalerService)
        {
            _tickService = tickService;
            _economyService = economyService;
            _reportService = reportService;
            _vendorService = vendorService;
            _consumerService = consumerService;
            _wholesalerService = wholesalerService;
        }

        public void Load()
        {
            Console.WriteLine($"Game.Load called");
            _wholesalerService.Load(_WHOLESALER_ORANGE_PRICE);
            _vendorService.Load(_NUM_VENDORS, _VENDOR_INIT_FUNDS_);
            _consumerService.Load(_NUM_CONSUMERS);
            _reportService.ReportAllEntities();
            __running__ = true;
        }

        public void Start()
        {
            while (__running__)
            {
                Console.WriteLine($"\n----LOOP START REPORT {_tickService.CurrentTick}----");

                // _tickService.TickAllEntities();
                _reportService.ReportAllEntities();

                _tickService.TickServices();
                Console.WriteLine($"\n----LOOP END REPORT {_tickService.CurrentTick}----");

                _reportService.ReportAllEntities();




                if (_tickService.CurrentTick == _MAX_TICKS_)
                {
                    Stop();
                }
                _tickService.UpdateTick();
            }
        }

        public void Stop()
        {
            Console.WriteLine($"Game.Stop called");
            __running__ = false;
        }
    }
}
