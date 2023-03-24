using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.console.Services
{

    public class Economy
    {

    }

    public class Agent { }

    public class ConsumerAgent : Agent { }
    public class VendorAgent : Agent { }

    internal class EconomyService
    {
        public Economy TheEconomy {  get; private set; }
    }
}
