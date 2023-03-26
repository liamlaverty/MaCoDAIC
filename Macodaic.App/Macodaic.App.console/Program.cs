// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Macodaic.App.Core.Services;
using Macodaic.App.Core.Services.Impl;

Console.WriteLine($"Main starting");


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging();
        services.AddSingleton<ITickService, TickService>();
        services.AddSingleton<IReportService, ReportService>();
        services.AddSingleton<IWholesalerService, WholesalerService>();
        services.AddSingleton<IVendorService, VendorService>();
        services.AddSingleton<IConsumerService, ConsumerService>();
        services.AddSingleton<IGameService, GameService>();
        services.AddScoped<IEconomyService, EconomyService>();
        services.BuildServiceProvider();
    })
    .Build();


var _gameService = host.Services.GetService<IGameService>();
_gameService.Load(numberVendors: 0, numberConsumers: 1);
_gameService.Start();