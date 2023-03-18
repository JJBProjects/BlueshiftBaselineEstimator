using BlueshiftBaselineEstimator;
using BlueshiftBaselineEstimator.Logic;

namespace UnitTests
{
    public class CalculatorsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CalculateMovingAverageShouldReturnCorrectValues()
        {
            var inputValues = new double[] { 50, 60, 40, 30, 90, -5, 40, 50, 30, 40 };
            var expectedOutput = new double[] { 50.00, 55.00, 50.00, 43.33, 53.33, 38.33, 41.67, 28.33, 40.00, 40.00 };

            var baselineCalculator = new MovingAverageCalculator(3);
            var outputValues = baselineCalculator.CalculateBaseline(inputValues);

            Assert.IsTrue(Enumerable.SequenceEqual(expectedOutput, outputValues));
        }

        [Test]
        public void CalculateMovingAverageShouldReturnCorrectValuesWhenAverageIsLessThanZero()
        {
            var inputValues = new double[] { -10,-20,-30,-40,-50 };
            var expectedOutput = new double[] { 0, 0, 0, 0, 0 };

            var baselineCalculator = new MovingAverageCalculator(3);
            var outputValues = baselineCalculator.CalculateBaseline(inputValues);

            Assert.IsTrue(Enumerable.SequenceEqual(expectedOutput, outputValues));
        }

        [Test]
        public void CalculateSingleExponentialSmoothingShouldReturnCorrectValues1()
        {
            var inputValues = new double[] { 50, 60, 40, 30, 90, -5, 40, 50, 30, 40};
            var expectedOutput = new double[] { 50.00, 54.00, 48.40, 41.04, 60.62, 34.37, 36.62, 41.97, 37.18, 38.31 };

            var baselineCalculator = new SingleExponentialSmoothingCalculator(3, 0.4);
            var outputValues = baselineCalculator.CalculateBaseline(inputValues);

            Assert.IsTrue(Enumerable.SequenceEqual(expectedOutput, outputValues));
        }

        [Test]
        public void CalculateSingleExponentialSmoothingShouldReturnCorrectValues2()
        {
            var inputValues = new double[] { 15, 20, 16, 90, -90, -5, 17, 19, 16, 18 };
            var expectedOutput = new double[] { 35.25, 27.63, 21.81, 55.91, 0, 0, 8.50, 13.75, 14.88, 16.44 };

            var baselineCalculator = new SingleExponentialSmoothingCalculator(4, 0.5);
            var outputValues = baselineCalculator.CalculateBaseline(inputValues);

            Assert.IsTrue(Enumerable.SequenceEqual(expectedOutput, outputValues));
        }

        [Test]
        public void CalculateSingleExponentialSmoothingShouldReturnCorrectValuesWhenAverageIsLessThanZero()
        {
            var inputValues = new double[] { -10, -20, -30, -40, -50 };
            var expectedOutput = new double[] { 0, 0, 0, 0, 0 };

            var baselineCalculator = new SingleExponentialSmoothingCalculator(4, 0.5);
            var outputValues = baselineCalculator.CalculateBaseline(inputValues);

            Assert.IsTrue(Enumerable.SequenceEqual(expectedOutput, outputValues));
        }
    }
}