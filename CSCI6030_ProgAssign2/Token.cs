using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCI6030_ProgAssign2
{
    public static class Token
    {
        /// <summary>
        /// Take a string of terms and convert to lower, remove punctuation, and return the char[]
        /// </summary>
        /// <param name="term">the string that contains the terms</param>
        /// <returns>a tokenized char[]</returns>
        public static char[] Token_Char(string term)
        {
            List<char> returnList = new List<char>();
            term = term.Trim().ToLowerInvariant();
            foreach (char c in term)
            {
                if ((int)c >= 97 && (int)c <= 122)
                {
                    returnList.Add(c);
                }
            }
            return returnList.ToArray();
        }

        /// <summary>
        /// Takes a string of terms and converts to lower, removes punctuation, and returns the reconstructed string of terms
        /// </summary>
        /// <param name="term">the string that contains the terms</param>
        /// <returns>a new string of tokenized terms</returns>
        public static string Token_String(string terms)
        {
            terms = terms.Replace("--", " ");
            string[] sentence = terms.Split(' ');

            string returnItem = "";

            foreach (string term in sentence)
            {
                if (term.Length > 0)
                {
                    char[] tokenizedTerm = Token_Char(term);
                    if (tokenizedTerm.Length > 0)
                    {
                        string temp = "";
                        foreach (char c in tokenizedTerm)
                        {
                            temp += c;
                        }
                        returnItem += temp + " ";
                    }
                }
            }

            return returnItem;
        }
    }
}
