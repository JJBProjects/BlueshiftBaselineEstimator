using BlueshiftBaselineEstimator.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueshiftBaselineEstimator.Interface
{
    public static class BaselineEstimator
    {
        /// <summary>
        /// Baseline Calculator allows the user to provide a string of numerical CSV values and calculate the moving 
        /// baseline values using a specified method 
        /// </summary>
        public static void RunConsole()
        {
            List<double> inputValues = new List<double>();
            string outputValues = "";

            Console.WriteLine("Baseline Estimator");
            Console.WriteLine("------------------");

            Console.WriteLine("Welcome to the Baseline Estimator console application! ");
            Console.WriteLine("This app allows the user to enter a series of numerical data, calculate the estimated baseline, and output as desired.");
            Console.WriteLine("The baseline is a moving average that tries to eliminate large variations in data due to external factors.");
            Console.WriteLine("");

            // Prompt user for input method
            Console.WriteLine("How would you like to input values?");
            var inputDataFormatActions = new SortedDictionary<string, string>
            {
                {"1","Import from file" },
                {"2","Enter in console" }
            };
            InterfaceHelpers.ListOptions(inputDataFormatActions);
            var inputDataMethod = InterfaceHelpers.SelectFromList(inputDataFormatActions);

            switch (inputDataMethod)
            {
                case "1":
                    inputValues = ImportDataFromFile();
                    break;
                case "2":
                    inputValues = ReadValuesFromConsole();
                    break;
            }
            Console.WriteLine("");

            //Prompt user for baseline estimation method
            Console.WriteLine("Please choose a baseline estimation method from the following options:");
            var baselineMethodActions = new SortedDictionary<string, string>
            {
                {"1","Moving Average" },
                {"2","Single Exponential Smoothing" }
            };
            InterfaceHelpers.ListOptions(baselineMethodActions);
            var baselineMethod = InterfaceHelpers.SelectFromList(baselineMethodActions);

            switch (baselineMethod)
            {
                case "1":
                    outputValues = MovingAverageMethod(inputValues);
                    break;
                case "2":
                    outputValues = SingleExponentialSmoothingMethod(inputValues);
                    break;
            }
            Console.WriteLine("");

            // Prompt user for output data method
            Console.WriteLine("How would you like to display results?");
            var outputDataActions = new SortedDictionary<string, string>
            {
                {"1","Export to file" },
                {"2","Display in console" }
            };
            InterfaceHelpers.ListOptions(outputDataActions);
            var outputDataMethod = InterfaceHelpers.SelectFromList(outputDataActions);

            switch (outputDataMethod)
            {
                case "1":
                    ExportDataToFile(outputValues);
                    break;
                case "2":
                    Console.WriteLine("Results:");
                    Console.WriteLine(outputValues);
                    break;
            }
            Console.WriteLine("");

            Console.Write("Press any key to close the app");
            Console.ReadKey();
        }

        /// <summary>
        /// Handles input and validation of CSV data from an external text file
        /// </summary>
        /// <returns>Returns a list of numerical values</returns>
        private static List<double> ImportDataFromFile()
        {
            var readValues = new List<double>();
            bool success = false;

            while (!success)
            {
                Console.WriteLine("Please specify the file path and filename of a text file containing one line of numeric CSV data");
                var filePath = Console.ReadLine();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("VALIDATION: File not found. Please check the filename and try again");
                }
                else
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        List<string> listA = new List<string>();
                        while (!reader.EndOfStream)
                        {
                            var csvString = reader.ReadLine();
                            if (csvString != null)
                            {
                                try
                                {
                                    var stringValues = csvString.Split(',').ToList();
                                    readValues = stringValues.Select(x => double.Parse(x)).ToList();
                                    success = true;
                                    Console.WriteLine("Successfully imported from file");
                                }
                                catch
                                {
                                    Console.WriteLine("VALIDATION: An error occured while trying to parse the CSV data. Please check the file and try again.\n");
                                }
                            }
                        }
                    }
                }
            }

            return readValues;
        }

        /// <summary>
        /// Handles input and validation of CSV data entered in the console
        /// </summary>
        /// <returns>Returns a list of numerical values</returns>
        private static List<double> ReadValuesFromConsole()
        {
            var readValues = new List<double>();
            bool success = false;

            while (!success)
            {
                Console.WriteLine("Please enter a list of numeric values in CSV format and press enter when complete");
                var csvString = Console.ReadLine();
                if (csvString != null)
                {
                    try
                    {
                        var stringValues = csvString.Split(',').ToList();
                        readValues = stringValues.Select(x => double.Parse(x)).ToList();
                        success = true;
                    }
                    catch
                    {
                        Console.WriteLine("VALIDATION: An error occured while trying to parse the entered values. Please check your values and try again.");
                    }
                }
            }

            return readValues;
        }

        /// <summary>
        /// Handles creation of text file containing CSV data
        /// </summary>
        private static void ExportDataToFile(string outputValues)
        {
            var readValues = new List<double>();
            bool success = false;

            while (!success)
            {
                Console.WriteLine("Please specify the file path and filename for a new text file");
                string filePath = Console.ReadLine();

                try
                {
                    using StreamWriter file = new(filePath);
                    file.WriteLineAsync(outputValues);
                    success = true;
                    Console.WriteLine("Succesfully written to file");
                }
                catch
                {
                    Console.WriteLine("VALIDATION: specified filename not valid");
                }
            }
        }

        /// <summary>
        /// Prompts for input of parameters required for the Moving Average method of baseline calculation and outputs results to console
        /// </summary>
        private static string MovingAverageMethod(List<double> inputValues)
        {
            string output = "";
            try
            {
                var period = GetPeriodValue();
                var baseLineCalculator = new MovingAverageCalculator(period);
                var result = baseLineCalculator.CalculateBaseline(inputValues.ToArray());
                output = String.Join(",", result.Select(x => x.ToString("0.##")).ToArray());
                Console.WriteLine("Calculation Successful");
            }
            catch
            {
                Console.WriteLine("Something went wrong");
            }

            return output;
        }

        /// <summary>
        /// Prompts for input of parameters for Single Exponential Smoothing method of baseline calculation and outputs results to console
        /// </summary>
        private static string SingleExponentialSmoothingMethod(List<double> inputValues)
        {
            string output = "";
            try
            {
                var period = GetPeriodValue();
                var smoothing = GetSmoothingValue();
                var baseLineCalculator = new SingleExponentialSmoothingCalculator(period, smoothing);
                var result = baseLineCalculator.CalculateBaseline(inputValues.ToArray());
                output = String.Join(",", result.Select(x => x.ToString("0.##")).ToArray());
                Console.WriteLine("Calculation Successful");
            }
            catch
            {
                Console.WriteLine("Something went wrong");
            }

            return output;
        }

        /// <summary>
        /// Handles input and validation of period parameter
        /// </summary>
        /// <returns>Returns the period value</returns>
        private static int GetPeriodValue()
        {
            int period = 0;
            bool success = false;

            while (!success)
            {
                Console.Write("Please specify the number of periods - Integer (0-10): ");
                try
                {
                    period = int.Parse(Console.ReadLine());
                    if (period >= 0 && period <= 10)
                    {
                        success = true;
                    }
                    else
                    {
                        Console.WriteLine("VALIDATION: Please enter a value between 0 and 10");
                    }
                }
                catch
                {
                    Console.WriteLine("VALIDATION: Please enter an integer value");
                }
            }
            return period;
        }

        /// <summary>
        /// Handles input and validation of smoothing parameter
        /// </summary>
        /// <returns>Returns the smoothing parameter value</returns>
        private static double GetSmoothingValue()
        {
            double smoothing = 0;
            bool success = false;

            while (!success)
            {
                Console.Write("Please specify the smoothing factor - Numeric (0-1): ");
                try
                {
                    smoothing = double.Parse(Console.ReadLine());
                    if (smoothing >= 0 && smoothing <= 1)
                    {
                        success = true;
                    }
                    else
                    {
                        Console.WriteLine("VALIDATION: Please enter a value between 0 and 1");
                    }
                }
                catch
                {
                    Console.WriteLine("VALIDATION: Please enter an numeric value");
                }
            }
            return smoothing;
        }
    }
}
