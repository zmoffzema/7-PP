using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class CurrencyConverter
{
    // Метод для получения текущего курса валют
    static async Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency)
    {
        // Замените YOUR_API_KEY на ваш ключ API от Open Exchange Rates или другого сервиса
        string apiKey = "YOUR_API_KEY";
        string url = $"https://openexchangerates.org/api/latest.json?app_id={apiKey}&base={fromCurrency}&symbols={toCurrency}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);
            decimal exchangeRate = json["rates"][toCurrency].Value<decimal>();

            return exchangeRate;
        }
    }

    static async Task Main(string[] args)
    {
        Console.WriteLine("Конвертер валют");

        // Запрашиваем у пользователя исходную валюту, целевую валюту и сумму
        Console.Write("Введите исходную валюту (например, USD): ");
        string fromCurrency = Console.ReadLine().ToUpper();

        Console.Write("Введите целевую валюту (например, EUR): ");
        string toCurrency = Console.ReadLine().ToUpper();

        Console.Write("Введите сумму для конвертации: ");
        decimal amount;
        if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
        {
            try
            {
                // Получаем текущий курс валют и конвертируем сумму
                decimal exchangeRate = await GetExchangeRate(fromCurrency, toCurrency);
                decimal convertedAmount = amount * exchangeRate;

                // Выводим результат
                Console.WriteLine($"{amount} {fromCurrency} = {convertedAmount:F2} {toCurrency}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении курса валют: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Пожалуйста, введите корректную сумму.");
        }
    }
}
