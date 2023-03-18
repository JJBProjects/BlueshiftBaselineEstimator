using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueshiftBaselineEstimator.Logic
{
    internal class LogicHelpers
    {
        /// <summary>
        /// Truncates input to three decimal places then rounds to two decimal places. This is done to override default rounding behaviour of .4999 (recurring) rounding up to 1.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Value rounded to two decimal places</returns>
        public static double Round(double input)
        {
            var truncatedInput = Math.Truncate(1000 * input) / 1000;
            return Math.Round(truncatedInput, 2, MidpointRounding.AwayFromZero);
        }
    }
}
