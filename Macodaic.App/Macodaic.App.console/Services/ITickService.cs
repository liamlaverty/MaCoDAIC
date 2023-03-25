using Macodaic.App.console.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.console.Services
{
    internal interface ITickService    {
        
        List<ITickableEntity> TickableEntities { get; }
        int Ticks { get; }

        void RegisterTickableEntity(ITickableEntity tickableEntity);
        void TickAllEntities();
    }
}
