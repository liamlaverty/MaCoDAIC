// See https://aka.ms/new-console-template for more information

using Macodaic.App.console.Services;

EconomyService _economyService;
TickService _tickService;   


bool __running__ = true;

Load();
Start();


void Load()
{
    _economyService = new EconomyService(); 
    _tickService = new TickService(); 
}

void Start()
{
    while (__running__)
    {
        Console.WriteLine($"current tick: {_tickService.Ticks}");




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