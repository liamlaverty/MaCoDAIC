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
    _economyService = new EconomyService(_tickService, vendorCount: 1, consumerCount: 2);

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