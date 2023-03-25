// See https://aka.ms/new-console-template for more information

using Macodaic.App.console.Services;
using Macodaic.App.console.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine($"Main starting");


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging();
        services.AddSingleton<ITickService, TickService>();
        services.AddSingleton<IGameService, GameService>();
        services.AddScoped<IEconomyService, EconomyService>();
        services.BuildServiceProvider();
    })
    .Build();


var _gameService = host.Services.GetService<IGameService>();
_gameService.Load(numberVendors: 5, numberConsumers: 10);
_gameService.Start();