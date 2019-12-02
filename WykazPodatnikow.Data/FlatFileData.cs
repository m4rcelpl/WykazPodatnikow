using System.Collections.Generic;

namespace WykazPodatnikow.Data
{
    public class FlatFileData
    {
        public Naglowek naglowek { get; set; }
        public string liczbatransformacji { get; set; }
        public List<string> skrotypodatnikowczynnych { get; set; }
        public List<string> skrotypodatnikowzwolnionych { get; set; }
        public List<string> maski { get; set; }
    }

    public class Naglowek
    {
        public string datagenerowaniadanych { get; set; }
        public string liczbatransformacji { get; set; }
        public string schemat { get; set; }
    }
}