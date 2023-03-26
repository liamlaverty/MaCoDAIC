using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services.Impl
{

    public class EconomyService : IEconomyService
    {
        private readonly ITickService _tickService;
        private readonly IReportService _reportService;


        public Economy TheEconomy { get; private set; }


        public EconomyService(ITickService tickService,
            IReportService reportService)
        {
            _tickService = tickService;
            _reportService = reportService;


            TheEconomy = new Economy();
        }
    }
}
