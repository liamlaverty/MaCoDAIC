using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.console.Models
{
    public interface IReportableEntity 
    {
        public abstract void Report();
    }

    public interface ITickableEntity
    {
        public abstract void Tick();
    }
}
