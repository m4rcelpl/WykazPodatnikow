using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace WykazPodatnikow.Standard
{
    internal class NamingPolicy : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

            foreach (JsonProperty prop in list)
            {
                if (prop.UnderlyingName == "skrotypodatnikowczynnych")
                    prop.PropertyName = "skroty podatnikow czynnych";
                else if (prop.UnderlyingName == "skrotypodatnikowzwolnionych")
                    prop.PropertyName = "skroty podatnikow zwolnionych";
            }

            return list;
        }
    }
}