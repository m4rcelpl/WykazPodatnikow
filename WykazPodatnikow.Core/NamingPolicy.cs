using System.Text.Json;

namespace WykazPodatnikow.Core
{
    internal class NamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name switch
            {
                "skrotypodatnikowczynnych" => "skroty podatnikow czynnych",
                "skrotypodatnikowzwolnionych" => "skroty podatnikow zwolnionych",
                _ => name
            };
        }
    }
}