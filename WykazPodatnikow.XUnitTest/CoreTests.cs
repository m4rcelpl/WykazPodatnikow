using System;
using System.Net.Http;
using System.Threading.Tasks;
using WykazPodatnikow.Core;
using WykazPodatnikow.Data;
using Xunit;
using Exception = System.Exception;

namespace WykazPodatnikow.XUnitTest
{
    public class CoreTest
    {
        private readonly VatWhiteList vatWhiteList;
        private readonly VatWhiteListFlatFile vatWhiteListFlatFile;

        public CoreTest()
        {
            try
            {
                vatWhiteList = new VatWhiteList(new HttpClient());
                vatWhiteListFlatFile = new VatWhiteListFlatFile(@"C:\Users\mgarbarczyk\Desktop\20191021.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while initialize vatWhiteList or vatWhiteListFlatFile. {ex.Message} | {ex.InnerException?.Message}");
            }
        }

        [Fact]
        public void FoundInVirtualFlatFile()
        {
            Assert.Equal(FlatFile.FoundInVirtual, vatWhiteListFlatFile.IsInFlatFile("4356579386", "20721233708680000022663112"));
        }

        [Fact]
        public void FoundInRegularFlatFile()
        {
            Assert.Equal(FlatFile.FoundInRegular, vatWhiteListFlatFile.IsInFlatFile("1435721230", "34102012221314181237774212"));
        }

        [Theory]
        [InlineData("6222468959")]
        public async Task GetDataFromNip_GoodNip(string nip)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.GetDataFromNipAsync(nip, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.NotNull(result.Result.RequestId);
            Assert.Contains(nip, result.Result.Subject.Nip);
        }

        [Theory]
        [InlineData("123456")]
        public async Task GetDataFromNip_BadNip(string nip)
        {
            var result = await vatWhiteList.GetDataFromNipAsync(nip, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }

        [Theory]
        [InlineData("6222468959,7811835788,5423288666", 3)]
        public async Task GetDataFromNips_GoodNips(string nips, int ReturnCount)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.GetDataFromNipsAsync(nips, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.True(result.Result.Subjects.Count == ReturnCount);
            Assert.NotNull(result.Result.RequestId);
        }

        [Theory]
        [InlineData("12345,123456,1234567")]
        public async Task GetDataFromNips_BaadNips(string nips)
        {
            var result = await vatWhiteList.GetDataFromNipsAsync(nips, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }

        [Theory]
        [InlineData("251546820")]
        public async Task GetDataFromRegon_GoodRegon(string regon)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.GetDataFromRegonAsync(regon, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.NotNull(result.Result.RequestId);
            Assert.Contains(regon, result.Result.Subject.Regon);
        }

        [Theory]
        [InlineData("12345")]
        public async Task GetDataFromRegon_BadRegon(string regon)
        {
            var result = await vatWhiteList.GetDataFromRegonAsync(regon, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }

        [Theory]
        [InlineData("251546820,301087548,368793892", 3)]
        public async Task GetDataFromRegons_GoodRegons(string regons, int ReturnCount)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.GetDataFromRegonsAsync(regons, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.True(result.Result.Subjects.Count == ReturnCount);
            Assert.NotNull(result.Result.RequestId);
        }

        [Theory]
        [InlineData("251546820,12345,368793892")]
        public async Task GetDataFromRegons_BadRegons(string regons)
        {
            var result = await vatWhiteList.GetDataFromRegonsAsync(regons, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }

        [Theory]
        [InlineData("23109011600000000101980438")]
        public async Task GetDataFromBankAccount_GoodBA(string bankaccount)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.GetDataFromBankAccountAsync(bankaccount, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.NotNull(result.Result.RequestId);
        }

        [Theory]
        [InlineData("1234567890")]
        public async Task GetDataFromBankAccount_BadBA(string bankaccount)
        {
            var result = await vatWhiteList.GetDataFromBankAccountAsync(bankaccount, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }

        [Theory]
        [InlineData("74175010190000000011453438,23109011600000000101980438,50109025900000000135521483", 3)]
        public async Task GetDataFromBankAccounts_GoodBAs(string bankaccounts, int ReturnCount)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.GetDataFromBankAccountsAsync(bankaccounts, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.True(result.Result.Subjects.Count == ReturnCount);
            Assert.NotNull(result.Result.RequestId);
        }

        [Theory]
        [InlineData("74175010190000000011453438,11234567890,50109025900000000135521483")]
        public async Task GetDataFromBankAccounts_BadBAs(string bankaccounts)
        {
            var result = await vatWhiteList.GetDataFromBankAccountsAsync(bankaccounts, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }

        [Theory]
        [InlineData("6222468959", "92103011460000000086837021")]
        public async Task CheckFromNipAndBankAccounts_GoodCheck(string nip, string bankaccount)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.CheckFromNipAndBankAccountsAsync(nip, bankaccount, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.NotNull(result.Result.RequestId);
            Assert.Contains(result.Result.AccountAssigned, "TAK");
        }

        [Theory]
        [InlineData("6222468959", "39114010100000777770001001")]
        public async Task CheckFromNipAndBankAccounts_NotEqual(string nip, string bankaccount)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.CheckFromNipAndBankAccountsAsync(nip, bankaccount, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.NotNull(result.Result.RequestId);
            Assert.Contains(result.Result.AccountAssigned, "NIE");
        }

        [Theory]
        [InlineData("123456", "39114010100000777770001001")]
        public async Task CheckFromNipAndBankAccounts_BadNip(string nip, string bankaccount)
        {
            var result = await vatWhiteList.CheckFromNipAndBankAccountsAsync(nip, bankaccount, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }

        [Theory]
        [InlineData("6222468959", "123456789062444")]
        public async Task CheckFromNipAndBankAccounts_BadBA(string nip, string bankaccount)
        {
            var result = await vatWhiteList.CheckFromNipAndBankAccountsAsync(nip, bankaccount, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }

        [Theory]
        [InlineData("251546820", "92103011460000000086837021")]
        public async Task CheckFromRegonAndBankAccounts_GoodCheck(string regon, string bankaccount)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.CheckFromRegonAndBankAccountsAsync(regon, bankaccount, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.NotNull(result.Result.RequestId);
            Assert.Contains(result.Result.AccountAssigned, "TAK");
        }

        [Theory]
        [InlineData("251546820", "39114010100000777770001001")]
        public async Task CheckFromRegonAndBankAccounts_BadEqual(string regon, string bankaccount)
        {
            await Task.Delay(2000);
            var result = await vatWhiteList.CheckFromRegonAndBankAccountsAsync(regon, bankaccount, DateTime.Now);

            Assert.NotNull(result.Result);
            Assert.NotNull(result.Result.RequestId);
            Assert.Contains(result.Result.AccountAssigned, "NIE");
        }

        [Theory]
        [InlineData("12345678", "39114010100000777770001001")]
        public async Task CheckFromRegonAndBankAccounts_BadRegon(string regon, string bankaccount)
        {
            var result = await vatWhiteList.CheckFromRegonAndBankAccountsAsync(regon, bankaccount, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }

        [Theory]
        [InlineData("251546820", "1234564321232342355")]
        public async Task CheckFromRegonAndBankAccounts_BadBA(string regon, string bankaccount)
        {
            var result = await vatWhiteList.CheckFromRegonAndBankAccountsAsync(regon, bankaccount, DateTime.Now);

            Assert.NotNull(result.Exception);
            Assert.Contains("6", result.Exception.Code);
        }
    }
}