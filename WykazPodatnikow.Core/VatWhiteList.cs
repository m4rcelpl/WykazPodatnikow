using WykazPodatnikow.Data;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WykazPodatnikow.SharedLib;
using Exception = WykazPodatnikow.Data.Exception;

namespace WykazPodatnikow.Core
{
    public class VatWhiteList
    {
        private readonly HttpClient httpClient;
        private JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions {PropertyNameCaseInsensitive = true };

        public VatWhiteList(HttpClient httpClient, string url = "https://wl-api.mf.gov.pl")
        {
            httpClient.BaseAddress = new Uri(url);
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Wyszukiwanie pojedynczego podmiotu po nip.
        /// </summary>
        /// <param name="nip"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<EntityResponse> GetDataFromNipAsync(string nip, DateTime dateTime)
        {
            string GetString = string.Empty;
            try
            {
                if (!Extension.IsValidNIP(nip))
                    return new EntityResponse { Exception = new Exception { Code = "6", Message = $"Invalid Nip" } };

                var Getresult = await httpClient.GetAsync($"/api/search/nip/{nip}?date={dateTime.ToString("yyyy-MM-dd")}");

                GetString = await Getresult.Content.ReadAsStringAsync();

                if (Getresult.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<EntityResponse>(GetString, JsonSerializerOptions);
                }
                return new EntityResponse { Exception = JsonSerializer.Deserialize<Exception>(GetString, JsonSerializerOptions) };
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Wyszukiwanie podmiotow po numerach nip.
        /// </summary>
        /// <param name="nips"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<EntityListResponse> GetDataFromNipsAsync(string nips, DateTime dateTime)
        {
            string GetString = string.Empty;
            try
            {
                foreach (var item in nips.Split(","))
                {
                    if (!Extension.IsValidNIP(item))
                        return new EntityListResponse { Exception = new Exception { Code = "6", Message = $"Invalid Nip: {item}" } };
                }

                var Getresult = await httpClient.GetAsync($"/api/search/nips/{nips}?date={dateTime.ToString("yyyy-MM-dd")}");

                GetString = await Getresult.Content.ReadAsStringAsync();

                if (Getresult.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<EntityListResponse>(GetString, JsonSerializerOptions);
                }
                return new EntityListResponse { Exception = JsonSerializer.Deserialize<Exception>(GetString, JsonSerializerOptions) };
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Wyszukiwanie pojedynczego podmiotu po regon.
        /// </summary>
        /// <param name="regon"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<EntityResponse> GetDataFromRegonAsync(string regon, DateTime dateTime)
        {
            string GetString = string.Empty;
            try
            {
                if (!Extension.IsValidREGON(regon))
                    return new EntityResponse { Exception = new Exception { Code = "6", Message = "Invalid Regon" } };

                var Getresult = await httpClient.GetAsync($"/api/search/regon/{regon}?date={dateTime.ToString("yyyy-MM-dd")}");

                GetString = await Getresult.Content.ReadAsStringAsync();

                if (Getresult.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<EntityResponse>(GetString, JsonSerializerOptions);
                }
                return new EntityResponse { Exception = JsonSerializer.Deserialize<Exception>(GetString, JsonSerializerOptions) };
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Wyszukiwanie podmiotow po numerach regon.
        /// </summary>
        /// <param name="regons"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<EntityListResponse> GetDataFromRegonsAsync(string regons, DateTime dateTime)
        {
            string GetString = string.Empty;
            try
            {
                foreach (var item in regons.Split(","))
                {
                    if (!Extension.IsValidREGON(item))
                        return new EntityListResponse { Exception = new Exception { Code = "6", Message = $"Invalid Regon: {item}" } };
                }

                var Getresult = await httpClient.GetAsync($"/api/search/regons/{regons}?date={dateTime.ToString("yyyy-MM-dd")}");

                GetString = await Getresult.Content.ReadAsStringAsync();

                if (Getresult.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<EntityListResponse>(GetString, JsonSerializerOptions);
                }
                return new EntityListResponse { Exception = JsonSerializer.Deserialize<Exception>(GetString, JsonSerializerOptions) };
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Zwraca EntityListResponse na podstawie podanego konta bankowego.
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<EntityListResponse> GetDataFromBankAccountAsync(string bankAccount, DateTime dateTime)
        {
            string GetString = string.Empty;
            try
            {
                if (!Extension.IsValidBankAccountNumber(bankAccount))
                    return new EntityListResponse { Exception = new Exception { Code = "6", Message = "Invalid Bank Accounts" } };

                var Getresult = await httpClient.GetAsync($"/api/search/bank-account/{bankAccount}?date={dateTime.ToString("yyyy-MM-dd")}");

                GetString = await Getresult.Content.ReadAsStringAsync();

                if (Getresult.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<EntityListResponse>(GetString, JsonSerializerOptions);
                }
                return new EntityListResponse { Exception = JsonSerializer.Deserialize<Exception>(GetString, JsonSerializerOptions) };
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Wyszukiwanie podmiotow po numerach kont.
        /// </summary>
        /// <param name="bankAccounts"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<EntityListResponse> GetDataFromBankAccountsAsync(string bankAccounts, DateTime dateTime)
        {
            string GetString = string.Empty;
            try
            {
                foreach (var item in bankAccounts.Split(","))
                {
                    if (!Extension.IsValidBankAccountNumber(item))
                        return new EntityListResponse { Exception = new Exception { Code = "6", Message = $"Invalid Bank Account: {item}" } };
                }

                var Getresult = await httpClient.GetAsync($"/api/search/bank-accounts/{bankAccounts}?date={dateTime.ToString("yyyy-MM-dd")}");

                GetString = await Getresult.Content.ReadAsStringAsync();

                if (Getresult.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<EntityListResponse>(GetString, JsonSerializerOptions);
                }
                return new EntityListResponse { Exception = JsonSerializer.Deserialize<Exception>(GetString, JsonSerializerOptions) };
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sprawdzenie pojedynczego podmiotu po nip i numerze konta.
        /// </summary>
        /// <param name="nip"></param>
        /// <param name="bankAccount"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<EntityCheckResponse> CheckFromNipAndBankAccountsAsync(string nip, string bankAccount, DateTime dateTime)
        {
            string GetString = string.Empty;
            try
            {
                if (!Extension.IsValidNIP(nip))
                    return new EntityCheckResponse { Exception = new Exception { Code = "6", Message = "Invalid nip" } };

                if (!Extension.IsValidBankAccountNumber(bankAccount))
                    return new EntityCheckResponse { Exception = new Exception { Code = "6", Message = "Invalid Bank Account" } };

                var Getresult = await httpClient.GetAsync($"/api/check/nip/{nip}/bank-account/{bankAccount}?date={dateTime.ToString("yyyy-MM-dd")}");

                GetString = await Getresult.Content.ReadAsStringAsync();

                if (Getresult.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<EntityCheckResponse>(GetString, JsonSerializerOptions);
                }
                return new EntityCheckResponse { Exception = JsonSerializer.Deserialize<Exception>(GetString, JsonSerializerOptions) };
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sprawdzenie pojedynczego podmiotu po regon i numerze konta
        /// </summary>
        /// <param name="regon"></param>
        /// <param name="bankAccount"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<EntityCheckResponse> CheckFromRegonAndBankAccountsAsync(string regon, string bankAccount, DateTime dateTime)
        {
            string GetString = string.Empty;
            try
            {
                if (!Extension.IsValidREGON(regon))
                    return new EntityCheckResponse { Exception = new Exception { Code = "6", Message = "Invalid regon" } };

                if (!Extension.IsValidBankAccountNumber(bankAccount))
                    return new EntityCheckResponse { Exception = new Exception { Code = "6", Message = "Invalid Bank Account" } };

                var Getresult = await httpClient.GetAsync($"/api/check/regon/{regon}/bank-account/{bankAccount}?date={dateTime.ToString("yyyy-MM-dd")}");

                GetString = await Getresult.Content.ReadAsStringAsync();

                if (Getresult.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<EntityCheckResponse>(GetString, JsonSerializerOptions);
                }
                return new EntityCheckResponse { Exception = JsonSerializer.Deserialize<Exception>(GetString, JsonSerializerOptions) };
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}