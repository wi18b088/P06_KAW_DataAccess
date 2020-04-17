using System;
using System.Collections.Generic;
using System.Text;

namespace P06_KAW_DataAccess.App2
{
    public class Stat
    {
        public int Input { get; set; }
        public string Result { get; set; }
        public DateTime FirstTest { get; set; }
        public DateTime LastTest { get; set; }
        public int BestTime { get; set; }
        public int TestCount { get; set; }
    }
}
