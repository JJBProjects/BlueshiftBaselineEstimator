using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueshiftBaselineEstimator.Interface
{
    internal class InterfaceHelpers
    {
        /// <summary>
        /// Displays all actions in the action list
        /// </summary>
        /// <param name="actions">A dictionary containing numbered menu actions and their description</param>
        public static void ListOptions(SortedDictionary<string, string> actions)
        {
            foreach (var action in actions)
            {
                Console.WriteLine($"\t{action.Key} - {action.Value}");
            }
        }

        /// <summary>
        /// Returns a selected action if it exists in the action list
        /// </summary>
        /// <param name="actions">A dictionary containing numbered menu actions and their description</param>
        /// <returns>Returns the key of the action selected</returns>
        public static string SelectFromList(SortedDictionary<string, string> actions)
        {
            string selectedAction = "";

            while (!actions.ContainsKey(selectedAction))
            {
                Console.Write("Your option? ");
                selectedAction = Console.ReadLine();
            }
            Console.WriteLine("You have selected: " + actions[selectedAction]);
            Console.WriteLine("");

            return selectedAction;
        }
    }
}
