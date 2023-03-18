using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueshiftBaselineEstimator.Logic
{
    public abstract class BaselineCalculator
    {
        public abstract double[] CalculateBaseline(double[] inputArray);
    }

    /// <summary>
    /// Calculator class for moving average method
    /// </summary>
    public class MovingAverageCalculator : BaselineCalculator
    {
        int period = 0;
        public MovingAverageCalculator(int period)
        {
            this.period = period;
        }

        /// <summary>
        /// The moving average function takes an array of inputs and a single integer parameter p (period). Each value in 
        /// the output is calculated by taking an average of a moving window of inputs, with the size of the 
        /// window based on the p parameter”. Negative outputs are treated as zeros.
        /// </summary>
        /// <param name="inputArray"></param>
        /// <returns>Returns a list of numerical values representing the estimated baseline at each point</returns>
        public override double[] CalculateBaseline(double[] inputArray)
        {
            var outputList = new List<double>();

            for (int index = 1; index <= inputArray.Length; index++)
            {
                int lowerIndex = Math.Max(0, index - period);
                double averageAtIndex = Math.Max(0, inputArray[lowerIndex..index].Average());
                outputList.Add(LogicHelpers.Round(averageAtIndex));
            }

            return outputList.ToArray();
        }
    }

    public class SingleExponentialSmoothingCalculator : BaselineCalculator
    {
        int period = 0;
        double smoothingFactor = 0;
        public SingleExponentialSmoothingCalculator(int period, double smoothingFactor)
        {
            this.period = period;
            this.smoothingFactor = smoothingFactor;
        }

        /// <summary>
        /// The single exponential smoothing function takes an array of inputs and two parameters, p (period) and α (smoothingFactor).
        /// The initial output value is the average of the first p input values. Subsequent values are
        /// calculated by adding the current input value and the previous output value, with the current input
        /// value being multiplied by α, and the previous output value being multiplied by 1-α. Negative outputs are treated as zeros.
        /// </summary>
        /// <param name="inputArray"></param>
        /// <returns>Returns a list of numerical values representing the estimated baseline at each point</returns>
        public override double[] CalculateBaseline(double[] inputArray)
        {
            var outputList = new List<double>();

            for (int index = 1; index <= inputArray.Length; index++)
            {
                if (index == 1)
                {
                    double averageAtIndex = Math.Max(0, inputArray[0..period].Average());
                    outputList.Add(LogicHelpers.Round(averageAtIndex));
                }
                else
                {
                    double smoothedValue = Math.Max(0, smoothingFactor * inputArray[index - 1] + (1 - smoothingFactor) * outputList.Last());
                    outputList.Add(LogicHelpers.Round(smoothedValue));
                }
            }

            return outputList.ToArray();
        }
    }

}
