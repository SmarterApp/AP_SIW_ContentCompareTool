using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace SGContent
{
    public class ConfigurationProvider
    {
        public IConfigurationRoot Configuration { get; }
        public AppSettings AppSettings { get; }
        private readonly ILogger logger;

        public ConfigurationProvider(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ConfigurationProvider>();
            string appSettingsUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/master/SmarterBalanced.SampleItems/src/SmarterBalanced.SampleItems.Web/appsettings.json";
            string itemsPatchUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/master/SmarterBalanced.SampleItems/ClaimConfigurations/ItemsPatch.xml";

            SaveDependency(appSettingsUrl, "appsettings.json");
            SaveDependency(itemsPatchUrl, "ItemsPatch.xml");
            logger.LogInformation("Successfully downloaded dependencies");

            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Override.json", optional: true);
            Configuration = builder.Build();
            AppSettings appSettings = new AppSettings();

            Configuration.Bind(appSettings);
            AppSettings = appSettings;
            logger.LogInformation("Successfully loaded configuration");
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
