using System;
using System.Collections.Generic;
using System.IO;

namespace CSCI6030_ProgAssign2
{
    /// <summary>
    /// This class will conduct the indexing of a text file
    /// </summary>
    public class Index
    {
        /// <summary>
        /// This is the dictionary that holds the term and the positional frequency
        /// </summary>
        Dictionary<string, List<string>> Terms { get; set; }

        /// <summary>
        /// constructor that sets that indexes the file and writes it to disk
        /// </summary>
        public Index()
        {
            Terms = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// This will check if the key already exist in the collection. If it does not, then it will add it, else it will increment the frequency by 1
        /// </summary>
        /// <param name="term">The term to add or increment in the dictionary</param>
        /// <param name="position">The location of the word in the document</param>
        public void Add(string term, string document)
        {
            if (document.Contains("."))
            {
                document = document.Substring(0,document.IndexOf('.'));
            }
            if (!Terms.ContainsKey(term))
            {
                Terms.Add(term, new List<string>() { document });
            }
            else
            {
                if (!Terms[term].Contains(document))
                    Terms[term].Add(document);
            }
        }

        /// <summary>
        /// This will write the dictionary to a 'reports' folder of the root directory (location of the exe)
        /// </summary>
        /// <param name="OutputName">The name of the file being written too</param>
        public void ToCSV()
        {
            using (StreamWriter sw = new StreamWriter(Program.global.IndexPath + Program.global.MainIndexFile))
            {
                foreach (KeyValuePair<string, List<string>> keyValue in Terms)
                {
                    sw.Write(keyValue.Key + ",");

                    keyValue.Value.Sort();
                    foreach (string s in keyValue.Value)
                    {
                        sw.Write(s + ",");
                    }
                    sw.WriteLine();
                }
                sw.Flush();
                sw.Close();
            }
        }
    }
}
