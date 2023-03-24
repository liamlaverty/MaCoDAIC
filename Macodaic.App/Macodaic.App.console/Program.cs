// See https://aka.ms/new-console-template for more information

using Macodaic.App.console.Services;
using Macodaic.App.console.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// private readonly IEconomyService _economyService;
 // private readonly ITickService _tickService;   


bool __running__ = true;


using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ITickService, TickService>();
        services.AddSingleton<IGameService, GameService>();
        services.AddScoped<IEconomyService, EconomyService>();
    })
    .Build();

await host.RunAsync();

Load();
Start();


void Load()
{
    // _economyService = new EconomyService(_tickService, vendorCount: 1, consumerCount: 2);

}

void Start()
{
    while (__running__)
    {
        _tickService.UpdateTick();
        Report();

        if (_tickService.Ticks == 100)
        {
            Stop();
        }
    }
    Stop();
}


void Stop()
{
    __running__ = false;
}

void Report()
{
    Console.WriteLine($"\n\ncurrent tick: {_tickService.Ticks}");
    Console.WriteLine($"Economy has : {_economyService.vendorAgents.Count()} vendors");
    Console.WriteLine($"Economy has : {_economyService.consumerAgents.Count()} consumers");

}

void Loop(int loopCount)
{
    Console.WriteLine($"current tick: {loopCount}");

    
    
    
    if (loopCount == 100)
    {
        __running__ = false;
        return;
    } 
    Loop(++loopCount);
}