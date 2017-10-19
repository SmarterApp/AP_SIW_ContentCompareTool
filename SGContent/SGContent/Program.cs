using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SGContent
{
    class Program
    {
        static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
              .AddConsole()
              .AddDebug();
            ILogger logger = loggerFactory.CreateLogger<Program>();

            string appSettingsUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/master/SmarterBalanced.SampleItems/src/SmarterBalanced.SampleItems.Web/appsettings.json";
            string itemsPatchUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/master/SmarterBalanced.SampleItems/ClaimConfigurations/ItemsPatch.xml";
            SaveDependency(appSettingsUrl, "appsettings.json");
            SaveDependency(itemsPatchUrl, "ItemsPatch.xml");

            Console.WriteLine(Startup.Instance.AppSettings.SbContent.ContentRootDirectory);
            logger.LogInformation("Test");
        }

        static void SaveDependency(string url, string fileName)
        {
            using (var client = new HttpClient())
            {
                using (var result = client.GetAsync(url).Result)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        string contents = result.Content.ReadAsStringAsync().Result;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                        File.WriteAllText(path, contents);
                    }
                }
            }
        }
    }
}
