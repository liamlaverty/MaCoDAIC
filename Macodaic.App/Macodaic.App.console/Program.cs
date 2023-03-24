// See https://aka.ms/new-console-template for more information

using Macodaic.App.console.Services;

EconomyService _economyService;
TickService _tickService;   


bool __running__ = true;

Load();
Start();


void Load()
{
    _tickService = new TickService();
    _economyService = new EconomyService(vendorCount: 5, consumerCount: 100);

}

void Start()
{
    while (__running__)
    {

        Report();

        if (_tickService.Ticks == 100)
        {
            Stop();
        }
        _tickService.UpdateTick();
    }
    Stop();
}


void Stop()
{
    __running__ = false;
}

void Report()
{
    Console.WriteLine($"current tick: {_tickService.Ticks}");
    Console.WriteLine($"Economy has : {_economyService.TheEconomy.vendorAgents.Count()}");
    Console.WriteLine($"Economy has : {_economyService.TheEconomy.consumerAgents.Count()}");

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