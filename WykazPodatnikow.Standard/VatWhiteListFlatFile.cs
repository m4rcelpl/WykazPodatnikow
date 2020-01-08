using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WykazPodatnikow.Data;
using WykazPodatnikow.SharedLib;

namespace WykazPodatnikow.Standard
{
    public class VatWhiteListFlatFile
    {
        public static FlatFileData flatFileData;

        public static Task LoadFlatFileAsync(string PathToJson)
        {
            Task task = Task.Run(() =>
            {
                if (!File.Exists(PathToJson))
                    throw new System.Exception("Json file not found");

                try
                {
                    using (FileStream s = new FileStream(PathToJson, FileMode.Open, FileAccess.Read))
                    using (StreamReader sr = new StreamReader(s))
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        JsonSerializer serializer = new JsonSerializer();

                        flatFileData = serializer.Deserialize<FlatFileData>(reader);
                    }
                }
                catch (System.Exception)
                {
                    throw;
                }

                if (flatFileData == null)
                    throw new System.Exception("Deserialize error, check if Json file is valide");

                if (string.IsNullOrEmpty(flatFileData.naglowek?.datagenerowaniadanych))
                    throw new System.Exception("Invalide Json file, datagenerowaniadanych is empty");

                if (flatFileData.maski == null || flatFileData.maski.Count <= 0)
                    throw new System.Exception("Invalide Json file, maski is empty");

                if (flatFileData.skrotypodatnikowczynnych == null || flatFileData.skrotypodatnikowczynnych.Count <= 0)
                    throw new System.Exception("Invalide Json file, skrotypodatnikowczynnych is empty");

                if (flatFileData.skrotypodatnikowzwolnionych == null || flatFileData.skrotypodatnikowzwolnionych.Count <= 0)
                    throw new System.Exception("Invalide Json file, skrotypodatnikowzwolnionych is empty");
            });

            return task;
        }

        public FlatFile IsInFlatFile(string nip, string bankAccount)
        {
            if (!(flatFileData?.skrotypodatnikowczynnych?.Count >= 0))
                throw new System.Exception("Json file is not loaded. Use first LoadFlatFileAsync()");

            if (!nip.IsValidNIP())
                return FlatFile.InvalidNip;

            if (string.IsNullOrEmpty(bankAccount) || string.IsNullOrWhiteSpace(bankAccount) || bankAccount?.Length < 26)
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
                    int IndexFrom = item.IndexOf("Y");
                    int range = item.Count(p => p.Equals('Y'));
                    string VirtualAccount = Regex.Replace(item, "Y\\w*Y", bankAccount.Substring(IndexFrom, range));

                    FlatFile checkResult = CheckInBody(VirtualAccount);

                    if (checkResult == FlatFile.NotFound)
                        continue;
                    else
                        return checkResult;
                }
            }

            return FlatFile.NotFound;

            FlatFile CheckInBody(string account)
            {
                string hash = (flatFileData.naglowek.datagenerowaniadanych + nip + account).SHA512(Convert.ToInt32(flatFileData.naglowek.liczbatransformacji));

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