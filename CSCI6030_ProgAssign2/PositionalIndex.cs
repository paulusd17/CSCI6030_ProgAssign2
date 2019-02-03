using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCI6030_ProgAssign2
{
    /// <summary>
    /// This class holds the terms and frequencies for a corpus
    /// </summary>
    public class PositionalIndex
    {
        /// <summary>
        /// This is the dictionary that holds the term and the positional frequency
        /// </summary>
        Dictionary<string, List<int>> Terms { get; set; }

        /// <summary>
        /// This is the document to process
        /// </summary>
        private FileInfo Document { get; set; }

        /// <summary>
        /// An instance of the Porters Algorithum class <see cref="PortersStemmer"/>
        /// </summary>
        private PortersStemmer Stemmer { get; set; }

        /// <summary>
        /// An pointer to the main Index
        /// </summary>
        private Index MainIndex { get; set; }

        /// <summary>
        /// constructor that sets that indexes the file and writes it to disk
        /// </summary>
        public PositionalIndex(FileInfo Info, Index mainIndex)
        {
            MainIndex = mainIndex;
            Document = Info;
            Terms = new Dictionary<string, List<int>>();
            Stemmer = new PortersStemmer();
            IndexFile();
        }

        /// <summary>
        /// This will check if the key already exist in the collection. If it does not, then it will add it, else it will increment the frequency by 1
        /// </summary>
        /// <param name="term">The term to add or increment in the dictionary</param>
        /// <param name="position">The location of the word in the document</param>
        private void Add(string term, int position)
        {
            if (!Terms.ContainsKey(term))
            {
                Terms.Add(term, new List<int>() { position });
            }
            else
            {
                Terms[term].Add(position);
            }
            MainIndex.Add(term, Document.Name);
        }

        /// <summary>
        /// This will write the dictionary to a 'reports' folder of the root directory (location of the exe)
        /// </summary>
        /// <param name="OutputName">The name of the file being written too</param>
        private void ToCSV()
        {
            if (!Directory.Exists(Program.global.PositionalIndexPath))
            {
                Directory.CreateDirectory(Program.global.PositionalIndexPath);
            }

            string document = Document.Name;

            if (document.Contains("."))
            {
                document = document.Substring(0, document.IndexOf('.'));
            }

            using (StreamWriter sw = new StreamWriter(Program.global.PositionalIndexPath + String.Format(Program.global.PositionalIndexFile, document)))
            {
                foreach (KeyValuePair<string, List<int>> keyValue in Terms)
                {
                    sw.Write(keyValue.Key + ",");
                    keyValue.Value.Sort();
                    foreach (int i in keyValue.Value)
                    {
                        sw.Write(i + ",");
                    }
                    sw.WriteLine();
                }
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// This will take in a text file, read the file by line, parse the terms, stem the terms, and set the terms into the correct dictionary
        /// </summary>
        private void IndexFile()
        {
            List<string> tempPath = PreProcessDocument();

            for (int row = 0; row < tempPath.Count; row++)
            {
                string[] terms = tempPath[row].Split(' ');

                for (int term_index = 0; term_index < terms.Length; term_index++)
                {
                    if (terms[term_index].Length > 0)
                        Add(terms[term_index], (row + (term_index + 1)));
                }
            }

            ToCSV();
        }

        /// <summary>
        /// This will conduct preprocessing on the document so that its position can be extracted after tokenization
        /// </summary>
        /// <returns>A list of the tokenized strings</returns>
        private List<string> PreProcessDocument()
        {
            List<string> tempDoc = new List<string>();
            StreamReader sr = new StreamReader(Document.FullName);
            while (!sr.EndOfStream)
            {
                tempDoc.Add(Token.Token_String(sr.ReadLine()));
            }
            return tempDoc;
        }

    }
}
