﻿namespace Macodaic.App.Core.Services.Impl
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
        

        public GameService(ITickService tickService, IEconomyService economyService,
            IReportService reportService, IVendorService vendorService, 
            IConsumerService consumerService)
        {
            _tickService = tickService;
            _economyService = economyService;
            _reportService = reportService;
            _vendorService = vendorService;
            _consumerService = consumerService;
        }

        public void Load(int numberVendors, int numberConsumers)
        {
            Console.WriteLine($"Game.Load called");
            _vendorService.Load(numberVendors);
            _consumerService.Load(numberConsumers);
            __running__ = true;
        }

        public void Report()
        {
            Console.WriteLine($"\n\ncurrent tick: {_tickService.Ticks}");
            _reportService.ReportAllEntities();
        }

        public void Start()
        {
            while (__running__)
            {
                _tickService.TickAllEntities();
                Report();

                if (_tickService.Ticks == _MAX_TICKS_)
                {
                    Stop();
                }
            }
            Stop();
        }

        public void Stop()
        {
            Console.WriteLine($"Game.Stop called");
            __running__ = false;
        }
    }
}
