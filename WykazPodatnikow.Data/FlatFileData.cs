using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WykazPodatnikow.Data
{

    public class FlatFileData
    {
        public Head head { get; set; }
        public List<string> body { get; set; }
        public List<string> masks { get; set; }
    }

    public class Head
    {
        public string datagenerowaniadanych { get; set; }
    }

}
