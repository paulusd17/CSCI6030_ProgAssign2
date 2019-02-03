using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class IndexTest
    {
        CSCI6030_ProgAssign2.Global global = new CSCI6030_ProgAssign2.Global();

        [TestInitialize]
        public void CreateTestIndex()
        {
            if (Directory.Exists(global.IndexPath))
            {
                if (!Directory.Exists(global.PositionalIndexPath))
                {
                    string testPath = Directory.GetCurrentDirectory() + "/TestData/corpus";
                    CSCI6030_ProgAssign2.Program.Main(new string[] { testPath });
                }
            }
            else
            {
                string testPath = Directory.GetCurrentDirectory() + "/TestData/corpus";
                CSCI6030_ProgAssign2.Program.Main(new string[] { testPath });
            }


        }
        /// <summary>
        /// This will run through the whole program and utilizing the supplied test data
        /// </summary>
        [TestMethod]
        public void TestIndexNoError()
        {
            Assert.IsTrue(Directory.Exists(global.IndexPath), "Index dir is null");
            Assert.IsTrue(Directory.Exists(global.PositionalIndexPath), "Positional Index dir is null");
            Assert.IsTrue(((string[])Directory.GetFiles(global.PositionalIndexPath)).Length > 0, "Positional Index files null");
            Assert.IsTrue(((string[])Directory.GetFiles(global.IndexPath)).Length > 0, "Index files null");
        }

        private void DeleteFiles(string path)
        {
            string[] files = new string[0];
            files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }
            Directory.Delete(path);
        }

        [TestMethod]
        public void TestProximityQuery1Match()
        {
            CSCI6030_ProgAssign2.ProximityQuery query = new CSCI6030_ProgAssign2.ProximityQuery();
            List<string> result = query.Intersect("this", "test", 4);
            Assert.IsTrue(result.Count == 1, "not corrent number of documents");
            Assert.IsTrue(result[0].Contains("Doc1"), "Wrong Document Returned");
        }

        [TestMethod]
        public void TestProximityQuery2Match()
        {
            CSCI6030_ProgAssign2.ProximityQuery query = new CSCI6030_ProgAssign2.ProximityQuery();
            List<string> result = query.Intersect("I", "am", 4);
            Assert.IsTrue(result.Count == 2, "not corrent number of documents");
            Assert.IsTrue(result[0].Contains("Doc2"), "Wrong Document Returned");
            Assert.IsTrue(result[1].Contains("Doc3"), "Wrong Document Returned");
        }

        [TestMethod]
        public void TestProximityQuery3Match()
        {
            CSCI6030_ProgAssign2.ProximityQuery query = new CSCI6030_ProgAssign2.ProximityQuery();
            List<string> result = query.Intersect("this", "IS!", 4);
            Assert.IsTrue(result.Count == 3, "not corrent number of documents");
            Assert.IsTrue(result[0].Contains("Doc1"), "Wrong Document Returned");
            Assert.IsTrue(result[1].Contains("Doc2"), "Wrong Document Returned");
            Assert.IsTrue(result[2].Contains("Doc3"), "Wrong Document Returned");
        }

        [TestMethod]
        public void TestProximityQueryNoMatchKey()
        {
            CSCI6030_ProgAssign2.ProximityQuery query = new CSCI6030_ProgAssign2.ProximityQuery();
            List<string> result = query.Intersect("this", "NotAWord", 2);
            Assert.IsTrue(result.Count == 0, "To many documents");
        }

        [TestMethod]
        public void TestProximityQueryNoMatchSeperation()
        {
            CSCI6030_ProgAssign2.ProximityQuery query = new CSCI6030_ProgAssign2.ProximityQuery();
            List<string> result = query.Intersect("document", "additional", 2);
            Assert.IsTrue(result.Count == 0, "To many documents");
        }

        ///// <summary>
        ///// This will op the char report and ensure that the correct char count and items exist
        ///// </summary>
        //[TestMethod]
        //public void TestIndexCharReport()
        //{
        //    string testPath = Directory.GetCurrentDirectory() + "/Index/Index.csv";
        //    if (File.Exists(testPath))
        //    {
        //        StreamReader sr = new StreamReader(testPath);
        //        int rows = 0;
        //        bool value = false;
        //        while (!sr.EndOfStream)
        //        {
        //            string line = sr.ReadLine();
        //            rows++;
        //            if (line.Contains("d"))
        //            {
        //                if (line.Substring(line.IndexOf(',')).Contains("1"))
        //                {
        //                    value = true;
        //                }
        //            }
        //        }
        //        Assert.IsTrue(rows == 15, "Invalid amount of characters");
        //        Assert.IsTrue(value, "the char d was not found or it had a value greater than 1");
        //    }
        //    else
        //    {
        //        Assert.Fail("Char.csv does not exist");
        //    }
        //}

        ///// <summary>
        ///// This will op the uni-gram report and ensure that the correct char count and items exist
        ///// </summary>
        //[TestMethod]
        //public void TestIndexUniReport()
        //{
        //    string testPath = Directory.GetCurrentDirectory() + "/Reports/Uni.csv";
        //    if (File.Exists(testPath))
        //    {
        //        StreamReader sr = new StreamReader(testPath);
        //        int rows = 0;
        //        bool value = false;
        //        while (!sr.EndOfStream)
        //        {
        //            string line = sr.ReadLine();
        //            rows++;
        //            if (line.Contains("line"))
        //            {
        //                if (line.Substring(line.IndexOf(',')).Contains("2"))
        //                {
        //                    value = true;
        //                }
        //            }
        //        }
        //        Assert.IsTrue(rows == 9, "Invalid amount of uni-grams");
        //        Assert.IsTrue(value, "the uni-gram 'line' was not found or it had a value != 2");
        //    }
        //    else
        //    {
        //        Assert.Fail("Char.csv does not exist");
        //    }
        //}

        ///// <summary>
        ///// This will op the bi-gram report and ensure that the correct char count and items exist
        ///// </summary>
        //[TestMethod]
        //public void TestIndexBiReport()
        //{
        //    string testPath = Directory.GetCurrentDirectory() + "/Reports/Bi.csv";
        //    if (File.Exists(testPath))
        //    {
        //        StreamReader sr = new StreamReader(testPath);
        //        int rows = 0;
        //        bool value = false;
        //        while (!sr.EndOfStream)
        //        {
        //            string line = sr.ReadLine();
        //            rows++;
        //            if (line.Contains("is a"))
        //            {
        //                if (line.Substring(line.IndexOf(',')).Contains("2"))
        //                {
        //                    value = true;
        //                }
        //            }
        //        }
        //        Assert.IsTrue(rows == 8, "Invalid amount of bi-grams");
        //        Assert.IsTrue(value, "the bi gram 'is a' was not found or it had a value != 2");
        //    }
        //    else
        //    {
        //        Assert.Fail("Char.csv does not exist");
        //    }
        //}

        ///// <summary>
        ///// This will op the tri-gram report and ensure that the correct char count and items exist
        ///// </summary>
        //[TestMethod]
        //public void TestIndexTriReport()
        //{
        //    string testPath = Directory.GetCurrentDirectory() + "/Reports/Tri.csv";
        //    if (File.Exists(testPath))
        //    {
        //        StreamReader sr = new StreamReader(testPath);
        //        int rows = 0;
        //        bool value = false;
        //        while (!sr.EndOfStream)
        //        {
        //            string line = sr.ReadLine();
        //            rows++;
        //            if (line.Contains("line with punctuat"))
        //            {
        //                if (line.Substring(line.IndexOf(',')).Contains("1"))
        //                {
        //                    value = true;
        //                }
        //            }
        //        }
        //        Assert.IsTrue(rows == 7, "Invalid amount of tri-grams");
        //        Assert.IsTrue(value, "the string 'line with punctuat' was not found or it had a value != 1");
        //    }
        //    else
        //    {
        //        Assert.Fail("Char.csv does not exist");
        //    }
        //}
    }
}
