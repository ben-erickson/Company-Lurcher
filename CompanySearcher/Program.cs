using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.CustomSearchAPI.v1;
using Google.Apis;
using System.Net.Http;
using System.Web;
using System.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CompanySearcher
{
    internal static class Program
    {
        internal static string apiKey = string.Empty;
        internal static string searchEngineKey = string.Empty;
        internal static List<List<string>> keywordSearches = new List<List<string>>();
        internal static string dataFileDirectory = string.Empty;
        internal static List<(string, List<string?>)> resultLinks = new List<(string, List<string?>)>();

        public static void Main(string[] args)
        {
            // Arguments: 1) either the full directory to the data file, or the name of the file in the AppData folder
            // 2) directory for the output file

            // Hard-Coding the argument for testing
            args = new string[] { @"D:\Project Repositories\KeywordSearcher\CompanySearcher\CompanySearcher\data\SampleSearch.txt", @"D:\Project Repositories\KeywordSearcher\CompanySearcher\CompanySearcher\data\outputlinks.txt" };

            bool fileFound = false;
            string fileErrorMessage = string.Empty;

            (fileFound, fileErrorMessage) = FindFile(args[0]);

            if (!fileFound)
            {
                Console.WriteLine(fileErrorMessage);
            }
            else
            {
                bool dataObtained = false;
                string dataErrorMessage = string.Empty;

                (dataObtained, dataErrorMessage) = OpenFile();

                if (!dataObtained)
                {
                    Console.WriteLine(dataErrorMessage);
                }
                else
                {
                    bool resultsObtained = false;
                    string searchErrorMessage = string.Empty;
                    
                    (resultsObtained, searchErrorMessage) = ExecuteSearches();

                    if (resultsObtained)
                    {
                        using (var file = File.CreateText(args[1]))
                        {
                            for (int i = 0; i < resultLinks.Count; i++)
                            {
                                file.WriteLine(resultLinks[i].Item1 + ", " + String.Join(',', resultLinks[i].Item2));
                            }
                        }
                    }
                }
            }
        }

        public static (bool, string) ExecuteSearches()
        {
            bool success = false;
            string errorMessage = string.Empty;

            foreach (List<string> keywordSearch in keywordSearches)
            {
                // Turn the current search into an actual search string
                string query = string.Empty;

                foreach (string keyword in keywordSearch)
                {
                    query += keyword;
                }

                string searchURL = $"https://customsearch.googleapis.com/customsearch/v1?cx={searchEngineKey}&key={apiKey}&q={query}&start=1";

                using (var client = new HttpClient())
                {
                    using (var result = client.GetStringAsync(searchURL))
                    {
                        JObject resultJson = JObject.Parse(result.Result);

                        List<string?> jsonLinks = resultJson["items"].Select(x => (string?)x.SelectToken("link")).ToList();

                        resultLinks.Add((keywordSearch[0], jsonLinks));
                    }
                }
            }

            return (success, errorMessage);
        }

        public static (bool, string) OpenFile()
        {
            bool success = false;
            string errorMessage = string.Empty;

            string[] fileContents = File.ReadAllLines(dataFileDirectory);

            if (fileContents.Length < 3)
            {
                success = false;
                errorMessage = "Data file was not in expected format.";
            }
            else
            {
                success = true;

                apiKey = fileContents[0];
                searchEngineKey = fileContents[1];

                for (int i = 2; i < fileContents.Length; i++)
                {
                    keywordSearches.Add(fileContents[i].Split(',').ToList<string>());
                }
            }

            return (success, errorMessage);
        }

        public static (bool, string) FindFile(string filepath)
        {
            bool success = false;
            string errorMessage = string.Empty;

            // Check if the argument provides a full directory
            if (File.Exists(filepath))
            {
                success = true;
                dataFileDirectory = filepath;
            }
            else
            {
                // Check if the appdata folder exists
                if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CompanySearcher")))
                {
                    success = false;
                    errorMessage = "Application folder not found. Creating folder.";

                    // If the folder doesn't exist, create it
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CompanySearcher"));
                }
                else if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), filepath)))
                {
                    success = true;
                    dataFileDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), filepath);
                }
                else
                {
                    success = false;
                    errorMessage = "Data file could not be found. Please enter the directory of the data file as the argument for the program.";
                }
            }

            return (success, errorMessage);
        }
    }
}
