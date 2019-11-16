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
        private readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = new NamingPolicy()};
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

            if (string.IsNullOrEmpty(flatFileData.naglowek.datagenerowaniadanych))
                throw new System.Exception("Invalide Json file, datagenerowaniadanych is empty");
        }

        public FlatFile IsInFlatFile(string nip, string bankAccount)
        {
            if (!nip.IsValidNIP())
                return FlatFile.InvalidNip;

            if (!Extension.IsValidBankAccountNumber(bankAccount))
                return FlatFile.InvalidBankAccount;

            switch (CheckInBody(bankAccount))
            {
                case FlatFile.FoundInActiveVatPayer:
                    return FlatFile.FoundInActiveVatPayer;

                case FlatFile.FoundInExemptVatPayer:
                    return FlatFile.FoundInExemptVatPayer;

                case FlatFile.InvalidNip:
                    return FlatFile.InvalidNip;

                case FlatFile.InvalidBankAccount:
                    return FlatFile.InvalidBankAccount;

                case FlatFile.NotFound:
                    break;

                default:
                    break;
            }

            string bankBranchNumber = bankAccount.Substring(2, 8);
            string maskToCompare = string.Empty;

            foreach (var item in flatFileData.maski)
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

            return (CheckInBody(VirtualAccount)) switch
            {
                FlatFile.FoundInActiveVatPayer => FlatFile.FoundInActiveVatPayer,
                FlatFile.FoundInExemptVatPayer => FlatFile.FoundInExemptVatPayer,
                FlatFile.InvalidNip => FlatFile.InvalidNip,
                FlatFile.InvalidBankAccount => FlatFile.InvalidBankAccount,
                FlatFile.NotFound => FlatFile.NotFound,
                _ => FlatFile.NotFound,
            };

            FlatFile CheckInBody(string account)
            {
                string hash = (flatFileData.naglowek.datagenerowaniadanych + nip + account).SHA512();

                foreach (var item in flatFileData.skrotypodatnikowczynnych)
                {
                    if (item.Equals(hash, StringComparison.OrdinalIgnoreCase))
                    {
                        return FlatFile.FoundInActiveVatPayer;
                    }
                }

                foreach (var item in flatFileData.skrotypodatnikowzwolnionych)
                {
                    if (item.Equals(hash, StringComparison.OrdinalIgnoreCase))
                    {
                        return FlatFile.FoundInExemptVatPayer;
                    }
                }

                return FlatFile.NotFound;
            }
        }
    }
}