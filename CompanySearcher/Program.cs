using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanySearcher
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Arguments: either the full directory to the data file, or the name of the file in the AppData folder

            // Hard-Coding the argument for testing
            args = new string[] { @"D:\Project Repositories\KeywordSearcher\CompanySearcher\CompanySearcher\data\SampleSearch.txt" };

            bool fileFound = false;
            string dataDirectory = string.Empty;

            // Check if the argument provides a full directory
            if (File.Exists(args[0]))
            {
                fileFound = true;
                dataDirectory = args[0];
            }
            else
            {
                // Check if the appdata folder exists
                if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CompanySearcher")))
                {
                    fileFound = false;

                    // If the folder doesn't exist, create it and give the user an error
                    Console.WriteLine("Application folder not found. Creating folder.");

                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CompanySearcher"));
                }
                else if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), args[0])))
                {
                    fileFound = true;
                    dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), args[0]);
                }
            }

            // If we weren't able to find the data file, let the user know and stop processing.
            if (!fileFound)
            {
                Console.WriteLine("Data file was unable to be located. Cancelling processing.");                
            }
            else
            {
                string apiKey = string.Empty;
                string searchEngineKey = string.Empty;
                List<List<string>> keywordSearches = new List<List<string>>();

                // Break the data file into an array
                string[] fileContents = File.ReadAllLines(dataDirectory);

                // If the file has less than three lines, throw an error
                if (fileContents.Length < 3)
                {
                    Console.WriteLine("File does not contain any searches.");
                }
            }
        }
    }
}
