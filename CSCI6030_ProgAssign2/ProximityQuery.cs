using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCI6030_ProgAssign2
{
    public class ProximityQuery
    {
        private Dictionary<string, List<string>> index { get; set; }

        public ProximityQuery()
        {
            //load the document index into memory
            index = new Dictionary<string, List<string>>();
            StreamReader sr = new StreamReader(Program.global.IndexPath + Program.global.MainIndexFile);
            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(',');
                List<string> documents = new List<string>();
                for (int i = 1; i < line.Length; i++)
                {
                    documents.Add(line[i]);
                }
                index.Add(line[0], documents);
            }
        }

        public List<string> Intersect(string term1, string term2, int seperation)
        {
            List<string> documents = new List<string>();

            term1 = Token.Token_String(term1).Replace(" ", "");
            term2 = Token.Token_String(term2).Replace(" ", "");

            //if the terms are not in the index then there is no match
            if (!(index.ContainsKey(term1) && index.ContainsKey(term2)))
            {
                return documents;
            }

            List<string> docTerm1 = null;
            List<string> docTerm2 = null;

            if (index[term1].Count < index[term2].Count)
            {
                docTerm1 = index[term1];
                docTerm2 = index[term2];
            }
            else
            {
                docTerm1 = index[term2];
                docTerm2 = index[term1];
            }
            bool documentAdded = false;

            for (int i = 0; i < docTerm1.Count; i++)
            {
                documentAdded = false;
                if (docTerm1[i].Length > 0)
                {
                    for (int j = 0; j < docTerm2.Count; j++)
                    {
                        if (docTerm2[j].Length > 0)
                        {
                            if (docTerm1[i] == docTerm2[j])
                            {
                                Dictionary<string, List<int>> postionalIndex = LoadPositionalIndex(docTerm1[i]);

                                List<int> pp1 = null;
                                List<int> pp2 = null;

                                if (postionalIndex[term1].Count < postionalIndex[term2].Count)
                                {
                                    pp1 = postionalIndex[term1];
                                    pp2 = postionalIndex[term2];
                                }
                                else
                                {
                                    pp1 = postionalIndex[term2];
                                    pp2 = postionalIndex[term1];
                                }

                                for (int i1 = 0; i1 < pp1.Count; i1++)
                                {
                                    for (int j1 = 0; j1 < pp2.Count; j1++)
                                    {
                                        if (Math.Abs(pp1[i1] - pp2[j1]) <= seperation)
                                        {
                                            documents.Add("Document: " + docTerm2[j] + " { /" +term1 + " Loc: " + pp1[i1] + " /" + term2 + " Loc: " + pp2[j1] + "}");
                                            documentAdded = true;
                                        }
                                        else if (pp2[j1] > pp2[i1])
                                        {
                                            break;
                                        }
                                        if (documentAdded)
                                            break;
                                    }
                                    if (documentAdded)
                                        break;
                                }
                            }
                        }

                        if (documentAdded)
                            break;
                    }
                }
            }
            return documents.Distinct().ToList();
        }

        private Dictionary<string, List<int>> LoadPositionalIndex(string DocumentID)
        {
            //load the document index into memory
            Dictionary<string, List<int>> positionalIndex = new Dictionary<string, List<int>>();
            StreamReader sr = new StreamReader(Program.global.PositionalIndexPath + String.Format(Program.global.PositionalIndexFile, DocumentID));
            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(',');
                List<int> positions = new List<int>();
                for (int i = 1; i < line.Length; i++)
                {
                    if (line[i].Length > 0)
                        positions.Add(Convert.ToInt32(line[i]));
                }

                positionalIndex.Add(line[0], positions);
            }

            return positionalIndex;
        }
    }
}
