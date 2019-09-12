# 💰 Podatnicy VAT

Biblioteka do odczytu danych z API białej listy podatników VAT.

Helper library to get data from Polish ministry of finance about VAT Taxpayer.

# 👉 Wersje

Biblioteka jest udostępniona w dwóch wersjach:

[**.NET Core 3 library**](https://www.nuget.org/packages/WykazPodatnikow.Core/) - ta wersja korzysta z szybszego i wbudowanego w framework `System.Text.Json`

[**.NET Standard 2.1**](https://www.nuget.org/packages/WykazPodatnikow.Standard/) - standardowa wersja korzystająca z `Newtonsoft.Json`

# 🤝 Zgodność
Cała struktura danych w przestrzeni nazw `BialaLista.data` - jest w 100% zgodna ze specyfikacją [opublikowaną przez ministerstwo.](https://wl-api.mf.gov.pl/) 

# 👨‍💻 Jak korzystać

Zainstaluj bibliotekę z menagera NuGet.

[WykazPodatnikow.Core](https://www.nuget.org/packages/WykazPodatnikow.Core/)<br>
`dotnet add package WykazPodatnikow.Core`

[WykazPodatnikow.Standard](https://www.nuget.org/packages/WykazPodatnikow.Standard/)<br>
`dotnet add package WykazPodatnikow.Standard`

Dodaj:<br>
`using WykazPodatnikow.Core;`<br>
lub<br> 
`using WykazPodatnikow.Standard;`<br>

Następnie zainicjuj klasę:

Jako argument trzeba przekazać HttpClient, można dodać jako nową instancję:
``` csharp
var vatWhiteList = new VatWhiteList(new HttpClient());
```

lub z dependency injection:

```csharp
public class SomeClass
{
    private readonly HttpClient httpClient;

    public SomeClass(HttpClient httpClient)
    {
       this.httpClient = httpClient;
    }

    public void ReadVatData()
    {
        var vatWhiteList = new VatWhiteList(httpClient);
    }
}
```

Jako drugi argument można podać adres API. Domyślnie jest wpisany produkcyjny https://wl-api.mf.gov.pl Można go nadpisać testowym https://wl-test.mf.gov.pl:9091/wykaz-podatnikow/ lub zmodyfikować gdyby w przyszłości się zmienił. 

```csharp
var vatWhiteList = new VatWhiteList(new HttpClient(), "https://wl-test.mf.gov.pl:9091/wykaz-podatnikow/");
```

Po inicjalizacji można zacząć korzystać ze wszystkich metod.

```csharp
EntityResponse result = await vatWhiteList.GetDataFromNipAsync("[NIP]", DateTime.Now); //Wyszukiwanie pojedyńczego podmiotu po nip.

EntityListResponse result = await vatWhiteList.GetDataFromNipsAsync("[NIPy]", DateTime.Now); //Wyszukiwanie podmiotów po numerach nip.

EntityResponse result = await vatWhiteList.GetDataFromRegonAsync("[Regon]", DateTime.Now); //Wyszukiwanie pojedyńczego podmiotu po regon.

EntityListResponse result = await vatWhiteList.GetDataFromRegonsAsync("[Regony]", DateTime.Now); //Wyszukiwanie podmiotów po numerach regon.

EntityResponse result = await vatWhiteList.GetDataFromBankAccountAsync("[Konto bankowe]", DateTime.Now); //Wyszukiwanie podmiotuw po numerze konta.

EntityListResponse result = await vatWhiteList.GetDataFromBankAccountsAsync("[Konta bankowe]", DateTime.Now); //Wyszukiwanie podmiotów po numerach kont.

EntityCheckResponse result = await vatWhiteList.CheckFromNipAndBankAccountsAsync("[NIP]","[Konta bankowe]", DateTime.Now); //Sprawdzenie pojedyńczego podmiotu po nip i numerze konta.

EntityCheckResponse result = await vatWhiteList.CheckFromRegonAndBankAccountsAsync("[Regon]","[Konta bankowe]", DateTime.Now); //Sprawdzenie pojedyńczego podmiotu po regon i numerze konta.

```

Wartość DateTime można podać przeszłą, np. `DateTime.Now.AddDays(-7)` - pokaże dane z przed tygodnia.

Szczegółowy opis API można znaleźć na stronach ministerstwa: https://www.gov.pl/web/kas/api-wykazu-podatnikow-vat

## 🧨 Uwaga
Każda metoda musi być umieszczona w bloku `try...catch`. W przypadku problemów z serwerem zostanie rzucony wyjątek. W każdym innym przypadku błędy są zgłaszane w klasie `Exception`.
```sharp
EntityResponse.Exception 
EntityListResponse.Exception
EntityCheckResponse.Exception
```


***Baza ministerstwa jest obecnie w fazie rozwoju i często zdarzają się przerwy w jej działaniu.***

# 📜 Pełny przykład

```csharp
 private static async System.Threading.Tasks.Task Main(string[] args)
        {
            string nip = "5270103391";
            string regon = "010016565";
            string bankaccount = "72103015080000000500217006";

            Console.WriteLine("Start!");

            var vatWhiteList = new VatWhiteList(new HttpClient());

            try
            {
                Console.WriteLine($"Sprawdzam firmę na podstawie NIP: {nip}");
                var resultNip = await vatWhiteList.GetDataFromNipAsync(nip, DateTime.Now);

                if (resultNip.Exception is null)
                {
                    Console.WriteLine($"Id sprawdzenia: {resultNip.Result?.RequestId}");
                    Console.WriteLine($"Nazwa firmy: {resultNip.Result?.Subject.Name}");
                    Console.WriteLine($"Regon: {resultNip.Result?.Subject.Regon}");
                    Console.WriteLine($"Status VAT: {resultNip.Result?.Subject.StatusVat}");
                    Console.WriteLine($"Konta bankowe:");
                    foreach (var item in resultNip.Result?.Subject?.AccountNumbers)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"Wystąpił błąd podczas sprawdzania: Kod {resultNip.Exception.Code} | Komunikat: {resultNip.Exception.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Błąd] {ex.Message}");
            }

            try
            {
                Console.WriteLine($"Sprawdzam firmę na podstawie Regon: {regon}");
                var resultRegon = await vatWhiteList.GetDataFromRegonAsync(regon, DateTime.Now);

                if (resultRegon.Exception is null)
                {
                    Console.WriteLine($"Id sprawdzenia: {resultRegon.Result?.RequestId}");
                    Console.WriteLine($"Nazwa firmy: {resultRegon.Result?.Subject.Name}");
                    Console.WriteLine($"Regon: {resultRegon.Result?.Subject.Regon}");
                    Console.WriteLine($"Status VAT: {resultRegon.Result?.Subject.StatusVat}");
                    Console.WriteLine($"Konta bankowe:");
                    foreach (var item in resultRegon.Result?.Subject.AccountNumbers)
                    {
                        Console.WriteLine(item);
                    }
                }
                else
                {
                    Console.WriteLine($"Wystąpił błąd podczas sprawdzania: Kod {resultRegon.Exception.Code} | Komunikat: {resultRegon.Exception.Message}");
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Błąd] {ex.Message}");
            }

            try
            {
                Console.WriteLine($"Sprawdzam parę nip: {nip} i numeru konta: {bankaccount}");
                var resultCheckNip = await vatWhiteList.CheckFromNipAndBankAccountsAsync(nip, bankaccount, DateTime.Now);

                if (resultCheckNip.Exception is null)
                {
                    Console.WriteLine($"Id sprawdzenia: {resultCheckNip.Result?.RequestId}");
                    Console.WriteLine($"Zwrócony status: {resultCheckNip.Result?.AccountAssigned}");
                }
                else
                {
                    Console.WriteLine($"Wystąpił błąd podczas sprawdzania: Kod {resultCheckNip.Exception.Code} | Komunikat: {resultCheckNip.Exception.Message}");
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Błąd] {ex.Message}");
            }

            try
            {
                Console.WriteLine($"Sprawdzam parę regon: {regon} i numeru konta: {bankaccount}");
                var resultCheckRegon = await vatWhiteList.CheckFromRegonAndBankAccountsAsync(regon, bankaccount, DateTime.Now);

                if (resultCheckRegon.Exception is null)
                {
                    Console.WriteLine($"Id sprawdzenia: {resultCheckRegon.Result?.RequestId}");
                    Console.WriteLine($"Zwrócony status: {resultCheckRegon.Result?.AccountAssigned}");
                }
                else
                {
                    Console.WriteLine($"Wystąpił błąd podczas sprawdzania: Kod {resultCheckRegon.Exception.Code} | Komunikat: {resultCheckRegon.Exception.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Błąd] {ex.Message}");
            }

            Console.ReadLine();
        }
```

 # 📝 To-Do

* Dodanie obsługi szyfrowanego [pliku](https://www.gov.pl/web/kas/api-wykazu-podatnikow-vat).
