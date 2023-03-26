using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.Core.Helpers
{
    internal static class MathHelper
    {

        /// <summary>
        ///  LERPs between to values, by a given amount
        /// </summary>
        /// <param name="lerpFrom"></param>
        /// <param name="lerpTo"></param>
        /// <param name="lerpBy"></param>
        /// <returns></returns>
        internal static decimal Lerp(this decimal lerpFrom, decimal lerpTo, decimal lerpBy)
        {
            return lerpFrom * (1 - lerpBy) + lerpTo * lerpBy;
        }

    }
}
