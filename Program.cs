// See https://aka.ms/new-console-template for more information

// See https://aka.ms/new-console-template for more information
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Lab6
{
    public class Program
    {
        const string token = "07db4fc75de179fef02e094a27cddb14";

        public static async Task<Weather> GetRandomWeather()
        {
            double lat = (Random.Shared.NextDouble() - 0.5) * 2 * 90;
            double lon = (Random.Shared.NextDouble() - 0.5) * 2 * 180;
            HttpClient client = new HttpClient();
            HttpResponseMessage webReq = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={token}");

            try
            {
                webReq.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Unable to get query result");
                return default(Weather);
            }
            string response = await webReq.Content.ReadAsStringAsync();

            var pre.v_culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            //string response = "{\"coord\":{\"lon\":-0.1257,\"lat\":51.5085},\"weather\":[{\"id\":803,\"main\":\"Clouds\",\"description\":\"broken clouds\",\"icon\":\"04d\"}],\"base\":\"stations\",\"main\":{\"temp\":289.81,\"feels_like\":289.25,\"temp_min\":288.53,\"temp_max\":291.31,\"pressure\":1019,\"humidity\":66},\"visibility\":10000,\"wind\":{\"speed\":4.12,\"deg\":230},\"clouds\":{\"all\":75},\"dt\":1665583069,\"sys\":{\"type\":2,\"id\":2075535,\"country\":\"GB\",\"sunrise\":1665555533,\"sunset\":1665594890},\"timezone\":3600,\"id\":2643743,\"name\":\"London\",\"cod\":200}";
            var table = JsonSerializer.Deserialize<JsonNode>(response);
            var weather = new Weather
            {
                Name = table!["name"]?.ToString(),
                Description = table!["weather"]![0]!["description"]!.ToString(),
                Country = table["sys"]?["country"]?.ToString(),
                Temp = float.Parse(table!["main"]!["temp"]!.ToString()),
            };

            Thread.CurrentThread.CurrentCulture = prev_culture;

            return weather;
        }

        public static async Task Main()
        {
            //Weather weather = await GetRandomWeather();
            List<Weather> weathers = new();
            while(weathers.Count < 50)
            {
                Weather weather = await GetRandomWeather();
                if(weather.Name != string.Empty &&
                   weather.Country != string.Empty)
                {
                    weathers.Add(weather);
                    Console.WriteLine($"Added weather!\n{weather}");
                }
            }

            float maxTemp = weathers.Select(x => x.Temp).Max();
            float minTemp = weathers.Select(x => x.Temp).Min();

            float avgTemp = weathers.Select(x => x.Temp).Average();

            int uniqueCountries = weathers.GroupBy(x => x.Country).Select(x => x.First()).Count();

            Weather weatherClearSky = weathers.Find(x => x.Description.Equals("clear sky"));
            Weather weatherRain = weathers.Find(x => x.Description.Equals("rain"));
            Weather weatherFewClouds = weathers.Find(x => x.Description.Equals("few clouds"));


            Console.WriteLine($"""
                Max temp : {maxTemp},
                Min temp : {minTemp},
                Average temp : {avgTemp},
                Unique countries : {uniqueCountries},
                Country, name with clear sky : {weatherClearSky.Country}, {weatherClearSky.Name},
                Country, name with rain : {weatherRain.Country}, {weatherRain.Name},
                Country, name with few clouds : {weatherFewClouds.Country}, {weatherFewClouds.Name},
                """);

            Console.ReadKey();
        }
    }
}
