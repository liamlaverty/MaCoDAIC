using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.Core.Helpers
{
    internal static class MathHelper
    {
        internal static decimal Lerp(this decimal lerpFrom, decimal lerpTo, decimal lerpBy)
        {
            return lerpFrom * (1 - lerpBy) + lerpTo * lerpBy;
        }

    }
}
