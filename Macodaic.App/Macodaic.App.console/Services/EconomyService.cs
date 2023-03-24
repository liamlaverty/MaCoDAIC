using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Macodaic.App.console.Models;

namespace Macodaic.App.console.Services
{

   

    internal class EconomyService
    {
        public Economy TheEconomy {  get; private set; }

        public EconomyService(int vendorCount, int consumerCount)
        {
            TheEconomy = new Economy(vendorCount, consumerCount);
        }

     
    }
}
