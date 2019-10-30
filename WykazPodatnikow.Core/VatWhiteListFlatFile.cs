using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WykazPodatnikow.Data;
using WykazPodatnikow.SharedLib;

namespace WykazPodatnikow.Core
{
    public class VatWhiteListFlatFile
    {
        private JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private readonly FlatFileData flatFileData;

        public VatWhiteListFlatFile(string PathToJson)
        {
            if (!File.Exists(PathToJson))
                throw new System.Exception("File not found");

            try
            {
                flatFileData = JsonSerializer.Deserialize<FlatFileData>(File.ReadAllText(PathToJson), JsonSerializerOptions);
            }
            catch (System.Exception)
            {
                throw;
            }

            if(string.IsNullOrEmpty(flatFileData.head.datagenerowaniadanych))
                throw new System.Exception("Invalide file, datagenerowaniadanych is empty");
        }
        
        public bool IsInFlatFile(string nip, string bankAccount)
        {
            string hash = (flatFileData.head.datagenerowaniadanych + nip + bankAccount).SHA512();
            foreach (var item in flatFileData.body)
            {
                if (item.Equals(hash, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
