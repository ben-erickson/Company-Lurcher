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
            // Arguments:
            // 1) either the full directory to the data file, or the name of the file in the AppData folder
            // 2) directory for the output file

            // The processing is placed in a try block so that when an exception is thrown, it will be shown to the user and processing will be stopped
            try
            {
                // Check for correct number of arguments
                if (args.Length == 0)
                {
                    throw new Exception("No arguments were given. Please provide the directory of the input file, and the desired output directory.");
                }
                else if (args.Length != 2)
                {
                    throw new Exception("Please provide two arguments: the directory of the input file, and the desired output directory.");
                }

                // Check if output directory exists
                if (!Directory.Exists(args[1]))
                {
                    throw new Exception("Output directory could not be found. Please enter a valid directory for the output file.");
                }

                bool fileFound = false;
                string fileErrorMessage = string.Empty;

                Console.WriteLine("Searching for file.");
                (fileFound, fileErrorMessage) = FindFile(args[0]);

                if (!fileFound)
                {
                    throw new Exception(fileErrorMessage);
                }

                // File has been found, get the data out of the file
                Console.WriteLine("File has been found. Extracting data.");

                bool dataObtained = false;
                string dataErrorMessage = string.Empty;

                (dataObtained, dataErrorMessage) = OpenFile();

                if (!dataObtained)
                {
                    throw new Exception(dataErrorMessage);
                }

                // Data has been extracted, execute the searches defined by the file
                Console.WriteLine("Data has been extracted. Performing searches.");

                bool resultsObtained = false;
                string searchErrorMessage = string.Empty;

                (resultsObtained, searchErrorMessage) = ExecuteSearches();

                if (!resultsObtained)
                {
                    throw new Exception(searchErrorMessage);
                }

                // Searches have been executed successfully, write the results to a file
                Console.WriteLine("Searches have been performed. Writing the results to a file.");

                // Add filename to directory given in argument
                string outputFileName = args[1] + "\\LurcherSearch-" + DateTime.Today.ToString("mm-dd-yyyy") + ".csv";

                using (var file = File.CreateText(outputFileName))
                {
                    for (int i = 0; i < resultLinks.Count; i++)
                    {
                        file.WriteLine(resultLinks[i].Item1 + ", " + String.Join(',', resultLinks[i].Item2));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static (bool, string) ExecuteSearches()
        {
            bool success = true;
            string errorMessage = string.Empty;

            for (int i = 0; i < keywordSearches.Count; i++)
            {
                // Turn the current search into an actual search string
                string query = string.Empty;

                foreach (string keyword in keywordSearches[i])
                {
                    query += keyword;
                }

                string searchURL = $"https://customsearch.googleapis.com/customsearch/v1?cx={searchEngineKey}&key={apiKey}&q={query}&start=1";

                using (var client = new HttpClient())
                {
                    try
                    {
                        using (var result = client.GetStringAsync(searchURL))
                        {
                            JObject resultJson = JObject.Parse(result.Result);

                            List<string?> jsonLinks = resultJson["items"].Select(x => (string?)x.SelectToken("link")).ToList();

                            resultLinks.Add((keywordSearches[i][0], jsonLinks));
                        }
                    }
                    catch
                    {
                        errorMessage = $"An error occured during the execution of the searches. Row number of error: {i + 1}.\nIf the error occured during the first search, the issue may be with the API key or the Search Engine key.";
                        success = false;

                        // If we hit an error during processing, exit the for loop
                        break;
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
                    errorMessage = "Data file could not be found. Please enter the directory of the data file as the first argument for the program.";
                }
            }

            return (success, errorMessage);
        }
    }
}
