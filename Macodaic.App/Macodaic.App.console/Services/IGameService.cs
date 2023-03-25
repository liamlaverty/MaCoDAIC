using Macodaic.App.console.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.console.Services
{
    internal interface IGameService
    {

        void Load(int numberVendors, int numberConsumers);
        void Start();
        void Stop();
        void Report();
    }
}
