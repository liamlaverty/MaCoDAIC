using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.console.Models
{
    internal class TickableEntity
    {
        public virtual void Tick()
        {
            Console.WriteLine("Ticking <TickableEntities>");

        }
    }
}
