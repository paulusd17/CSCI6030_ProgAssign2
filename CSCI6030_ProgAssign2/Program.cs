using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSCI6030_ProgAssign2
{
    /// <summary>
    /// This is the main script for the program
    /// </summary>
    public class Program
    {
        private static DateTime start = DateTime.Now;
        public static Global global = new Global();

        /// <summary>
        /// This is the start method for the program. It wil read in the corpus
        /// and pass it to the indexer one at a time
        /// </summary>
        /// <param name="args">Not used</param>
        public static void Main(string[] args)
        {

            Console.WriteLine("Program Started");

            try
            {
                if (!Directory.Exists(global.IndexPath))
                {
                    BuildIndex(args);
                }
                else if (!File.Exists(global.IndexPath + global.MainIndexFile))
                {
                    BuildIndex(args);
                }

                if (args is null || args.Length == 0)
                {
                    Console.WriteLine("Do you want to rebuild the index? Y or N");
                    string answer = Console.ReadLine();
                    if (answer.ToUpper() == "Y")
                    {
                        BuildIndex(args);
                    }
                    ProximityQuery proximityQuery = new ProximityQuery();

                    string query = "";
                    while (query.ToUpper() != "N")
                    {
                        string term1 = null, term2 = null, sep = null;
                        int seperation = 0;

                        Console.Write("Enter the first term - ");
                        term1 = Console.ReadLine();

                        Console.Write("Enter the second term - ");
                        term2 = Console.ReadLine();

                        Console.Write("Enter the max seperation between terms - ");
                        sep = Console.ReadLine();

                        try
                        {
                            seperation = Convert.ToInt32(sep);
                            DateTime beginQuery = DateTime.Now;
                            List<string> result = proximityQuery.Intersect(term1, term2, seperation);
                            TimeSpan time = DateTime.Now.Subtract(beginQuery);
                            if (result.Count > 0)
                            {
                                int i = 1;
                                Console.WriteLine(result.Count + " documents found in ~" + time.Milliseconds + " ms");
                                foreach (string s in result)
                                {
                                    Console.WriteLine(i + ") " + s);
                                    i++;
                                }                  
                            }
                            else
                            {
                                Console.WriteLine("No results found");
                            }
                        }
                        catch
                        {
                            Console.WriteLine("ERROR! Max seperation must be a number");
                        }

                        Console.Write("Do you want to run anothe query? y or n - ");
                        query = Console.ReadLine();

                    }

                }

                Console.WriteLine("Run Time:" + DateTime.Now.Subtract(start).ToString());
                Console.WriteLine("Program Complete. This window will close in 5 seconds.");
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                SetError(start, "Unknow error occured. " + ex.Message);
            }
        }

        public static void BuildIndex(string[] args)
        {
            DirectoryInfo CorpusPath = null;
            if (args is null || args.Length == 0)
            {
                Console.Write("Do you want to use the default path location for the corpus (current location where running exe is located)? y or n - ");
                string answer = Console.ReadLine();
                if (answer.ToUpper() == "Y")
                {
                    CorpusPath = new DirectoryInfo(global.CorpusPath);
                }
                else
                {
                    GetPath(CorpusPath);
                }
            }
            else
            {
                CorpusPath = new DirectoryInfo(args[0]);
            }

            if (!Directory.Exists(global.IndexPath))
                Directory.CreateDirectory(global.IndexPath);

            Console.WriteLine("Building Index");

            Index mainIndex = new Index();

            foreach (FileInfo path in CorpusPath.EnumerateFiles())
            {
                Console.WriteLine("Processing Index file :" + path.Name);
                Console.WriteLine("Run Time:" + DateTime.Now.Subtract(start).ToString());
                try
                {
                    PositionalIndex index = new PositionalIndex(path, mainIndex);
                    Console.WriteLine("Index of " + path.Name + " Complete");
                    Console.WriteLine("Run Time:" + DateTime.Now.Subtract(start).ToString());
                }
                catch (Exception ex)
                {
                    SetError(start, "Error processing " + path.FullName + ". " + ex.Message);
                }
            }

            mainIndex.ToCSV();
        }

        /// <summary>
        /// This will set the exception message and exit the program
        /// </summary>
        /// <param name="start">The datatime that the program began</param>
        /// <param name="msg">The error message to display</param>
        public static void SetError(DateTime start, string msg)
        {
            Console.WriteLine("Run Time:" + DateTime.Now.Subtract(start).ToString());
            Console.WriteLine(msg);
            Console.WriteLine("Program Error. This window will close in 5 seconds.");
            Thread.Sleep(5000);
            Environment.Exit(1);
        }

        //This will handle getting the path to the corpus if the user does not wish to use the default path
        private static void GetPath(DirectoryInfo CorpusPath)
        {
            while (CorpusPath is null)
            {
                Console.WriteLine("Enter the fully qualified path to the root corpus folder");
                string answer = Console.ReadLine();
                if (Directory.Exists(answer))
                    CorpusPath = new DirectoryInfo(answer);
                else
                {
                    Console.WriteLine("The path entered does not exist.");
                    CorpusPath = null;
                }
            }
        }
    }
}
