using Macodaic.App.console.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.console.Services
{
    public interface IReportServiceLifetime
    {
        Guid Id { get;  }
        ServiceLifetime Lifetime { get; }
    }


    internal interface ITickService : IReportServiceLifetime
    {
        ServiceLifetime IReportServiceLifetime.Lifetime => ServiceLifetime.Scoped;
        
        List<TickableEntity> TickableEntities { get; }
        int Ticks { get; }

        void AddTickableEntities(TickableEntity tickableEntity);
        void UpdateTick();
    }
}
