using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
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
                throw new System.Exception("Json file not found");

            try
            {
                flatFileData = JsonSerializer.Deserialize<FlatFileData>(File.ReadAllText(PathToJson), JsonSerializerOptions);
            }
            catch (System.Exception)
            {
                throw;
            }

            if (string.IsNullOrEmpty(flatFileData.head.datagenerowaniadanych))
                throw new System.Exception("Invalide Json file, datagenerowaniadanych is empty");
        }

        public FlatFile IsInFlatFile(string nip, string bankAccount)
        {
            if (!nip.IsValidNIP() || !Extension.IsValidBankAccountNumber(bankAccount))
                return FlatFile.NotFound;

            if (CheckInBody(bankAccount))
                return FlatFile.FoundInRegular;

            string bankBranchNumber = bankAccount.Substring(2, 8);
            string maskToCompare = string.Empty;

            foreach (var item in flatFileData.masks)
            {
                if (bankBranchNumber.Equals(item.Substring(2, 8), StringComparison.OrdinalIgnoreCase))
                {
                    maskToCompare = item;
                    break;
                }
            }

            if (string.IsNullOrEmpty(maskToCompare))
                return FlatFile.NotFound;

            int IndexFrom = maskToCompare.IndexOf("Y");
            int range = maskToCompare.Count(p => p.Equals('Y'));
            string VirtualAccount = Regex.Replace(maskToCompare, "Y.Y", bankAccount.Substring(IndexFrom, range));

            if (CheckInBody(VirtualAccount))
                return FlatFile.FoundInVirtual;
            else
                return FlatFile.NotFound;

            bool CheckInBody(string account)
            {
                string hash = (flatFileData.head.datagenerowaniadanych + nip + account).SHA512();

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
}