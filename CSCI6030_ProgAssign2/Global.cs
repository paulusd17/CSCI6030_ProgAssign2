using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCI6030_ProgAssign2
{
    public class Global
    {
        public readonly string IndexPath = Directory.GetCurrentDirectory() + "/Index";
        public readonly string CorpusPath = Directory.GetCurrentDirectory() + "/Corpus";
        public readonly string PositionalIndexPath = Directory.GetCurrentDirectory() + "/Index/Positional_Index";
        public readonly string MainIndexFile = "/Index.csv";
        public readonly string PositionalIndexFile = "/{0}_PositionalIndex.csv";
    }
}
