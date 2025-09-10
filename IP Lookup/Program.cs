using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static readonly HttpClient client = new HttpClient();

    static async Task Main()
    {
        Console.Write("Enter IP: ");
        string ip = Console.ReadLine();

        
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };

        var r = await client.GetAsync($"http://ip-api.com/json/{ip}");
        r.EnsureSuccessStatusCode();
        string json = await r.Content.ReadAsStringAsync();

        
        var ipInfo = JsonSerializer.Deserialize<Response>(json, options);

        if (ipInfo.Status == "fail")
        {
            Console.WriteLine("Request failed.");
            Console.ReadKey();
            return;
        }

        string prettyJson = JsonSerializer.Serialize(ipInfo, options);

        string defaultJson = $"{ipInfo.Query}.json";
        string newJson = $"{ip}.json";

        if (string.IsNullOrEmpty(ip)) {
            await File.WriteAllTextAsync(defaultJson, prettyJson);
            Console.WriteLine($"Saved in {defaultJson}");
        } else
        {
            await File.WriteAllTextAsync(newJson, prettyJson);
            Console.WriteLine($"Saved in {newJson}");
        }

            
        Console.ReadKey();
    }

    public class Response
    {
        public string? Status { get; set; }
        public string? Country { get; set; }
        public string? CountryCode { get; set; }
        public string? Region { get; set; }
        public string? RegionName { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string? Timezone { get; set; }
        public string? Isp { get; set; }
        public string? Org { get; set; }
        public string? As { get; set; }
        public string? Query { get; set; }
        public string _credits { get; set; } = "https://github.com/skun0";
    }


}
